using System;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using ObjCRuntime;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using Functional.Maybe;
using System.Text.RegularExpressions;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    partial class PreviewViewController : UIViewController
    {
        // observer
        CommunicationNotifier CommunicationNotifier;

        NSObject applicationDidEnterBackgroundObserver;

        // ---- データ ----
        iOSEditBoxManager EditBoxManager { get; set; }
        CanvasViewController CanvasViewController { get; set; }


        // プロパティ
        CanvasPresentationManager PresentationManager {
            get {
                return CanvasViewController.CanvasPresentationManager;
            }
        }

        iOSFieldManager FieldManager {
            get {
                return CanvasViewController.FieldManager;
            }
        }

        iOSFieldContext FieldContext        {
            get {
                return CanvasViewController.FieldContext;
            }
        }

        Holder<FieldSource> FieldSourceHolder   { get; } = new Holder <FieldSource> (AutoSaveManager.FieldSource);

        FieldSource FieldSource {
            get {
                return FieldSourceHolder.Content;
            }
        }


		public PreviewViewController (IntPtr handle) : base (handle)
		{
		}


        /// <summary>
        /// Maybe モナドから ジェスチャを取り出します．
        /// ジェスチャがない場合は 実装ミスと判断し，例外を発生させます．
        /// </summary>
        /// <returns>The gesture.</returns>
        /// <param name="gesture">Gesture.</param>
        /// <param name="view">View.</param>
        static UIGestureRecognizer ValidGesture (Maybe<UIGestureRecognizer> gesture, UIView view)
        {
            if (gesture.HasValue)
                return gesture.Value;
            throw new ArgumentException ();
        }

        void UpdateFileName ()
        {
            FileNameLabel.Text = FieldSource.From;
        }

        void CanvasViewControllerWillLoad (CanvasViewController canvas)
        {
            CanvasViewController = canvas;
            // ---- 各Manager の初期化を行う ----
            EditBoxManager = new iOSEditBoxManager (PropertyEditorsBox);
            canvas.TapFieldToSelectListener = new Action <TapField> (EditBoxManager.ListenTapFieldEvent).ToMaybe ();
            canvas.PanFieldToMoveListener   = new Action <PanField> (EditBoxManager.ListenPanFieldEvent).ToMaybe ();
        }

		/// <summary>
		/// Views the did load.
		/// </summary>
		override
		public void ViewDidLoad(){

			base.ViewDidLoad ();

            // ---- ボタンとデリゲートの配線 ----
            ButtonZoomIn.TouchUpInside += FieldManager.ActScaleUp;
            ButtonZoomOut.TouchUpInside += FieldManager.ActScaleDown;
            ButtonZoomReset.TouchUpInside += FieldManager.ActResetScale;
            ButtonPosReset.TouchUpInside += FieldManager.ActResetPosition;

            // ---- タブとデリゲートの配線 ----
            TabBar.ItemSelected += OnTabItemSelected;
            FieldContext.TryDeleteAll ();

            // --- ファイルを読み込んでフィールドをセットします ----
            if (FieldSource.TryLoadTo (FieldContext)) {
                Log.Debug("[PreviewViewController] 自動保存ファイルを読み込みました。");
            } else {
                iOSFieldContext.LoadDemo (FieldContext);
                Log.Debug("[PreviewViewController] 自動保存ファイルを読み込めませんでした。");
            }
            UpdateFileName ();

			// ---- プロパティ編集領域を隠します ----
			PropertyEditorsBox.Hidden = true;

            // ---- シリアル設定再読み込み時のコールバックを取得します ----
            SerialSettingsManager.Instance.OnReloaded += () => {
                try
                {
                    var storeCollector = new TextStoreCollector();
                    foreach (var numAndOwnersPair in FieldContext.SearchSerialFields())
                    {
                        var counter = SerialSettingsManager.Instance.Counters[numAndOwnersPair.Key - 1];
                        var owners = numAndOwnersPair.Value;

                        foreach (var owner in owners)
                        {
                            var editor = owner.ToEditor<iOSEditor>();
                            var textStore = editor.Accept(storeCollector, null);
                            if (textStore != null)
                            {
                                textStore.SetIfValid(Regex.Replace(
                                    textStore.Content,
                                    Serial.Consts.FormatRegEx,
                                    string.Format("@S[{0}-{1}]", counter.CurrentValue, numAndOwnersPair.Key)
                                ));
                            }
                            FieldContext.TryReplace(owner, editor.ToOwner<iOSOwner>());
                            PresentationManager.DrawRequest();
                        }
                    }
                } catch (Exception ex)
                {
                    Log.Error($"Unhandled Exception ${ex}");
                }
            };

            // ---- コントローラへの接続状態をビューに反映します ----

            UIColor OfflineColor      = UIColor.LightGray;
            UIColor NotExcludingColor = UIColor.Orange   ;
            UIColor OnlineColor       = UIColor.Green    ;

            // 未接続時は暗く (初期値)
            WifiStatusButton.TintColor = OfflineColor;
            WifiStatusButton.TouchUpInside += async (sender, arg) => {
                try
                {
                    // wifi ボタンが押された時に発動
                    var client = CommunicationClientManager.Instance;
                    var state = client.GetCurrentState();
                    if (state.NotExcluding())
                    {
                        // ボタンを押して通信を始める
                        await StartCommunication();
                    }
                    else if (state.Ready())
                    {
                        // ボタンを押して通信をやめる
                        await TerminateCommunication();
                    }
                } catch (Exception ex)
                {
                    Log.Error($"WifiStatusButton.TouchUpInside | Unhandled Exceptioin: {ex}");
                }
            };

            CommunicationNotifier = new CommunicationNotifier(1000);
            CommunicationNotifier.OnConnectionStatusChanged += (ConnectionState state, ConnectionState preState) => {
                InvokeOnMainThread (async () => {
                    try
                    {
                        switch (state)
                        {
                            case ConnectionState.Ready:
                                {
                                    // 通信準備完了時は 緑色 に
                                    WifiStatusButton.TintColor = OnlineColor;
                                    break;
                                }
                            case ConnectionState.NotExcluding:
                                {
                                    // 排他処理前は 橙色に
                                    WifiStatusButton.TintColor = NotExcludingColor;
                                    // オフライン状態から オンライン状態になる時に限定して
                                    // 排他処理を試みる
                                    if (preState.SwitchedOfflineToOnline(state))
                                    {
                                        await StartCommunication();
                                    }
                                    break;
                                }
                            case ConnectionState.NotConnected:
                                {
                                    // 未接続時は 灰色に
                                    WifiStatusButton.TintColor = OfflineColor;
                                    break;
                                }
                        } // end of switch
                    } catch (Exception ex)
                    {
                        Log.Error($"Unhandled Exception: {ex}");
                        InvokeOnMainThread(() => {
                            ToastIOS.Toast
                                .MakeText(string.Format("communication.abort".Localize(), $"{ex.Message}"))
                                .SetDuration(1800)
                                .Show();
                        });
                    }
                });
            };
            CommunicationNotifier.Start ();
        }

        override
        public void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);

            // ---- プレビューとジェスチャーの配線 ----
            CanvasViewController.SetupGesture ();

            // キーボード表示の監視開始
            PropertyEditorsBox.StartObserveKeyboard ();

            // アプリが閉じたとき，自動保存するように設定．
            // コントローラとのコミュニケーションを止める．
            var notificationCenter = NSNotificationCenter.DefaultCenter;
            applicationDidEnterBackgroundObserver = notificationCenter.AddObserver ("OnResignActivation".ToNSString (), async (notify) => {
                // 自動保存
                Log.Debug ("[PreviewViewController] 編集中のファイルを自動保存します。");
                FieldSource.Autosave (FieldContext);
                //

                // コミュニケーションの中断
                await TerminateCommunication();
            });
        }

        override
        public void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);


            // ビューポートの設定
            CanvasViewController.Viewport = Viewport.Bounds;

            var selector  = Selector.FromHandle (Selector.GetHandle ("OnOrientationChange:"));
            NSNotificationCenter
                .DefaultCenter
                .AddObserver (this, selector, UIDevice.OrientationDidChangeNotification, null);


            // 再表示直後に再描画する
            PresentationManager.DrawRequest ();
        }


        override
        public void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);

            // ---- プレビューとジェスチャーの配線 ----
            CanvasViewController.DestroyGesture ();

            // 自動保存
            Log.Debug ("[PreviewViewController] 編集中のファイルを自動保存します。");
            FieldSource.Autosave (FieldContext);
        }

        override
        public void ViewDidDisappear (bool animated)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver (this);

            // キーボード表示の監視停止
            PropertyEditorsBox.StopObserveKeyboard ();

            base.ViewDidDisappear (animated);
        }


        public override void DidReceiveMemoryWarning ()
        {
            // 念のためファイルの保存を行う
            Log.Debug ("[PreviewViewController] 編集中のファイルを自動保存します。");
            FieldSource.Autosave (FieldContext);
            base.DidReceiveMemoryWarning ();
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing) {
                // 自動保存の停止を行う
                var notificationCenter = NSNotificationCenter.DefaultCenter;
                notificationCenter.RemoveObserver (applicationDidEnterBackgroundObserver);
                applicationDidEnterBackgroundObserver = null;
            }
            base.Dispose (disposing);
        }


        /// <summary>
        /// 端末の方向が変化した時に呼び出されます
        /// NSNotification notification が邪魔なので
        /// 引数なしの AdaptOrientation () に処理を投げています．
        /// </summary>
        /// <returns>The orientation.</returns>
        [Action ("OnOrientationChange:")]
        public void OnOrientationChange (NSNotification notification)
        {
            AdaptOrientation ();
        }

        /// <summary>
        /// プレビュー画面が 生成された直後か，端末の方向が変化した時に呼び出されます
        /// </summary>
        /// <returns>The orientation.</returns>
        void AdaptOrientation ()
        {
            // ビューポートの設定
            // 選択されたフィールドは ここで設定した領域の中に入るように表示されます
            CanvasViewController.Viewport = Viewport.Bounds;

            PresentationManager.DrawRequest ();
        }


		public void OnTabItemSelected (object sender, UITabBarItemEventArgs argument) {

			if (argument.Item.Equals (TabField)) {
				
				PerformSegue ("ShowFieldMenu", this);

			} else if (argument.Item.Equals (TabFile)) {

				PerformSegue ("ShowFileMenu", this);

			} else if (argument.Item.Equals (TabMarking)) {

				PerformSegue ("ShowMarkingMenu", this);

			}  else if (argument.Item.Equals (TabSettings)) {

				PerformSegue ("ShowSettings", this);

			} else {
				Console.WriteLine ("Other");
			}
		}

        /// <summary>
        /// 他のコントローラにフィールドリストを渡す．
        /// 渡す前に編集中の内容をリストに適用します．
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
		override
		public void PrepareForSegue (UIStoryboardSegue segue, NSObject sender) {
            if (segue.Identifier.Equals ("ShowFieldMenu")) {
                var field = (FieldMenuController)segue.DestinationViewController;

                FieldManager.Refreash ();
                field.FieldManager = FieldManager;
                field.EditBoxManager = EditBoxManager;

            } else if (segue.Identifier.Equals ("ShowFileMenu")) {
                var file = (FileMenuController)segue.DestinationViewController;

                FieldManager.Refreash ();
                file.Source = new CommonFileMenuSource (
                    FieldSourceHolder,
                    FieldManager,
                    new ConcreteFieldActionSource (
                        PresentationManager,
                        FieldContext,
                        FieldSourceHolder,
                        FileNameLabel
                    )
                );
            } else if (segue.Identifier.Equals ("ShowMarkingMenu")) {
                var marking = (MarkingMenuController)segue.DestinationViewController;

                FieldManager.Refreash ();
                marking.FieldManager = FieldManager;

            } else if (segue.Identifier.Equals ("ShowSettings")) {
                var nav = (UINavigationController)segue.DestinationViewController;
                var setting = (SettingsViewController)nav.TopViewController;
                FieldManager.Refreash ();
                setting.FieldManager = FieldManager;

            } else if (segue.Identifier.Equals ("CanvasControllerSegue")) {
                // インスタンスの取得を行い，フィールド表示領域に値を渡す．
                CanvasViewControllerWillLoad ((CanvasViewController)segue.DestinationViewController);
            }
		}


		/// <summary>
		/// Unwind segue's jump point.
		/// </summary>
		/// <param name="segue">Segue.</param>
		[Action ("FromFieldMenuToPreview:")]
		public void FromFieldMenuToPreview (UIStoryboardSegue segue) {
			var field = (FieldMenuController)segue.SourceViewController;
			

            // 選択状態解除
            TabBar.SelectedItem = null;
			Console.WriteLine ("From Field Menu");
		}


		/// <summary>
		/// Unwind segue's jump point.
		/// </summary>
		/// <param name="segue">Segue.</param>
		[Action ("FromFileMenuToPreview:")]
		public void FromFileMenuToPreview (UIStoryboardSegue segue) {
			var field = (FileMenuController)segue.SourceViewController;


            // 選択状態解除
            TabBar.SelectedItem = null;
			Console.WriteLine ("From File Menu");
		}

		/// <summary>
		/// Unwind segue's jump point.
		/// </summary>
		/// <param name="segue">Segue.</param>
		[Action ("FromMarkingMenuToPreview:")]
		public void FromMarkingMenuToPreview (UIStoryboardSegue segue) {
			var field = (MarkingMenuController)segue.SourceViewController;


            // 選択状態解除
            TabBar.SelectedItem = null;
			Console.WriteLine ("From Marking Menu");
		}


        /// <summary>
        /// Unwind segue's jump point.
        /// </summary>
        /// <param name="segue">Segue.</param>
        [Action ("FromSettingsMenuToPreview:")]
        public void FromSettingsMenuToPreview (UIStoryboardSegue segue) {
            PresentationManager.DrawRequest ();
            CanvasViewController.SetupGesture ();


            // 選択状態解除
            TabBar.SelectedItem = null;
            Console.WriteLine ("From Settings Menu");
        }

        async Task StartCommunication ()
        {
            await ControllerUtils.ActionWithLoadingOverlay (async () => {
                try
                {
                    var client = CommunicationClientManager.Instance;
                    var succeed = await client.TryExcludingOther();

                    InvokeOnMainThread(() => {
                        try
                        {
                            if (succeed)
                            {
                                ToastIOS.Toast
                                    .MakeText("communication.start".Localize())
                                    .SetDuration(1800)
                                    .Show();
                            }
                            else
                            {
                                ToastIOS.Toast
                                    .MakeText("communication.excluded-me".Localize())
                                    .SetDuration(6000)
                                    .Show();
                            }
                        } catch (Exception ex)
                        {
                            Log.Error($"Unhandled Exceptioin: {ex}");
                        }
                    });

                    if (succeed)
                        // データの読み込み
                        await LoadControllerData();
                } catch(Exception ex) {
                    Log.Error($"通信失敗... {ex}");
                    InvokeOnMainThread(() => {
                        ToastIOS.Toast
                            .MakeText(string.Format("communication.abort".Localize(), ""))
                            .SetDuration(1800)
                            .Show();
                    });
                }
            });
        }

        /// <summary>
        /// 通信を終了します．
        /// </summary>
        /// <returns>The communication.</returns>
        async Task TerminateCommunication ()
        {
            await ControllerUtils.ActionWithLoadingOverlay (async () => {
                try
                {
                    // 排他処理無効
                    var client = CommunicationClientManager.Instance;
                    InvokeOnMainThread(() =>
                    {
                        ToastIOS.Toast
                                .MakeText("communication.terminate".Localize())
                                .SetDuration(1800)
                                .Show();
                    });
                    await client.FreeController();
                } catch (Exception ex)
                {
                    Log.Error($"TerminateCommunication | Unhandled Exception: {ex}");
                    InvokeOnMainThread(() => {
                        ToastIOS.Toast
                            .MakeText(string.Format("communication.abort".Localize(), ""))
                            .SetDuration(1800)
                            .Show();
                    });
                }
            });
        }

        /// <summary>
        /// コントローラからデータを読み込みます．
        /// StartCommunicationから 直接呼び出されます．
        /// </summary>
        /// <returns>The controller data.</returns>
        async Task LoadControllerData ()
        {
            try
            {
                // コントローラから各種設定値を読み出す
                // カレンダー
                await CalendarSettingsManager.Instance.Reload();
                // シリアル
                await SerialSettingsManager.Instance.Reload(null);
            }
            catch (Exception ex)
            {
                Log.Error($"LoadControllerData | Unhandled Exception: {ex}");
                InvokeOnMainThread(() => {
                    ToastIOS.Toast
                        .MakeText(string.Format("communication.abort".Localize(), ""))
                        .SetDuration(1800)
                        .Show();
                });
            }
        }
    }
}
