using System;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;
using TokyoChokoku.Patmark.iOS.Presenter.Utility;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.Settings;

using TokyoChokoku.Patmark.Common;


namespace TokyoChokoku.Patmark.iOS
{
    using Presenter;
    using TokyoChokoku.MarkinBox.Sketchbook;

    public class CommunicationClientController: BaseCommunicationClientController
    {
        /// <summary>
        /// 一度だけ出せるダイアログの表示用クラス.
        /// このクラスのメソッドはすべてUIスレッド上で実行される必要があります. (コンストラクタを除く)
        /// </summary>
        class OneShotDialogLatch
        {
            // もう表示したかどうか
            public bool Shown { get; private set; } = false;
            // 表示すべきかどうか.
            public bool NeededToShow { get; private set; } = false;
            // 表示許可
            public bool AllowedToShow { get; private set; } = false;
            // 表示コールバック
            public Action ShowCallback { get; set; } = null;

            public void AllowsToShow() {
                if (Shown)
                    return;
                if (NeededToShow)
                {
                    Show();
                }
                else
                {
                    AllowedToShow = true;
                }
            }

            public void NeedsToShow() {
                if (Shown)
                    return;
                if (AllowedToShow)
                {
                    Show();
                }
                else
                {
                    NeededToShow = true;
                }
            }

            void Show()
            {
                try
                {
                    ShowCallback?.Invoke();
                } finally {
                    Shown = true;
                }
            }
        }

        // (Clean | Connect)->Unit
        public event Action<CommunicationRound> OnCompleted = null;

        // Listeners
        [Obsolete("コールバックされなくなりました", true)]
        public event Action OnChangeMachineModel = null;

        readonly OneShotDialogLatch DetectMachineModelDialogLatch = new OneShotDialogLatch();



        public CommunicationClientController(CommunicationClient theClient = null): base(theClient)
        {
        }


        public override void OnStateChanged(ConnectionState next, ConnectionState prev)
        {
            base.OnStateChanged(next, prev);
            switch (next)
            {
                case ConnectionState.Ready:
                    ToastShowWithR("communication.start", 1800);
                    break;
                case ConnectionState.NotExcluding:
                    if(prev == ConnectionState.Ready)
                        ToastShowWithR("communication.terminate", 1800);
                    break;
            }
        }


        public override void OnFailOpening(ExclusionError error)
        {
            base.OnFailOpening(error);
            switch(error)
            {
                case ExclusionError.BrokenPipe:
                    ToastShowWithR("There is no response from the Patmark. Check the connection.", 1800);
                    break;
                case ExclusionError.ExcludedYet:
                    ToastShowWithR("communication.excluded-me", 6000);
                    break;
            }
        }

        public override void OnFailReleasing(ExclusionError error)
        {
            base.OnFailReleasing(error);
            switch (error)
            {
                case ExclusionError.BrokenPipe:
                    ToastShowWithR("There is no response from the Patmark. Check the connection.", 1800);
                    break;
                case ExclusionError.ExcludedYet:
                    // 起きない
                    ToastShowWithR("communication.excluded-me", 6000);
                    break;
            }
        }

        void ToastShowWithR(string resource, int duration) {
            MyToast.ShowMessage(resource, duration: duration / 1000.0);
        }

        async Task ReadControllerData()
        {
            try
            {
                var ss = await CRGlobalSetting.RetrieveFromController();
                var cs = await SerialGlobalSetting.RetrieveFromController(null);
                // Debug: デバック用のロギング
                CRGlobalSetting.Log();
                SerialGlobalSetting.Log();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        async Task SyncController()
        {
            await AppDelegate.RunOnMain(
                terminator: true,
                async () =>
                {
                    var repo = new CombinedMachineModelNoRepository();
                    var ceeckResult = await repo.IsControllerModelMatchedWith();

                    await ceeckResult.Aggregate<CommunicationResult<bool>, Task>(
                        onFailure: (_) =>
                        {
                            return AppDelegate.RunOnMain(() =>
                            {
                                MyToast.ShowMessage("appsetting.controller.fail_to_check", MyToast.Long);
                            });
                        },
                        onSuccess: (result) =>
                        {
                            if(!result.Value)
                            {
                                return AppDelegate.RunOnMain(() =>
                                {
                                    
                                    MyToast.ShowMessage("appsetting.controller.different", MyToast.Short);
                                });
                            } else
                            {
                                return Task.CompletedTask;
                            }
                        }
                    );
                }
            );
        }

        /// <summary>
        /// ダイアログ表示許可
        /// </summary>
        public void AllowToShowDialog()
        {
            DetectMachineModelDialogLatch.AllowsToShow();
        }

        Task FireCompletion(CommunicationRound round) {
            var task = AppDelegate.RunOnMain(() =>
            {
                try
                {
                    OnCompleted?.Invoke(round);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error on OnCompleted.");
                    Console.Error.WriteLine(ex);
                }
            });
            return task;
        }


        protected override Task RunBackground(Func<Task> task)
        {
            return AppDelegate.RunOnMain(async ()=>
            {
                await LoadingOverlay.ShowWithTask(task);
            });
        }

        protected override async Task DidTerminate()
        {
            await FireCompletion(CommunicationRound.Clean);
        }

        protected override async Task DoReady()
        {
            EmbossmentToolKit.InitGlobalIfNeeded();
            await ReadControllerData();
            await SyncController();
            await FireCompletion(CommunicationRound.Connect);
        }

        protected override async Task DoClean()
        {
            EmbossmentToolKit.DeleteGlobal();
        }
    }
}
