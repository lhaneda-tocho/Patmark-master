using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.Common;


namespace TokyoChokoku.Patmark.iOS
{
    using Presenter;
    using Presenter.Settings;
    using TokyoChokoku.Patmark.Settings;

    public partial class SettingsController : UITableViewController
    {
        public AppSetting Setting { get; private set; }
        //private bool FinishedLoadModelNo { get; set; } = false;

        protected readonly HashSet<UITableViewCell> SelectableCellSet = new HashSet<UITableViewCell>();

        public SettingsController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // View, Cellの初期化
            SetCellSelectable(AppVersion);
            SetCellSelectable(RomVersion);
            SetCellUnselectable(ModelNoSetting);


            // Modelオブジェクト初期化 & Viewへ設定
            Setting = new AppSetting();
            SetCellSelectable(ModelNoSetting);

            // iOS内の設定読み込み
            ModelNoSetting.MachineModel = Setting.CurrentMachineModelOnPreference();

            NavigationItem.Title = "ctrl-settings.title".Localize();
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.CellAt(indexPath);
            try
            {
                // 選択可能か確認する.
                if (!SelectableCellSet.Contains(cell))
                {
                    tableView.DeselectRow(indexPath, true);
                    return;
                }
                // 
                if (cell == AppVersion)
                {
                    ShowVersion();
                }
                else if (cell == RomVersion)
                {
                    ShowRomVersion();
                }
                else if (cell == ModelNoSetting)
                {
                    ShowModelNoSetting();
                }
            } finally {
                // 選択状態解除
                tableView.DeselectRow(indexPath, true);
            }
        }


        /// <summary>
        /// 指定したセルをセルセットから取り出し，選択不能にする.
        /// </summary>
        /// <param name="cell">Cell.</param>
        void SetCellUnselectable(UITableViewCell cell)
        {
            //var indexPath = TableView.IndexPathForCell(cell);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            SelectableCellSet.Remove(cell);
        }

        /// <summary>
        /// 選択したセルをセルセットに挿入し，選択可能にする.
        /// セルをリサイクルする場合は事前に[SetCellUnselectable]を呼び出してセルを削除すること.
        /// </summary>
        /// <param name="cell">Cell.</param>
        void SetCellSelectable(UITableViewCell cell)
        {
            //var indexPath = TableView.IndexPathForCell(cell);
            cell.SelectionStyle = UITableViewCellSelectionStyle.Default;

            SelectableCellSet.Add(cell);
        }


        void ShowVersion()
        {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
            MyToast.ShowMessageNotLocalized(Setting.ApplicationVersion, MyToast.Long);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
        }


        void ShowRomVersion()
        {
            if (CommunicationClient.Instance.Ready)
            {
                _ = LoadingOverlay.ShowWithTask(async () =>
                {
                    var version = await Setting.ReadRomVersion();
                    await Task.Delay(500);
                    await AppDelegate.RunOnMain(() =>
                    {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                        MyToast.ShowMessageNotLocalized(version, MyToast.Long);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                    });
                });
            }
            else
            {
                MyToast.ShowMessage("communication.required", duration: 1.8);
            }
        }


        /// <summary>
        /// モデル番号の設定のためのアクションシート
        /// </summary>
        void ShowModelNoSetting()
        {
            ShowModelNoSettingStatic(
                sourceView: ModelNoSetting,
                controller: this,
                appSetting: Setting,
                didHoldOnPreference: OnChangeModelNo
            );
        }

        /// <summary>
        /// モデル番号の設定のためのアクションシート
        /// </summary>
        /// <param name="sourceView">アクションシートを表示する大元のビュー (notnulk)</param>
        /// <param name="controller">アクションシートを表示するための親コントローラ (notnull) </param>
        /// <param name="appSetting">アプリ設定 (notnull) </param>
        /// <param name="didHoldOnPreference">環境設定に保存した直後に呼び出されます (nullable)</param>
        internal static void ShowModelNoSettingStatic(
            UIView                      sourceView,
            UIViewController            controller,
            AppSetting                  appSetting,
            Action<PatmarkMachineModel> didHoldOnPreference
        )
        {
            _ = sourceView ?? throw new ArgumentNullException(nameof(sourceView));
            _ = controller ?? throw new ArgumentNullException(nameof(controller));
            _ = appSetting ?? throw new ArgumentNullException(nameof(appSetting));

            void Listen(PatmarkMachineModel spec)
            {
                EmbossmentKit.EmbossmentToolKit.InitGlobalIfNeeded();
                // モデルの変更を受けた場合、それをプリファレンスに保存する
                if (!appSetting.HoldOnPreference(spec))
                {
                    MyToast.ShowMessage("appsetting.controller.fail_to_check", MyToast.Long);
                }
                // 設定ができたことを通知
                didHoldOnPreference?.Invoke(spec);
                // 設定が一致しているか確認
                _ = AppDelegate.CommunicateOnReady(
                    terminator: true,
                    onOffline: () => Task.CompletedTask,
                    block: async () =>
                    {
                        var checkResult = await appSetting.IsControllerModelMatchedWith();
                        await checkResult.Aggregate<CommunicationResult<bool>, Task>(
                            onFailure: (r) =>
                            {
                                Log.Error($"Failure to send machine model({r.Message}): {r.InnerException}");
                                return AppDelegate.RunOnMain(() =>
                                {
                                    MyToast.ShowMessage("appsetting.controller.fail_to_check", MyToast.Long);
                                });
                            },
                            onSuccess: (result) =>
                            {
                                if (!result.Value)
                                {
                                    return AppDelegate.RunOnMain(() =>
                                    {

                                        MyToast.ShowMessage("appsetting.controller.different", MyToast.Short);
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

            var builder = new ModelNoSheetBuilder(PatmarkMachineModel.SpecList);
            builder.Listener += (self, spec) => {
                Listen(spec);
            };
            var alert = builder.Build(sourceView);
            controller.PresentViewController(alert, true, null);
        }

        /// <summary>
        /// モデル番号の設定実行
        /// </summary>
        /// <param name="spec">Spec.</param>
        void OnChangeModelNo(PatmarkMachineModel spec)
        {
            this.ModelNoSetting.MachineModel = spec;
        }




    }
}
