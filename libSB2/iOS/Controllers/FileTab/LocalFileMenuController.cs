using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;
using Functional.Maybe;

using ToastIOS;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    

    public partial class LocalFileMenuController : UITableViewController
    {
        /// <summary>
        /// このコントローラに指定するモデルオブジェクトです，
        /// </summary>
        /// <value>The source.</value>
        public LocalFileMenuSource Source { get; set; }

        List<CellSource> CellSourceList { get; set; }

        // ---- イニシャライザー ----

        public LocalFileMenuController (IntPtr handle) : base (handle)
        {
        }


        override
        public void ViewDidLoad ()
        {

            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString ("ctrl-file-local.title", "");

            if (Source == null)
                throw new NullReferenceException ();

            CellSourceList = ToCellSourceList (Source.FileList);

            Source.Setup ();
        }

        // ---- イベントセンダー ----
        void WillDismiss (FileMenuDismissedEvent.BecauseOf cause)
        {
            Source.GetAction (new FileMenuDismissedEvent (cause)).Do ( (action) => {
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

        // ---- イベントリスナー ----

        /// <summary>
        /// ツールバーの追加ボタン
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void AddButton_Activated (UIBarButtonItem sender)
        {
            ShowSaveAlert ();
        }

        /// <summary>
        /// ツールバーのキャンセルボタン
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void CancelButton_Activated (UIBarButtonItem sender)
        {
            DismissAfterCloseButtunPushed ();
        }


        // ---- TableView Controll ----
        void Sync ()
        {
            CellSourceList = ToCellSourceList (Source.FileList);
        }

        void Reload ()
        {
            FileTable.ReloadData ();
        }

        void ReloadInsert (PathName name)
        {
            var maybeIndex = CellSourceList.FindIndex ( (e) => name.Equals (e.PathName));
            if (maybeIndex < 0) {
                Reload ();
                return;
            }
            var index = (nuint) maybeIndex;
            FileTable.InsertRows (new NSIndexPath [] {
                NSIndexPath.Create (new nuint[] {0, index })}, UITableViewRowAnimation.Automatic);
        }

        // ---- 操作そのものを表すメソッド ----
        void Rename (PathName target, PathName next)
        {
            Source.Rename (target, next);
            Sync ();
            Reload ();
        }

        void SaveAs (PathName path)
        {
            if (Source.Exists (path)) {
                ShowSaveOverMessage (path);
                return;
            }

            Source.SaveAs (path);
            Sync ();
            ReloadInsert (path);
        }

        void SaveOver (PathName path)
        {
            Source.SaveOver (path);
        }

        void Load (PathName path)
        {
            Source.Load (path);
        }

        void Delete (PathName path, NSIndexPath row)
        {
            if (!Source.Exists (path))
                return;
            Source.Delete (path);
            if (row != null) {
                Sync ();
                FileTable.DeleteRows (new NSIndexPath [] { row }, UITableViewRowAnimation.Automatic);
            }
        }



        // ---- アラートの表示 ----
        void ShowSaveAlert ()
        {
            var alertBuilder = new SaveAsAlertBuilder ();

            alertBuilder.Save = (action, textField) => {
                var text = textField.Text;
                var path = Source.GetName (text);

                // var path = FilePathManager.TryConvertToPpgPath (text);

                if (path.HasValue) {
                    SaveAs (path.Value);
                } else {
                    Toast.MakeText (
                        "This name is not available.".Localize (),
                        ToastDuration.Medium
                    ).Show ();
                }
            };

            var alert = alertBuilder.Build ();
            PresentViewController (alert, true, () => {
                // completion
            });
        }


        void ShowSaveOverMessage (PathName path)
        {
            var alertBuilder = new SaveOverAlertBuilder ();

            alertBuilder.SaveOver = action => {
                SaveOver (path);
            };

            var alert = alertBuilder.Build ();
            PresentViewController (alert, true, null);
        }


        //
        void ShowRenameAlert (PathName path)
        {
            var alertBuilder = new RenameAlertBuilder ();

            alertBuilder.TextFieldInit = textField => {
                textField.Text = path.Simple;
            };

            alertBuilder.Rename = (action, textField) => {
                var fileName = textField.Text;
                var newPath = Source.GetName (fileName);
                if (newPath.HasValue) {
                    Rename (path, newPath.Value);

                } else {
                    Toast.MakeText (
                        NSBundle.MainBundle.LocalizedString ("This name is not available.", ""),
                        ToastDuration.Medium
                    ).Show ();
                }
            };

            var alert = alertBuilder.Build ();
            PresentViewController (alert, true, null);
        }



        void ShowFileControlSheet (PathName path, NSIndexPath row)
        {
            var alertBuilder = new FileControlSheetBuilder ();

            alertBuilder.Title = path.Simple;

            alertBuilder.Read = (action) => {
                Load (path);
                DismissAfterLoaded ();
            };

            alertBuilder.SaveOver = (action) => {
                SaveOver (path);
            };

            alertBuilder.Rename = (action) => {
                ShowRenameAlert (path);
            };

            alertBuilder.Clear = (action) => {
                Delete (path, row);
            };
            var view = TableView.CellAt (row).ContentView;
            var alert = alertBuilder.Build (sourceView: view);
            PresentViewController (alert, true, null);
        }

        // ---- TableView Delegate ----
        override
        public void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            var element = GetElement (indexPath);
            TableView.DeselectRow (indexPath, true);
            ShowFileControlSheet (element.PathName, indexPath);
        }

        override
        public nint RowsInSection (UITableView tableView, nint section)
        {
            if (section == 0)
                return CellSourceList.Count;
            return 0;
        }

        override
        public UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            if (CellSourceList.Count == 0)
                CellSource.Empty.ToCell (tableView);
            return CellSourceList [indexPath.Row].ToCell (tableView);
        }


        CellSource GetElement (NSIndexPath path)
        {
            return CellSourceList [path.Row];
        }


        // ---- Utility ----
        static CellSource ToCellSource (PathName source)
        {
            return CellSource.Create (source);
        }

        static List<CellSource> ToCellSourceList (List<PathName> source)
        {
            return (
                from name in source
                select ToCellSource (name)
            ).ToList ();
        }

        sealed class InvalidFileSourceException : Exception
        {
            public InvalidFileSourceException ()
            {
            }

            public InvalidFileSourceException (string message) : base (message)
            {
            }

            public InvalidFileSourceException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
            {
            }

            public InvalidFileSourceException (string message, Exception innerException) : base (message, innerException)
            {
            }
        }

        sealed class CellSource
        {
            static readonly string CellId = "fsFileMenuCell";

            public string Label { get; }
            public PathName PathName { get; }
            public bool IsEmpty { get; }

            // primary
            internal CellSource (string simple, PathName content, bool isEmpty)
            {
                Label = simple;
                PathName = content;
                IsEmpty = isEmpty;
            }

            // factory
            internal static CellSource Create (PathName name)
            {
                return new CellSource (name.Simple, name, true);
            }
            internal static CellSource Empty {
                get {
                    return new CellSource ("Empty", new PathName (), false);
                }
            }

            // cell abstract factory
            internal UITableViewCell ToCell (UITableView tableView)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (CellId);
                if (cell == null)
                    cell = new UITableViewCell (UITableViewCellStyle.Default, CellId);
                cell.TextLabel.Text = Label;
                return cell;
            }
        }
    }
}
