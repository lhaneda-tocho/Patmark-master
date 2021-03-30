
using System;
using System.Threading.Tasks;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.Presenter.Embossment;
using TokyoChokoku.Patmark.Droid.Presenter.FileMenu;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.Droid.Presenter.Embossment
{
    using Custom;
    using EmbossmentKit;
    using FieldPreview;
    using Ruler;


    [Activity(Label = "@string/embossmentActivity_name")]
    public class EmbossmentActivity : Activity
    {
        /// <summary>
        /// このクラスの状態を取得するためのBundle Key
        /// </summary>
        public const string SelfRestoreKey = "EmbossmentActivity";
        /// <summary>
        /// EmbossmentDataを取得するためのBundleKey
        /// </summary>
        public const string EmbossmentDataRestoreKey = "EmbossmentData";

        class MyConnectionStateListener : ConnectionStateListener
        {
            public bool enable = true;
            EmbossmentActivity TheActivity;
            Button TheButton;

            public MyConnectionStateListener(EmbossmentActivity theActivity, Button theButton)
            {
                TheActivity = theActivity ?? throw new NullReferenceException();
                TheButton = theButton ?? throw new NullReferenceException();
            }

            public void Disable()
            {
                enable = false;
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

            public void OnFailOpening(ExclusionError error)
            {
            }

            public void OnFailReleasing(ExclusionError error)
            {
            }

            void ChangeColor(ConnectionState state)
            {
                switch (state)
                {
                    case ConnectionState.Ready:
                        {
                            // 通信準備完了時は 緑色 に
                            TheButton.SetTextColor(WifiButtonColor.Online.ToNativeColor());
                            break;
                        }
                    case ConnectionState.NotExcluding:
                        {
                            // 排他処理前は 橙色に
                            TheButton.SetTextColor(WifiButtonColor.Excluded.ToNativeColor());
                            break;
                        }
                    case ConnectionState.NotConnected:
                        {
                            // 未接続時は 灰色に
                            TheButton.SetTextColor(WifiButtonColor.Offline.ToNativeColor());
                            break;
                        }
                } // end of switch
            }
        }

        public EmbossmentData EmbossmentData { get; set; }
        public EmbossmentPreviewer ThePreviewer { get; private set; }
        public EmbossmentFileRepository FileRepo { get; } = new EmbossmentFileRepository();
        MyConnectionStateListener ConnectionStateListener { get; set; }
        Action<CommunicationRound> OnCompleteReadyCallback { get; set; }

        /// <summary>
        /// ProgressView を制御するクラスです。
        /// </summary>
        ProgressViewControllerAndroid ProgressViewController { get; set; }


        #region Outlet
        //
        public FieldPreView ThePreview { get; private set; }
        public RulerView TheRuler { get; private set; }
        public FrameLayout ThePreviewUI { get; private set; }

        //
        public FrameLayout FieldListUI { get; private set; }
        public FrameLayout FieldListArea { get; private set; }
        public ListView FieldList { get; private set; }
        public ImageButton ThePageSendAtFieldList { get; private set; }
        //
        public ImageButton ThePageForward { get; private set; }
        public ImageButton ThePageBack { get; private set; }
        public ImageButton ThePageSend { get; private set; }
        public Button TheWifiButton { get; private set; }
        public Button TheSettingsButton { get; private set; }
        public Button TheFileButton { get; private set; }
        public Button TheClearButton { get; private set; }
        //
        public TextView TheCurrentPageLabel { get; private set; }
        public TextView TheTotalPageLabel { get; private set; }
        //
        public TextSizeSelecter TheTextSize { get; private set; }
        public ForceSelecter TheForce { get; private set; }
        public QualitySelecter TheQuality { get; private set; }
        //
        public LoadingOverlayView TheOverlayFrame { get; private set; }
        #endregion


        class BlockTouch : Java.Lang.Object, View.IOnTouchListener
        {
            public bool OnTouch(View v, MotionEvent e)
            {
                return true;
            }
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SetContentView(Resource.Layout.Embossment);

            // instance state restore
            if (savedInstanceState != null)
                RestoreStateOnCreate(savedInstanceState);
            else
            {
                ProcessIntentExtra(Intent.Extras);
            }

            RetrieveViews();
            InitViewsOnCreate();

            // サブコントローラの初期化

            Log.Debug("Initialize ProgressViewControllerAndroid");
            ProgressViewController = new ProgressViewControllerAndroid(TheOverlayFrame);
            Log.Debug("Call OnCreate on ProgressViewControllerAndroid");
            ProgressViewController.OnCreate();
            Log.Debug("Success Initialize ProgressViewControllerAndroid");

            // リスナの登録
            ConnectionStateListener = new MyConnectionStateListener(this, TheWifiButton);
            PatmarkApplication.ConnectionStateObserver.AddListener(ConnectionStateListener);

            // 接続完了リスナ
            OnCompleteReadyCallback = (state) =>
            {
                if (state == CommunicationRound.Ready)
                {
                    // EmbossmentAttribの更新
                    NeedsUpdateEmbossmentAttrib();
                }
            };

            PatmarkApplication.CommunicationClientController.OnCompleted += OnCompleteReadyCallback;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            // このクラスのフィールドの保存
            var myState = new Bundle();
            myState.PutString("single", EmbossmentData.Text.Text);
            myState.PutInt("force", (int)EmbossmentData.Mode.Force);
            myState.PutInt("quality", (int)EmbossmentData.Mode.Quality);
            myState.PutInt("textsize", (int)EmbossmentData.Mode.TextSize);
            //myState.PutString(EmbossmentDataRestoreKey, EmbossmentData.ToJson());

            // 出来上がったバンドルを保存
            outState.PutBundle(SelfRestoreKey, myState);
        }

        /// <summary>
        /// Intent.Extraの内容をデシリアライズし，このインスタンスフィールドの初期化を行います
        /// </summary>
        /// <param name="bundle">Bundle.</param>
        void ProcessIntentExtra(Bundle bundle)
        {
            // Intent Extra
        }

        /// <summary>
        /// InstanceStateから状態のリストアを行います.
        /// </summary>
        /// <param name="bundle">Bundle.</param>
        void RestoreStateOnCreate(Bundle bundle)
        {
            // このクラスに対応するバンドルを取得
            var myState = bundle.GetBundle(SelfRestoreKey);
            // フィールドの復元
            var single = myState.GetString("single", "");
            var force    = (ForceLevel)   myState.GetInt("force"   , (int)ForceLevel.Medium);
            var quality  = (QualityLevel) myState.GetInt("quality" , (int)QualityLevel.Medium);
            var textsize = (TextSizeLevel)myState.GetInt("textsize", (int)TextSizeLevel.Medium); 
            EmbossmentData = EmbossmentData.Create(new EmbossmentMode(textsize, force, quality), single);
        }


        /// <summary>
        /// ビューを取得のみを行います．
        /// </summary>
        void RetrieveViews()
        {
            {
                //var g = FindViewById(Resource.Id.includePreviewArea);

                ThePreview = FindViewById<FieldPreView>(Resource.Id.fieldPreview);
                TheRuler = FindViewById<RulerView>(Resource.Id.rulerView);
                ThePreviewUI = FindViewById<FrameLayout>(Resource.Id.embossment_preview_ui);
            }
            {
                FieldListUI = FindViewById<FrameLayout>(Resource.Id.embossment_field_list_ui);
                FieldListArea = FindViewById<FrameLayout>(Resource.Id.field_list_area_frame);
                FieldList = FindViewById<ListView>(Resource.Id.field_list);
                ThePageSendAtFieldList = FindViewById<ImageButton>(Resource.Id.button_send_at_field_list);
            }
            {
                //var g = FindViewById(Resource.Id.includeEmbossmentPreviewUI);
                ThePageForward = FindViewById<ImageButton>(Resource.Id.button_pageForward);
                ThePageBack = FindViewById<ImageButton>(Resource.Id.button_pageBack);
                ThePageSend = FindViewById<ImageButton>(Resource.Id.button_send);

                TheCurrentPageLabel = FindViewById<TextView>(Resource.Id.label_currentPage);
                TheTotalPageLabel   = FindViewById<TextView>(Resource.Id.label_totalPage);
            }
            {
                var g = FindViewById(Resource.Id.includeTextSizeUI);
                var segment = g as RadioGroup; //g.FindViewById<RadioGroup>(Resource.Id.segment);
                TheTextSize = new TextSizeSelecter(segment);
            }
            {
                var g = FindViewById(Resource.Id.includeForceUI);
                var segment = g as RadioGroup; //g.FindViewById<RadioGroup>(Resource.Id.segment);
                TheForce = new ForceSelecter(segment);
            }
            {
                var g = FindViewById(Resource.Id.includeQualityUI);
                var segment = g as RadioGroup; //g.FindViewById<RadioGroup>(Resource.Id.segment);
                TheQuality = new QualitySelecter(segment);
            }
            {
                // Overlay Frame
                TheOverlayFrame = (LoadingOverlayView) FindViewById(Resource.Id.progressBarView);
			}
            {
                TheWifiButton = FindViewById<Button>(Resource.Id.wifi_button);
            }
            {
                TheSettingsButton = FindViewById<Button>(Resource.Id.settings_button);
            }
            {
                TheFileButton = FindViewById<Button>(Resource.Id.files_button);
            }
            {
                TheClearButton = FindViewById<Button>(Resource.Id.clear_button);
            }
        }

        void InitViewsOnCreate() {
            // injection
            TheRuler.InjectContentView(ThePreview.RulerContent);

            ThePreviewer = new EmbossmentPreviewer(
                ThePreview, 
                ThePageForward,
                ThePageBack, 
                TheCurrentPageLabel,
                TheTotalPageLabel
            );

            ThePreview.OnSizeUpdateRequired += OnAcceptResizePreview;

            // Send Callback init
            ThePageSend.Click += (sender, ev) => Task.Run(async ()=>{
                try
                {
                    await AcceptSend(sender, ev);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            // Send Callback init
            ThePageSendAtFieldList.Click += (sender, ev) => Task.Run(async ()=>{
                try { await AcceptSend(sender, ev); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            });

            // Segment Callback
            TheTextSize.Changed += SelectTextSize;
            TheForce   .Changed += SelectForce;
            TheQuality .Changed += SelectQuality;


            TheOverlayFrame.SetOnTouchListener(new BlockTouch());
            TheOverlayFrame.Visibility = ViewStates.Gone;

            // Wifi Button
            TheWifiButton.Click += (IntentSender, ev) =>
            {
                WifiStatusButton_TouchUpInside();
            };
            TheWifiButton.SetTextColor(WifiButtonColor.Offline.ToNativeColor());

            //UIColor OfflineColor = UIColor.LightGray;
            //UIColor NotExcludingColor = UIColor.Orange;
            //UIColor OnlineColor = UIColor.Green;

            TheSettingsButton.Click += (IntentSender, ev) =>
            {
                StartActivity(new Intent(this, typeof(SettingsActivity)).Also((intent) => {
                    //intent.PutExtras(bundle);
                }));
            };

            TheFileButton.Click += (semder, ev) =>
            {
                StartActivity(new Intent(this, typeof(FileCategorySelectionActivity)));
            };

            TheClearButton.Click += (semder, ev) =>
            {
                var f = FileContext.Empty();
                f.AutoSave();
                FileRepo.RestoreFile(f);

                OnRestoreEmbossmentData();
                EmbossmentData = FileRepo.CreateEmbossmentData();
                InitViewsOnResume();
                TheRuler.Invalidate();
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_current_file_cleared), ToastLength.Long).Show();
            };
        }

        void InitViewsOnResume() {
            var mode = EmbossmentData.Mode;

            // Init value
            TheTextSize.Level = mode.TextSize;
            TheForce.Level = mode.Force;
            TheQuality.Level = mode.Quality;

            // モード毎の表示切り替え
            if(FileRepo.CurrentFile.isLocalFile){
                TheRuler.Visibility = ViewStates.Visible;
                ThePreview.Visibility = ViewStates.Visible;
                ThePreviewUI.Visibility = ViewStates.Visible;
                TheTextSize.SetEnabled(true);
                TheForce.SetEnabled(true);
                TheQuality.SetEnabled(true);

                FieldListUI.Visibility = ViewStates.Invisible;
                FieldListArea.Visibility = ViewStates.Invisible;
            }else{
                var fields = from r in FileRepo.CurrentFile.Owner.Readers
                             select String.Format("{0}{1}",
                                 Resources.GetString(r.FieldType.LabelId()),
                                 String.IsNullOrEmpty(r.Text) ? "" : String.Format(" : {0}", r.Text)
                             ) ;
                FieldList.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, fields.ToArray());

                TheRuler.Visibility = ViewStates.Invisible;
                ThePreview.Visibility = ViewStates.Invisible;
                ThePreviewUI.Visibility = ViewStates.Invisible;
                TheTextSize.SetEnabled(false);
                TheForce.SetEnabled(false);
                TheQuality.SetEnabled(false);

                FieldListUI.Visibility = ViewStates.Visible;
                FieldListArea.Visibility = ViewStates.Visible;
            }



            TheTextSize.Invalidate();
            TheForce.Invalidate();
            TheQuality.Invalidate();

            // Model Object Injection
            UpdatePreview();

        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        static bool isFirstResume = true;

        protected override void OnResume()
        {
            base.OnResume();


            OnRestoreEmbossmentData();
            EmbossmentData = FileRepo.CreateEmbossmentData();
            InitViewsOnResume();

            // ビューサイズの修正
            OnAcceptResizePreview(ThePreview.Area);

            TheRuler.Invalidate();

            // CommunicationLifeCycleCallback の取りこぼし対策
            NeedsUpdateEmbossmentAttrib();

            // ProgressViewControllerAndroid のライフサイクルコールバック
            ProgressViewController.OnResume();

            // 初回起動時にダイアログを出す
            if(isFirstResume)
            {
                // アプリ起動中、ただ一度だけ呼び出されるロジック
                SettingsActivity.ShowModelNoSettingStatic(
                    this,
                    new AppSetting(this),
                    didHoldOnPreference: (spec) =>
                    {
                        /* ここで画面更新しないとプレビューの内容が修正されない */
                        System.Diagnostics.Debug.WriteLine("Set Machine Model On First Dialog");
                        UpdatePreview();
                    }
                );
            }

            isFirstResume = false;

        }


        protected override void OnPause()
        {
            // ProgressViewControllerAndroid のライフサイクルコールバック
            ProgressViewController.OnPause();

            OnAutoSave();
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            // ProgressViewControllerAndroid のライフサイクルコールバック
            ProgressViewController.OnDestroy();

            // 解放処理
            ConnectionStateListener.Disable();
            PatmarkApplication.ConnectionStateObserver.RemoveListener(ConnectionStateListener);
            PatmarkApplication.CommunicationClientController.OnCompleted -= OnCompleteReadyCallback;
            base.OnDestroy();
        }

        #region Callbacks
        

        public void NeedsUpdateEmbossmentAttrib()
        {
            Console.WriteLine("Attribute Updating!!");
            EmbossmentData = EmbossmentData.ReplaceAttribWithGlobal();
            UpdatePreview();
        }

        void OnAcceptResizePreview(EmbossArea area)
        {
            double areaAspectW = area.Widthmm / area.Heightmm;
            ThePreview.AspectRatioAttrib = AspectRatio.Exact((float)areaAspectW); // プレビューの自動調整を無効化する
            TheRuler.RequestLayout();
            ThePreview.ForceLayout();
        }


        void UpdatePreview()
		{
			ThePreviewer.TheEmbossmentData = EmbossmentData;
            ThePreview.Invalidate();
            TheRuler.Invalidate();

            ThePreviewer.Update();
        }

        void WifiStatusButton_TouchUpInside()
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
            PatmarkApplication.CommunicationClientController.StartCommunication();
        }

        /// <summary>
        /// コントローラとの通信を終了します．
        /// </summary>
        /// <returns>The communication.</returns>
        void TerminateCommunication()
        {
            PatmarkApplication.CommunicationClientController.TerminateCommunication();
        }


		//
        async Task AcceptSend(object sender, EventArgs ev)
        { 
            async Task OnOffline()
            {
                ShowOfflineError();
            }
            async Task Send()
            {
                try
                {
                    if (FileRepo.CurrentFile.isLocalFile)
                    {
                        var data = EmbossmentData;
                        var result = await EmbossmentToolKit.Instance.StartMarking(
                            data,
                            onEmpty: ShowEmptyError,
                            onTextError: ShowFontError,
                            onOffline: ShowOfflineError,
                            onMismatchMachineModel: ShowMismatchedMachineModel,
                            onSendFailure: ShowSendFailure,
                            onTextSizeTooLarge: ShowTextSizeTooLarge
                        );
                        if (result.IsSuccess)
                            ShowSendSucceed();
                    }
                    else
                    {
                        var result = await EmbossmentToolKit.Instance.StartMarking(
                            FileRepo.CurrentFile.Owner.Serializable.ToList(),
                            onEmpty: ShowEmptyError,
                            onOffline: ShowOfflineError,
                            onMismatchMachineModel: ShowMismatchedMachineModel,
                            onSendFailure: ShowSendFailure
                        );
                        if (result.IsSuccess)
                            ShowSendSucceed();
                    }
                }
                catch (ControllerIO.CommunicationProtectedException ex)
                {
                    Log.Error("通信阻止: ", ex.ToString());
                    RunOnUiThread(() =>
                    {
                        Toast
                            .MakeText(this,
                                      Resource.String.toast_send_protected,
                                      ToastLength.Long)
                            .Show();
                    });
                }
                catch (Exception e)
                {
                    Log.Error("打刻失敗:", e.ToString());
                    ShowSendFailure();
                }
            }

            // ====

			Console.WriteLine("RequestSend");

            if(FileRepo.CurrentFile.Owner.IsEmpty)
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.toast_marking_file_is_empty), ToastLength.Long).Show();
                }, null);
            }
            else if (CommunicationClient.Instance.Ready)
            {
                //await TheOverlayFrame.ShowWhileProcessing(async () =>
                //{
                //    await Send();
                //});

                await PatmarkApplication.CommunicateOnReady(
                    terminator: true,
                    onOffline: OnOffline,
                    block: Send
                );
            }
			else
			{
                await OnOffline();
            }
        }

        void ShowSendSucceed()
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_ready_to_mark), ToastLength.Long).Show();
            });
        }

        void ShowEmptyError()
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_marking_file_is_empty), ToastLength.Long).Show();
            });
        }

        void ShowFontError(TextError res)
        {
            RunOnUiThread(() =>
            {
                var mes = String.Format(
                    Resources.GetString(Resource.String.toast_font_error),
                    res.ErrorChar
                );
                Toast.MakeText(this, mes, ToastLength.Long).Show();
            });
        }

        void ShowOfflineError()
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_communication_required), ToastLength.Long).Show();
            });
        }

        void ShowMismatchedMachineModel()
        {
            RunOnUiThread(() =>
            {
                int id = Resource.String.toast_appsetting_controller_different;
                Toast.MakeText(Application.Context, id, ToastLength.Long).Show();
            });
        }

        void ShowSendFailure()
        {
            RunOnUiThread(() =>
            {
                Toast
                    .MakeText(this,
                              Resource.String.toast_failed_to_send_marking_file,
                              ToastLength.Long)
                    .Show();
            });
        }

        void ShowTextSizeTooLarge(PatmarkMachineModel model)
        {
            RunOnUiThread(() =>
            {
                var message = string.Format(
                    GetString(Resource.String.toast_validation_on_text_sizes_too_large),
                    model.Profile.MaxTextSize()
                );

                Toast
                    .MakeText(this, message, ToastLength.Long)
                    .Show();
            });
        }
        //

        //
        void SelectTextSize(TextSizeSelecter sender)
        {
			Console.WriteLine("TextSize Changed: " + sender.Level.GetName());
            EmbossmentData = EmbossmentData.ReplaceMode( new EmbossmentMode(
                sender.Level,
                EmbossmentData.Mode.Force,
                EmbossmentData.Mode.Quality
            ));
			UpdatePreview();
        }

        void SelectForce(ForceSelecter sender)
        {
			Console.WriteLine("Force Changed: " + sender.Level.GetName());
			EmbossmentData = EmbossmentData.ReplaceMode(new EmbossmentMode(
				EmbossmentData.Mode.TextSize,
                sender.Level,
				EmbossmentData.Mode.Quality
			));
        }

        void SelectQuality(QualitySelecter sender)
        {
			Console.WriteLine("Quality Changed: " + sender.Level.GetName());
			EmbossmentData = EmbossmentData.ReplaceMode(new EmbossmentMode(
				EmbossmentData.Mode.TextSize,
                EmbossmentData.Mode.Force,
                sender.Level
			));
        }

        void OnRestoreEmbossmentData()
        {
            // load auto save file
            FileRepo.RestoreFile(FileContext.Empty());
            // このコントローラに入った時点で何も読み込めていない場合は空文字のデータを保存する.
            FileRepo.SaveQuickModeAsBlankIfNeeded();
        }

        void OnAutoSave() {
            FileRepo.CommitIfQuickMode(EmbossmentData);
            FileRepo.AutoSave();
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnMenuItemSelected(featureId, item);
            }
        }
		//
		#endregion
    }
}
