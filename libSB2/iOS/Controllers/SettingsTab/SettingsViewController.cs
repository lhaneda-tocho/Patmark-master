using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;
using Foundation;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class SettingsViewController : UITableViewController
	{
        public iOSFieldManager FieldManager { get; set; } = null;

//		private readonly MBMemories.MachineModel[] MachineModels = new MBMemories.MachineModel[]{
//			MBMemories.MachineModel.No3315,
//			MBMemories.MachineModel.No8020,
//			MBMemories.MachineModel.No1010
//		};
//
//		private readonly OperationMode[] OperationModes = new OperationMode[]{
//			OperationMode.Administrator,
//			OperationMode.Operator
//		};

		public SettingsViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            // ナビゲーションのタイトル、戻るボタンをセットアップします。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-preferences.title", "");
            ControllerUtils.SetupNavigationBackButton(NavigationItem);

            // 機種セレクタを初期化します。
            {
				var  values = new MBMemories.MachineModel[]{
					MBMemories.MachineModel.No3315,
					MBMemories.MachineModel.No8020,
					MBMemories.MachineModel.No1010,
                    MBMemories.MachineModel.No2015
				};
				var selector = MachineModelSelector;
				//  
				selector.SelectedSegment = Array.IndexOf (values, MachineModelNoManager.Get ());
				selector.ValueChanged += async (object sender, EventArgs e) => {
                    await ControllerUtils.ActionWithLoadingOverlay (async () => {
                        FieldManager.ForceDeleteAll ();
                        MachineModelNoManager.Set (values [(int)selector.SelectedSegment]);
                        await MachineModelNoManager.TrySendToController ();
                        await UpdateSolenoidTypeSelector ();
                    });
				};
			}

            // ソレノイドの種類
            InitSolenoidTypeSelector ();

			// 操作モードを初期化します。
			{
				var values = new OperationMode[]{
					OperationMode.Administrator,
					OperationMode.Operator
				};
				var selector = OperationModeSelector;

                // タイトルを設定します。
                for (var i = 0; i < selector.NumberOfSegments; i++)
                {
                    selector.SetTitle(
                        NSBundle.MainBundle.LocalizedString(
                            string.Format("ctrl-preferences-models-segment-{0}.title", i + 1),
                            ""
                        ),
                        i
                    );
                }

				selector.SelectedSegment = Array.IndexOf (values, OperationModeManager.Get());
				// 
				selector.ValueChanged += (object sender, EventArgs e) => {
					OperationModeManager.Set (values [(int)selector.SelectedSegment]);
				};
			}
            // 1 Way or 2 Way モードを初期化します
            {
                var values = new BarcodeMarkingMode [] {
                    BarcodeMarkingMode.Marking1Way,
                    BarcodeMarkingMode.Marking2Way
                };

                var selector = BarcodeNWaySelector;

                selector.SelectedSegment = Array.IndexOf (values, BarcodeMarkingModeManager.Mode);

                selector.ValueChanged += async (sender, e) => {
                    await ControllerUtils.ActionWithLoadingOverlay (async () => {
                        var value = values [(short)selector.SelectedSegment];
                        BarcodeMarkingModeManager.Mode = value;
                        await BarcodeMarkingModeManager.TrySendToController ();
                    });
                };
            }

            // cancel button
            CancelButton.Clicked += (object sender, EventArgs e) => {
                Finish ();
            };
       
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath){

			var cell = GetCell(tableView, indexPath);

			if (cell.IsEqual (WifiSettingsCell)) {
				// 設定画面を開きます。
				UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
			} else if(cell.IsEqual(FinishAppCell)){

                InvokeOnMainThread(async () =>
                    {
                        await ControllerUtils.ActionWithLoadingOverlay(async () =>
                        {
                            // 排他処理無効
                            await CommunicationClientManager.Instance.FreeController();
                        });
                        // アプリを終了します。
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); 
                    });
			}
		}

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (section == 0)
            {
                return NSBundle.MainBundle.LocalizedString("ctrl-preferences-table-section-1.title", "");
            }
            else
            {
                return NSBundle.MainBundle.LocalizedString("ctrl-preferences-table-section-2.title", "");
            }
        }

        private void Finish(){
            PerformSegue ("exit", this);
        }


        /// <summary>
        /// 項目「ソレノイドの種類」の表示・初期化
        /// </summary>
        void InitSolenoidTypeSelector ()
        {
            var selector = SolenoidTypeSelector;
            for (int i = 0; i < selector.NumberOfSegments; i++) {
                var title = ("ctrl-preferences-solenoid-type-" + i).Localize ();
                selector.SetTitle (title, i);
            }
            selector.SelectedSegment = (int) SolenoidTypeManager.SolenoidType;
            selector.ValueChanged += async (sender, e) => {
                await ControllerUtils.ActionWithLoadingOverlay (async () => {
                    var type = SolenoidTypeFromSegment (selector.SelectedSegment);
                    await SolenoidTypeChanged (type);
                });
            };
            UpdateSolenoidTypeSelector ().Wait ();
        }

        /// <summary>
        /// セレクタのセグメント番号を Solenoid Type に変換します．
        /// </summary>
        /// <returns>The type from segment.</returns>
        /// <param name="segment">Segment.</param>
        SolenoidType SolenoidTypeFromSegment (nint segment)
        {
            return segment == 0 ? SolenoidType.Standard : SolenoidType.BigSolenoid;
        }

        /// <summary>
        /// Solenoid Type を セレクタのセグメント番号に変換します．
        /// </summary>
        /// <returns>The type segment of.</returns>
        /// <param name="type">Type.</param>
        nint SolenoidTypeSegmentOf (SolenoidType type)
        {
            return type == SolenoidType.Standard ? 0 : 1;
        }

        /// <summary>
        /// セレクタの表示を更新します．
        /// </summary>
        async Task UpdateSolenoidTypeSelector ()
        {
            var model    = MachineModelNoManager.Get ();
            var selector = SolenoidTypeSelector;
            var type     = SolenoidTypeFromSegment (selector.SelectedSegment);

            if (model == MBMemories.MachineModel.No3315 && type != SolenoidType.Standard) {
                // 3315モデルであり，かつ, Big に設定されている時は Standardに設定するイベントを発火
                type = SolenoidType.Standard;
                selector.SelectedSegment = SolenoidTypeSegmentOf (type);
                await SolenoidTypeChanged (type);
            }

            if (MachineModelNoManager.Get () == MBMemories.MachineModel.No3315) {
                // 3315モデルのとき，big を無効にする．
                selector.SetEnabled (false, SolenoidTypeSegmentOf (SolenoidType.BigSolenoid));
            } else {
                selector.SetEnabled (true , SolenoidTypeSegmentOf (SolenoidType.BigSolenoid));
            }
        }

        /// <summary>
        /// ソレノイドの種類が変更された時に呼び出されます．
        /// </summary>
        /// <param name="type">Type.</param>
        async Task SolenoidTypeChanged (SolenoidType type)
        {
            SolenoidTypeManager.SolenoidType = type;
            await SolenoidTypeManager.TrySendToController ();
        }
	}
}
