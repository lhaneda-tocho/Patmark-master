using System;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.EmbossmentKit;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using Android.App;
using Android.Widget;

using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.Patmark.Settings;

using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.Droid
{
    public class CommunicationClientController: BaseCommunicationClientController
    {
        
        // (Clean | Connect)->Unit
        public event Action<CommunicationRound>  OnCompleted = null;

        public CommunicationClientController(CommunicationClient theClient = null) : base(theClient)
        {
        }

        public override void OnStateChanged(ConnectionState next, ConnectionState prev)
        {
            base.OnStateChanged(next, prev);
            switch(next) {
                case ConnectionState.Ready:
                    if (prev != ConnectionState.Ready)
                    {
                        ToastShowWithR(Resource.String.toast_communication_started);
                    }
                    break;
                case ConnectionState.NotExcluding:
                    if(prev == ConnectionState.Ready){
                        ToastShowWithR(Resource.String.toast_communication_terminated);
                    }
                    break;
            }
        }


        public override void OnFailOpening(ExclusionError error)
        {
            base.OnFailOpening(error);
            switch (error)
            {
                case ExclusionError.BrokenPipe:
                    ToastShowWithR(Resource.String.toast_communication_timed_out, ToastLength.Long);
                    break;
                case ExclusionError.ExcludedYet:
                    ToastShowWithR(Resource.String.toast_communication_excluded_me, ToastLength.Long);
                    break;
            }
        }

        public override void OnFailReleasing(ExclusionError error)
        {
            base.OnFailReleasing(error);
            switch (error)
            {
                case ExclusionError.BrokenPipe:
                    ToastShowWithR(Resource.String.toast_communication_timed_out, ToastLength.Long);
                    break;
                case ExclusionError.ExcludedYet:
                    ToastShowWithR(Resource.String.toast_communication_excluded_me, ToastLength.Long);
                    break;
            }
        }

        void ToastShowWithR(int resource, ToastLength duration = ToastLength.Short)
        {
            PatmarkApplication.FragmentAccessHandler.Post(()=>
            {
                Toast
                    .MakeText(PatmarkApplication.CurrentActivity, resource, duration)
                    .Show();
            });
        }

        /// <summary>
        /// コントローラからあらかじめデータを取ってくるフェーズ.
        /// Activity側のコールバックよりも先に呼ばれるため，表示の反映前の処理で利用する.
        /// </summary>
        /// <returns>The controller data.</returns>
        async Task ReadControllerData()
        {
            try
            {
                // カレンダーとシリアルを取得します.
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
            await PatmarkApplication.RunOnMain(
                terminator: true,
                async () =>
                {
                    var repo = new CombinedMachineModelNoRepository();
                    var ceeckResult = await repo.IsControllerModelMatchedWith();

                    await ceeckResult.Aggregate<CommunicationResult<bool>, Task>(
                        onFailure: (_) =>
                        {
                            return PatmarkApplication.RunOnMain(() =>
                            {
                                int id = Resource.String.toast_appsetting_controller_fail_to_check;
                                Toast.MakeText(Application.Context, id, ToastLength.Long).Show();
                            });
                        },
                        onSuccess: (result) =>
                        {
                            if (!result.Value)
                            {
                                return PatmarkApplication.RunOnMain(() =>
                                {
                                    int id = Resource.String.toast_appsetting_controller_different;
                                    Toast.MakeText(Application.Context, id, ToastLength.Long).Show();
                                });
                            }
                            else
                            {
                                return Task.CompletedTask;
                            }
                        }
                    );
                }
            );
        }

        Task FireCompletion(CommunicationRound round)
        {
            var task = PatmarkApplication.RunOnMain(() =>
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
            return PatmarkApplication.RunOnMain(async () =>
            {
                var tag = DateTime.Now + "CommunicationController";
                await ProgressDialogManager.ShowAndProcessOnBackground(tag, task);
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
