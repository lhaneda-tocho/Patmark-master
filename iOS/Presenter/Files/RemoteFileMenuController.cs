using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;

using TokyoChokoku.Patmark.AvailableCharacters;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;

namespace TokyoChokoku.Patmark.iOS
{
    using Presenter;

	partial class RemoteFileMenuController : UITableViewController
	{
        static readonly string EmptyIdentifier = "-";

        public RemoteFileManager Source;
        List<CellSource> CellSourceList = new List<CellSource> ();

        public RemoteFileMenuController (IntPtr handle) : base (handle)
		{
		}


        public override void ViewDidLoad () {
            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-file-remote.title", "");

            if (Source == null)
                throw new NullReferenceException ("[RemoteFileMenuController - Source をセットしてください");

            // 再読込ボタンを押下した場合、一覧を再構築します。
            ReloadButton.TouchDown += (sender, e) =>
            {
                InvokeOnMainThread(async () => {
                    await LoadingOverlay.ShowWithTask(async () =>
                    {
                        try{
                            SetupTable(await Source.GetFileListForcibly());
                        }
                        catch (ControllerIO.CommunicationProtectedException ex)
                        {
                            InvokeOnMainThread(() =>
                            {
                                MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                            });
                        }
                        catch (Exception)
                        {
                            InvokeOnMainThread(() =>
                            {
                                MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                            });
                        }
                    });
                });
            };

            // 一覧の初期状態を構築します。
            InvokeOnMainThread(async () => {
                await LoadingOverlay.ShowWithTask(async () =>
                {
                    try
                    {
                        SetupTable(await Source.GetFileList());
                    }
                    catch (ControllerIO.CommunicationProtectedException ex)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                        });
                    }
                    catch (Exception)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                        });
                    }
                });
            });
        }

        void SetupTable (List<RemoteFileInfo> files) {
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
            CellSourceList = elements;

			InvokeOnMainThread(() =>
            {
                FileTable.ReloadData();
                FileTable.SetNeedsDisplay();
            });
        }

        //// ---- イベントセンダー ----
        //void WillDismiss (FileMenuDismissedEvent.BecauseOf cause)
        //{
        //    Source.GetAction (new FileMenuDismissedEvent (cause)).Do ((action) => {
        //        action ();
        //    });
        //}

        // ---- コントローラを閉じる ----
        void DismissAfterLoaded ()
        {
            //WillDismiss (FileMenuDismissedEvent.BecauseOf.FileLoaded);
            //DismissViewController (true, null);
            NavigationController.PopViewController(true);
        }

        void DismissAfterCloseButtunPushed ()
        {
            //WillDismiss (FileMenuDismissedEvent.BecauseOf.CloseButtonPushed);
            //DismissViewController (true, null);
            NavigationController.PopViewController(true);
        }

        // ---- イベントハンドラ ----
        partial void CancelButton_Activated (UIBarButtonItem sender)
        {
            DismissAfterCloseButtunPushed ();
        }

        // ---- File Controll ----
        async Task<bool> Clear(int indexOfFile, string fileName)
        {
            bool saved = false;

            await LoadingOverlay.ShowWithTask(async () => {
                try
                {
                    saved = await Source.Clear(indexOfFile);
                    if (saved)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("Cleared successfully.", duration: MyToast.Short);
                        });

                        SetupTable(await Source.GetFileList());
                    }
                }
                catch (ControllerIO.CommunicationProtectedException ex)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                    });
                }
                catch (Exception)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                    });
                }
            });

            return saved;
        }

        async Task<bool> Save (int indexOfFile, string fileName)
        {
            bool saved = false;

            if (Source.Loaded.IsEmpty)
            {
                InvokeOnMainThread(() =>
                {
                    ShowEmptyFileMessage();
                });
                return false;
            }
            await LoadingOverlay.ShowWithTask(async () => {
                try
                {
                    saved = (await Source.Save(indexOfFile)) && (await Source.SaveFileName(indexOfFile, fileName));
                    if (saved)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("Saved successfully.", MyToast.Short);
                        });

                        SetupTable(await Source.GetFileList());
                    }
                }
                catch (ControllerIO.CommunicationProtectedException ex)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                    });
                }
                catch (Exception)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                    });
                }
            });

            return saved;
        }

        async Task<bool> SaveFileName (int indexOfFile, string fileName)
        {
            var saved = false;
            await LoadingOverlay.ShowWithTask(async () => {
                try
                {
                    saved = await Source.SaveFileName(indexOfFile, fileName);
                    if (saved)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("Edited successfully.", duration: MyToast.Short);
                        });
                        SetupTable(await Source.GetFileList());
                    }
                }
                catch (ControllerIO.CommunicationProtectedException ex)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                    });
                }
                catch (Exception)
                {
                    InvokeOnMainThread(() =>
                    {
                        MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                    });
                }
            });
            return saved;
        }


        // ---- show alert ----
        void ShowRenameAlert(int indexOfFile, string errorMessage = "", string previousText = null)
        {
            var alertBuilder = new RenameAlertBuilder ();
            alertBuilder.ErrorMessage = errorMessage;
            alertBuilder.PreviousText = previousText;

            alertBuilder.TextFieldInit = textField => {
                textField.Placeholder = "File name".Localize();
            };

            alertBuilder.Rename = async (action, textField) => {
                string input = textField.Text;
                if(input != null)
                {
                    input = input.Replace("\0", "");
                }
                if (AvailableRemoteFileNameChar.IsValid(input))
                {
                    var newFileName = string.Format("F{0:D3}_{1}", indexOfFile + 1, input);
                    await SaveFileName(indexOfFile, newFileName);
                } else {
                    var errMsg = AvailableRemoteFileNameChar.ToStringAvailableChar(
                        "RemoteFileMenuController.ErrorMessage".Localize(),
                        "RemoteFileMenuController.Space".Localize()
                    );
                    ShowRenameAlert(indexOfFile, errMsg, input);
                }
            };

            //var alert = alertBuilder.Build ();
            alertBuilder.BuildAndShow((it) => {
                PresentViewController(it, true, null);
            });
        }


        void ShowFileControlSheet (int indexOfFile, string fileName, NSIndexPath row) {

            var alertBuilder = new FileControlSheetBuilder ();

            alertBuilder.Title = (indexOfFile + 1) + ":" + fileName;

            alertBuilder.Read = async action => {
                await LoadingOverlay.ShowWithTask(async () => {
                    try
                    {
                        var success = await Source.Load(indexOfFile, fileName);
                        if (success)
                        {
                            InvokeOnMainThread(() =>
                            {
                                MyToast.ShowMessage("Read successfully.", duration: MyToast.Short);
                                DismissAfterLoaded();
                            });
                        }
                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                MyToast.ShowMessage("File is empty.", duration: MyToast.Short);
                            });
                        }
                    }
                    catch (ControllerIO.CommunicationProtectedException ex)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                        });
                    }
                    catch (Exception)
                    {
                        InvokeOnMainThread(() =>
                        {
                            MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                        });
                    }
                });
            };

            alertBuilder.SaveOver = async action => {
                await Save (indexOfFile, fileName);
            };

            alertBuilder.Rename = action => {
                ShowRenameAlert (indexOfFile);
            };

            alertBuilder.Clear = async action =>
            {
                await Clear(indexOfFile, fileName);
            };

            var view = TableView.CellAt (row).ContentView;
            var alert = alertBuilder.Build (sourceView: view);

            // Display the alert
            PresentViewController(alert, true, null);
        }


        void ShowEmptyFileMessage()
        {
            MyToast.ShowMessage("File is empty.", MyToast.Short);
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
