using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using ToastIOS;
using Functional.Maybe;
using TokyoChokoku.MarkinBox.SB2;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class RemoteFileMenuController : UITableViewController
	{
        static readonly string EmptyIdentifier = "-";

        public RemoteFileMenuSource Source;

        List<CellSource> CellSourceList = new List<CellSource> ();

        public RemoteFileMenuController (IntPtr handle) : base (handle)
		{
		}

        override
        public void ViewDidLoad () {

            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-file-remote.title", "");

            if (Source == null)
                throw new NullReferenceException ("[RemoteFileMenuController - Source をセットしてください");

            // 再読込ボタンを押下した場合、一覧を再構築します。
            ReloadButton.TouchDown += (sender, e) =>
            {
                InvokeOnMainThread(async () => {
                await ControllerUtils.ActionWithLoadingOverlay(async () =>
                {

					SetupTable(await Source.GetFileListForcibly());
                });
            });
            };

            // 一覧の初期状態を構築します。
            InvokeOnMainThread(async () => {
                await ControllerUtils.ActionWithLoadingOverlay(async () =>
                {
                    SetupTable(await Source.GetFileList());
                });
            });
        }

        void SetupTable (List<RemoteFileManager.RemoteFileInfo> files) {
            var elements = new List<CellSource>();
            for (var i = 0; i<files.Count; i++)
            {
                var fileInfo = files[i];
                if (fileInfo.NumOfField > 0)
                {
                    elements.Add(new CellSource(i, fileInfo.NumOfField, fileInfo.Name));
                }
                else {
                    elements.Add(new CellSource(i, 0, ""));
                }
            }

			InvokeOnMainThread(() =>
            {
                CellSourceList = elements;
                FileTable.ReloadData();
                FileTable.SetNeedsDisplay();
            });
        }

        // ---- イベントセンダー ----
        void WillDismiss (FileMenuDismissedEvent.BecauseOf cause)
        {
            Source.GetAction (new FileMenuDismissedEvent (cause)).Do ((action) => {
                action ();
            });
        }

        // ---- コントローラを閉じる ----
        void DismissAfterLoaded ()
        {
            WillDismiss (FileMenuDismissedEvent.BecauseOf.FileLoaded);
            DismissViewController (true, null);
        }

        void DismissAfterCloseButtunPushed ()
        {
            WillDismiss (FileMenuDismissedEvent.BecauseOf.CloseButtonPushed);
            DismissViewController (true, null);
        }

        // ---- イベントハンドラ ----
        partial void CancelButton_Activated (UIBarButtonItem sender)
        {
            DismissAfterCloseButtunPushed ();
        }

        // ---- File Controll ----
        async Task<bool> Save (int indexOfFile, string fileName)
        {
            bool saved = false;

            await ControllerUtils.ActionWithLoadingOverlay (async () => {
                saved = await Source.Save (indexOfFile, fileName);
                if (saved) {
                    Toast.MakeText (
                        "Saved successfully.".Localize (),
                        ToastDuration.Medium
                    ).Show ();

                    InvokeOnMainThread(async () => {
                        await ControllerUtils.ActionWithLoadingOverlay(async () =>
                        {
        					SetupTable(await Source.GetFileList());
                        });
                    });
                }
            });

            return saved;
        }

        async Task<bool> SaveFileName (int indexOfFile, string fileName)
        {
            var saved = false;
            await ControllerUtils.ActionWithLoadingOverlay (async () => {
                saved = await Source.SaveFileName (indexOfFile, fileName);
                if (saved) {
                    Toast.MakeText (
                        "Saved successfully.".Localize (),
                        ToastDuration.Medium
                    ).Show ();

                    InvokeOnMainThread(async () => {
                        await ControllerUtils.ActionWithLoadingOverlay(async () =>
                        {
							SetupTable(await Source.GetFileList());
                        });
                    });
                }
            });
            return saved;
        }


        // ---- show alert ----
        void ShowRenameAlert(int indexOfFile, string errorMessage = "", string previousText = null)
        {
            var alertBuilder = new RenameAlertBuilder ();

            alertBuilder.TextFieldInit = textField => {
            };

            alertBuilder.Rename = async (action, textField) => {
                var input = textField.Text;
                if (AvailableRemoteFileNameChar.IsValid(input))
                {
                    var newFileName = string.Format("F{0:D3}_{1}", indexOfFile + 1, input);
                    await SaveFileName(indexOfFile, newFileName);
                } else {
                    var emsg = AvailableRemoteFileNameChar.ToStringAvailableChar(
                        "RemoteFileMenuController.ErrorMessageFormat".Localize(),
                        "RemoteFileMenuController.Space".Localize()
                    );
                    ShowRenameAlert(indexOfFile, emsg, input);
                }
            };

            alertBuilder.ErrorMessage = errorMessage;
            alertBuilder.PreviousText = previousText;

            var alert = alertBuilder.BuildAndShow( (it)=>{
                PresentViewController(it, true, null);
            });

        }


        void ShowFileControlSheet (int indexOfFile, string fileName, NSIndexPath row) {

            var alertBuilder = new FileControlSheetBuilder ();

            alertBuilder.Title = (indexOfFile + 1) + ":" + fileName;

            alertBuilder.Read = async action => {
                await ControllerUtils.ActionWithLoadingOverlay (async () => {
                    var success = await Source.Load (indexOfFile, fileName);
                    if (success) {
                        DismissAfterLoaded ();
                    } else {
                        Toast.MakeText (
                            NSBundle.MainBundle.LocalizedString ("File is empty.", ""),
                            ToastDuration.Medium
                        ).Show ();
                    }
                });
            };

            alertBuilder.SaveOver = async action => {
                await Save (indexOfFile, fileName);
            };

            alertBuilder.Rename = action => {
                ShowRenameAlert (indexOfFile);
            };

            var view = TableView.CellAt (row).ContentView;
            var alert = alertBuilder.BuildWithoutClear (sourceView: view);

            // Display the alert
            PresentViewController(alert, true, null);
        }



        // ---- TableViewDelegate ----
        override
        public nint RowsInSection (UITableView tableView, nint section)
        {
            if (section == 0) {
                return CellSourceList.Count;
            }
            return 0;
        }

        override
        public UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            if (CellSourceList.Count == 0) {
                UITableViewCell empty = tableView.DequeueReusableCell (EmptyIdentifier);

                if (empty == null) {
                    empty = new UITableViewCell (UITableViewCellStyle.Default, EmptyIdentifier);
                }

                empty.TextLabel.Text = "-";
            }

            var element = CellSourceList [indexPath.Row];

            UITableViewCell cell = tableView.DequeueReusableCell (element.Identitifer);

            if (cell == null) {
                cell = new UITableViewCell (UITableViewCellStyle.Default, element.Identitifer);
            }

            cell.TextLabel.Text = (element.IndexOfFile + 1) + ":" + element.FileName;
            return cell;
        }

        override
        public void RowSelected (UITableView tableView, NSIndexPath indexPath) {
            var element = GetElement (indexPath);
            TableView.DeselectRow (indexPath, true);
            ShowFileControlSheet ( element.IndexOfFile, element.FileName, indexPath );
        }

        CellSource GetElement (NSIndexPath path)
        {
            return CellSourceList [path.Row];
        }


        sealed class CellSource {
            public int IndexOfFile { get; }
            public int NumOfField { get; }
            public string FileName { get; }

            public string Identitifer {
                get {
                    return IndexOfFile.ToString();
                }
            }

            public CellSource (int indexOfFile, int numOfField, string fileName) {
                IndexOfFile = indexOfFile;
                NumOfField = numOfField;
                FileName = fileName;
            }
        }
	}
}
