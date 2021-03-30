using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using static UIKit.UIViewAutoresizing;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.iOS.Presenter.FieldPreview;
using TokyoChokoku.Patmark.iOS.Presenter.FieldEditor;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using System.Linq;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    public partial class EmbossmentController : UIViewController, FieldAccepter
    {
        class WifiColorManagerWithConnectionState : ConnectionStateListener
        {
            bool enable = true;
            readonly EmbossmentController TheParent;
            readonly UIButton TheButton;
            UIColor OfflineColor = UIColor.LightGray;
            UIColor NotExcludingColor = UIColor.Orange;
            UIColor OnlineColor = UIColor.Green;

            public WifiColorManagerWithConnectionState(EmbossmentController theParent, UIButton button)
            {
                TheParent = theParent; // inject
                TheButton = button;
                button.TintColor = OfflineColor;
            }

            public void OnFailOpening(ExclusionError error)
            {
            }

            public void OnFailReleasing(ExclusionError error)
            {
            }

            void ChangeColor(ConnectionState current)
            {
                var button = TheButton;
                switch (current)
                {
                    case ConnectionState.Ready:
                        {
                            // 通信準備完了時は 緑色 に
                            button.TintColor = OnlineColor;
                            break;
                        }
                    case ConnectionState.NotExcluding:
                        {
                            // 排他処理前は 橙色に
                            button.TintColor = NotExcludingColor;
                            break;
                        }
                    case ConnectionState.NotConnected:
                        {
                            // 未接続時は 灰色に
                            button.TintColor = OfflineColor;
                            break;
                        }
                } // end of switch
            }

            public void DidSumbit(ConnectionState current)
            {
                if (enable)
                    ChangeColor(current);
            }

            public void OnStateChanged(ConnectionState next, ConnectionState prev)
            {
                if (enable)
                    ChangeColor(next);
            }

            public void Disable()
            {
                enable = false;
            }
        }

        const string SegueIdFiles = "Files";
        const string SegueIdSettings = "Settings";

        EmbossmentUIController UIController { get; set; }
        FieldPreviewController PreviewController { get; set; }
        EmbossmentFieldListController FieldListController { get; set; }
        WifiColorManagerWithConnectionState ConnectionStateListenerForWifiColor { get; set; }
        Action<CommunicationRound> OnCompleteReadyCallback { get; set; }


        /// <summary>
        /// 通信状態を監視します。
        /// </summary>
        //private CommunicationNotifier CommunicationNotifier;

        /// <summary>
        /// アプリのバックグラウンドへの遷移を監視します。
        /// </summary>
        private NSObject applicationDidEnterBackgroundObserver;

        /// <summary>
        /// アプリのフォアグラウンドへの遷移を監視します。
        /// </summary>
        private NSObject applicationDidEnterForegroundObserver;



        FileContext CurrentFile;
        EmbossmentData DataField = EmbossmentData.Empty;
        EmbossmentData Data
        {
            get
            {
                return DataField;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                DataField = value;
            }
        }

        #region Init
        /// <summary>
        /// プログラム側で初期化
        /// </summary>
        public EmbossmentController() : base("EmbossmentController", null)
        {
            InitEmbossmentController();
        }

        /// <summary>
        /// StoryBoardから初期化
        /// </summary>
        /// <param name="handle">Handle.</param>
        public EmbossmentController(IntPtr handle) : base(handle)
        {
            InitEmbossmentController();
        }

        /// <summary>
        /// このクラスの初期化
        /// </summary>
        public void InitEmbossmentController()
        {
            UIController = new EmbossmentUIController();
            PreviewController = new FieldPreviewController();
            FieldListController = new EmbossmentFieldListController();
        }
        #endregion

        /// <summary>
        /// ファイルを受け取る
        /// </summary>
        public void ReceiveFile(FileContext file)
        {
            CurrentFile = file;

            // ファイルの内容に合わせてプレビュー・打刻用データを生成します。
            var fields = file.Owner.Serializable;
            if (fields.Count() > 0)
            {
                
                var first = fields.First();
                ushort mode = first.Mode;
                // ushort mode is flnm in MBDATA defined as a hex value
                // mode : 0 = Text or Logo; 400 = QR code; 800 = DM
                if (mode == 0)
                {
                    Data = EmbossmentData.Create(
                        EmbossmentMode.FromMBData(first),
                        first.Text
                    );
                }
                else
                {
                    Data = EmbossmentData.Create(
                        new EmbossmentMode(),
                        ""
                    );
                }
            }
            else
            {
                Data = EmbossmentData.Create(
                    new EmbossmentMode(),
                    ""
                );
            }
        }

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var barHeight = NavigationController.NavigationBar.Frame.Height + UIApplication.SharedApplication.StatusBarFrame.Height;

            if (UIController != null)
            {
                // NOTE: イベントハンドラによるメモリリークに注意する.
                UIController.EmbossmentModeChanged += (sender, arg) => EmbossmentModeChanged((EmbossmentUIController)sender, ((EmbossmentUIController.ModeChangeEventArgs)arg).ID);
                SetupChildController(UIController, UIContainer);
                UpdateUIController();
            }

            if (PreviewController != null)
            {
                PreviewController.SendRequested += (sender, _) => AcceptSend();
                SetupChildController(PreviewController, PreviewContainer);
                UpdatePreview();
            }

            if (FieldListController != null)
            {
                FieldListController.SendRequested += (sender, _) => AcceptSend();
                SetupChildController(FieldListController, FieldListContainer);
                UpdateFieldList();
            }

            // コントローラへの接続状態をビューに反映します。
            // Removeの実行を忘れずに. (メモリリークします)
            ConnectionStateListenerForWifiColor = new WifiColorManagerWithConnectionState(this, WifiStatusButton);
            AppDelegate.ConnectionStateObserver.AddListener(ConnectionStateListenerForWifiColor);

            // ファイルボタンタッチしたら、ファイル操作選択画面を開きます。
            FilesButton.TouchUpInside += (sender, e) =>
            {
                PerformSegue(SegueIdFiles, this);
            };

            // 設定ボタンをタッチしたら、設定画面を開きます。
            SettingsButton.TouchUpInside += (sender, e) =>
            {
                PerformSegue(SegueIdSettings, this);
            };


            // クリアボタンをタッチしたら、読み込んでいるファイルをクリアします。
            ClearButton.TouchUpInside += (sender, e) =>
            {
                var manager = new FileClearanceManager(CurrentFile, new FileManagementActions((file) =>
                {
                    ReceiveFile(file);
                    file.AutoSave();
                    MyToast.ShowMessage("The file was cleared.", duration: MyToast.Short);
                    UpdateViewsWithFile();
                }));
                manager.Clear();
            };

            // Readyになったらイベントを呼ぶコールバック
            OnCompleteReadyCallback = (state) =>
            {
                if (state == CommunicationRound.Ready)
                    NeedsUpdateEmbossmentAttrib();
            };
            AppDelegate.CommunicationClientController.OnCompleted += OnCompleteReadyCallback;
            // CommunicationControllerから機種番号が変更された時
            //AppDelegate.CommunicationClientController.OnChangeMachineModel += OnChangeMachineModelByCommunicationControllerCallback;
            // 機種番号変更のダイアログ表示の許可
            AppDelegate.CommunicationClientController.AllowToShowDialog();
        }


        /// <summary>
        /// セグエにより画面遷移する直前に呼び出される
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == SegueIdFiles)
            {
                var ctrl = segue.DestinationViewController as FileMenuController;

                CommitFileIfQuickMode();

                ctrl.Loaded = CurrentFile;
                ctrl.Actions = new FileManagementActions((file) =>
                {
                    ReceiveFile(file);
                    file.AutoSave();
                });
            }
            else if (segue.Identifier == SegueIdSettings)
            {

            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // アプリがフォアグラウンドへ遷移する際，コントローラとの通信を停止します。
            applicationDidEnterForegroundObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("OnActivated"), (notify) =>
            {
                //if (CommunicationClient.Instance.GetCurrentState().NotExcluding())
                //{
                //    var app = UIApplication.SharedApplication;
                //    var token = app.BeginBackgroundTask(() =>
                //    {
                //        // システムから時間切れを言い渡された場合の処理
                //        // 
                //        Console.Error.WriteLine("Background task is cancelled and shutdowned by OS! (Reason: timeout)");
                //    });
                //    StartCommunication(() =>
                //    {
                //        app.EndBackgroundTask(token);
                //        token = UIApplication.BackgroundTaskInvalid;
                //        Console.Error.WriteLine("Terminate!!");
                //    });
                //}
                var app = UIApplication.SharedApplication;
                var token = app.BeginBackgroundTask(() =>
                {
                    // システムから時間切れを言い渡された場合の処理
                    // 
                    Console.Error.WriteLine("Background task is cancelled and shutdowned by OS! (Reason: timeout)");
                });
                _ = StartCommunication(() =>
                {
                    app.EndBackgroundTask(token);
                    token = UIApplication.BackgroundTaskInvalid;
                    Console.Error.WriteLine("Terminate!!");
                });
                //CommunicationNotifier.LazyStart();
            });

            // アプリがバックグラウンドへ遷移する際，コントローラとの通信を停止します。
            applicationDidEnterBackgroundObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("OnResignActivation"), (notify) =>
            {
                var app = UIApplication.SharedApplication;
                var token = app.BeginBackgroundTask(() =>
                {
                    // システムから時間切れを言い渡された場合の処理
                    // 
                    Console.Error.WriteLine("Background task is cancelled and shutdowned by OS! (Reason: timeout)");
                });

                Console.Error.WriteLine("Terminate ...");
                _ = TerminateCommunication(() =>
                {
                    app.EndBackgroundTask(token);
                    token = UIApplication.BackgroundTaskInvalid;
                    Console.Error.WriteLine("Terminate!!");
                });
            });

            UpdateViewsWithFile();

        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateViewsWithFile()
        {
            // プレビューを再描画します。
            if (PreviewController != null)
            {
                UpdatePreview();
            }
            // 
            if (UIController != null)
            {
                UpdateUIController();
            }
            // 
            if (FieldListController != null)
            {
                UpdateFieldList();
            }

            UpdateViewEnabled();
        }

        /// <summary>
        /// ViewDidAppear が表示済みである時に true となります。
        /// </summary>
        static bool ViewDidAppearIsAlreadyCalled = false;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!ViewDidAppearIsAlreadyCalled)
            {
                // アプリ起動中、ただ一度だけ呼び出されるロジック
                SettingsController.ShowModelNoSettingStatic(
                    sourceView: View,
                    controller: this,
                    appSetting: new AppSetting(),
                    didHoldOnPreference: (spec) =>
                    {
                        /* ここで画面更新しないとプレビューの内容が修正されない */
                        System.Diagnostics.Debug.WriteLine("Set Machine Model On First Dialog");
                        UpdatePreview();
                    }
                );
            }

            ViewDidAppearIsAlreadyCalled = true;
        }

        /// <summary>
        /// 画面を離脱する際は、ファイルを保存します。
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        override public void ViewWillDisappear(bool animated)
        {
            CommitFileIfQuickMode();
            CurrentFile.AutoSave();

            base.ViewWillDisappear(animated);
        }

        /// <summary>
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        override public void ViewDidDisappear(bool animated)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
            base.ViewDidDisappear(animated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // アプリが閉じられた時に発動するタスクを解除します。
                var notificationCenter = NSNotificationCenter.DefaultCenter;
                notificationCenter.RemoveObserver(applicationDidEnterBackgroundObserver);
                applicationDidEnterBackgroundObserver = null;
                // リスナの開放 忘れるとメモリリークします.
                ConnectionStateListenerForWifiColor.Disable();
                AppDelegate.ConnectionStateObserver.RemoveListener(ConnectionStateListenerForWifiColor);
                // 同じくリスナの開放
                AppDelegate.CommunicationClientController.OnCompleted -= OnCompleteReadyCallback;
            }
            base.Dispose(disposing);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            //BarMargin.SetNeedsUpdateConstraints();
            //BarMargin.UpdateConstraintsIfNeeded();
            //View.SetNeedsLayout();
        }

        #endregion


        #region Event Handler
        public void OnChangeMachineModelByCommunicationController()
        {
            NeedsUpdateEmbossmentAttrib();
        }

        public void NeedsUpdateEmbossmentAttrib()
        {
            Console.WriteLine("Attribute Updating!!");
            Data = Data.ReplaceAttribWithGlobal();
            UpdatePreview();
        }

        public void EmbossmentModeChanged(EmbossmentUIController cnt, string id)
        {
            Console.WriteLine("Mode Changed!! " + id);
            var mode = cnt.EmbossmentMode;
            Data = Data.ReplaceMode(mode);
            if (id == "TextSize")
            {
                UpdatePreview();
            }
        }

        async Task AcceptSend()
        {
            Console.WriteLine("Accept Send on Embossment Controller.");
            Console.WriteLine(Data.Text.Text);
            if (!CommunicationClient.Instance.Ready)
            {
                ShowOfflineError();
                return;
            }

            // ファイルが空の場合は終了
            if (CurrentFile.Owner.IsEmpty)
            {
                ShowEmptyError();
                return;
            }

            await LoadingOverlay.ShowWithTask(async (CancellationToken token) =>
            {
                try
                {
                    if (CurrentFile.isLocalFile)
                    {
                        var kit = EmbossmentToolKit.Instance;
                        var result = await kit.StartMarking(
                            Data,
                            onEmpty    : ShowEmptyError,
                            onTextError: ShowFontError,
                            onOffline  : ShowOfflineError,
                            onMismatchMachineModel: ShowMismatchedMachineModel,
                            onSendFailure: ShowSendFailure,
                            onTextSizeTooLarge: ShowTextSizeTooLarge
                        );
                        if (result.IsSuccess)
                            ShowSendSucceed();
                    }
                    else
                    {
                        var kit = EmbossmentToolKit.Instance;
                        var result = await kit.StartMarking(
                            CurrentFile.Owner.Serializable.ToList(),
                            onEmpty  : ShowEmptyError,
                            onOffline: ShowOfflineError,
                            onMismatchMachineModel: ShowMismatchedMachineModel,
                            onSendFailure: ShowSendFailure
                        );
                        if(result.IsSuccess)
                            ShowSendSucceed();
                    }

                    if (token.IsCancellationRequested) { return; }
                }
                catch (ControllerIO.CommunicationProtectedException ex)
                {
                    Log.Error("通信阻止:", ex.ToString());
                    InvokeOnMainThread(() =>
                    {

                        MyToast.ShowMessage("Protected to send.", duration: 1.8);
                    });
                }
                catch (Exception e)
                {
                    Log.Error("打刻失敗:", e.ToString());
                    if (token.IsCancellationRequested) { return; }
                    ShowSendFailure();
                }

            });
        }

        void ShowSendSucceed()
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("Push the start button on the marking head.", duration: 1.8);
            });
        }

        void ShowEmptyError()
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("Marking file is empty.", duration: 1.8);
            });
        }

        void ShowFontError(TextError res)
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("FontError", fargs: (res.ErrorChar), duration: 5.0);
            });
        }

        void ShowOfflineError()
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("communication.required", duration: 1.8);
            });
        }

        void ShowMismatchedMachineModel()
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("appsetting.controller.different", MyToast.Short);
            });
        }

        void ShowSendFailure()
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("Failed to send marking file.", duration: 1.8);
            });
        }

        void ShowTextSizeTooLarge(PatmarkMachineModel model)
        {
            InvokeOnMainThread(() =>
            {
                MyToast.ShowMessage("validation.on_text_sizes_too_large", duration: 2.4, model.Profile.MaxTextSize());
            });
        }
        #endregion

        #region Util
        void UpdatePreview()
        {
            if (CurrentFile.isRemoteFile)
                UpdateFieldList();
            else
                UpdatePreviewData();
        }

        void UpdatePreviewData()
        {
            PreviewController.PreviewData = new EmbossmentPreviewData(Patmark.Settings.MachineModelNoRepositoryOfKeyValueStore.CurrentSpec, DataField);
        }

        void UpdateFieldList()
        {
            FieldListController.SetFields(CurrentFile.Owner.Readers);
        }

        void UpdateViewEnabled()
        {
            if (PreviewContainer != null)
            {
                var advance = CurrentFile.isRemoteFile;
                var quick = CurrentFile.isLocalFile;
                // Quickモードはこちらを表示
                PreviewContainer.Hidden = advance;
                // Advanceモードはこちらを表示
                FieldListContainer.Hidden = quick;
                // Quickモードのみ、編集可能
                UIController.SetSegmentsEnabled(quick);
            }
        }

        void UpdateUIController()
        {
            // ファイルの内容に合わせてセグメントの選択状態を切り替えます。
            if (UIController != null)
            {
                UIController.SelectSegments(
                    Data.Mode.TextSize,
                    Data.Mode.Force,
                    Data.Mode.Quality
                );
            }
        }

        void ReleaseUserComponent()
        {
            if (UIController != null)
            {
                UIController.EmbossmentModeChanged = null;
                UIController.Dispose();
                UIController = null;
            }
            if (PreviewController != null)
            {
                PreviewController.Dispose();
                PreviewController = null;
            }
            if (FieldListController != null)
            {
                FieldListController.Dispose();
                FieldListController = null;
            }
        }

        // TODO: UIVeiwコンテナへコードを移してみる
        private void SetupChildController(UIViewController subCnt, UIView parent)
        {
            var subView = subCnt.View;
            var barHeight = NavigationController.NavigationBar.Frame.Height;

            AddChildViewController(subCnt);

            var b = parent.Bounds;
            subView.Frame = b;
            subView.AutoresizingMask = All;
            //b.Y = barHeight;
            //b.Height -= barHeight;
            //subView.Frame = b;

            parent.AddSubview(subView);

            subCnt.DidMoveToParentViewController(this);
        }
        #endregion

        //
        partial void WifiStatusButton_TouchUpInside(UIButton sender)
        {
            var client = CommunicationClient.Instance;
            var state = client.GetCurrentState();
            if (state.NotExcluding())
            {
                // ボタンを押して通信を始める
                StartCommunication();
            }
            else if (state.Ready())
            {
                // ボタンを押して通信をやめる
                TerminateCommunication();
            }
        }

        /// <summary>
        /// コントローラとの通信を開始します。
        /// メインスレッド上で実行してください
        /// </summary>
        /// <returns>The communication.</returns>
        void StartCommunication()
        {
            AppDelegate.CommunicationClientController.StartCommunication();
        }

        async Task StartCommunication(Action after)
        {
            try
            {
                await AppDelegate.CommunicationClientController.StartCommunication();
            }
            finally
            {
                try
                {
                    after();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error on EmbossmentController.StartCommunication");
                    Console.Error.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// コントローラとの通信を終了します．
        /// </summary>
        /// <returns>The communication.</returns>
        void TerminateCommunication()
        {
            AppDelegate.CommunicationClientController.TerminateCommunication();
        }

        async Task TerminateCommunication(Action after)
        {
            try
            {
                await AppDelegate.CommunicationClientController.TerminateCommunication();
            }
            finally
            {
                try
                {
                    after();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error on EmbossmentController.TerminateCommunication");
                    Console.Error.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// クイックモードならUIControllerの入力内容をCurrentFileに反映します。
        /// </summary>
        void CommitFileIfQuickMode()
        {
            if (CurrentFile.isLocalFile)
            {
                CurrentFile = CurrentFile.QuickEditor.Apply((src) =>
                {
                    return EmbossmentToolKit.ApplyEmbossmentData(Data, null, new MBData(src)).ToMutable();
                });
            }
        }
    }
}

