using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Monad;



namespace TokyoChokoku.Patmark.iOS
{
    using Presenter;
    using TokyoChokoku.Patmark.Common;

    public partial class LocalFileMenuController : UITableViewController
    {
        public LocalFileManager Source { get; set; }

        List<CellSource> CellSourceList { get; set; }

        // ---- イニシャライザー ----

        public LocalFileMenuController (IntPtr handle) : base (handle)
        {
        }


        override
        public void ViewDidLoad ()
        {
            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = "ctrl-file-local.title".Localize();

            if (Source == null)
                throw new NullReferenceException ();

            CellSourceList = RetrieveFileList();


        }

        //// ---- イベントセンダー ----
        //void WillDismiss (FileMenuDismissedEvent.BecauseOf cause)
        //{
        //    Source.GetAction (new FileMenuDismissedEvent (cause)).Do ( (action) => {
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

        /// <summary>
        /// ファイルリストの取得メソッドです。
        /// 必ずこのメソッドを経由して最新のファイルリストの読み込みを行なってください。
        /// </summary>
        /// <returns></returns>
        List<CellSource> RetrieveFileList()
        {
            var list = ToCellSourceList(Source.FileList);
            list.Sort();
            return list;
        }


        void SyncAndReload()
        {
            // リスト更新
            var old = CellSourceList;
            var current = RetrieveFileList();
            CellSourceList = current;

            // 即時リロード
            void Reload()
            {
                FileTable.ReloadData();
            }

            // 挿入リロード
            void ReloadInsert(PathName name)
            {
                var maybeIndex = current.FindIndex((e) => name.Equals(e.PathName));
                if (maybeIndex < 0)
                {
                    Reload();
                    return;
                }
                var index = (nuint)maybeIndex;
                FileTable.InsertRows(
                    new NSIndexPath[] {
                        NSIndexPath.Create (new nuint[] {0, index })
                    },
                    UITableViewRowAnimation.Automatic
                );
            }


            void ReloadDelete(PathName name)
            {
                var maybeIndex = old.FindIndex((e) => name.Equals(e.PathName));
                if (maybeIndex < 0)
                {
                    Reload();
                    return;
                }
                var index = (nuint)maybeIndex;
                FileTable.DeleteRows(
                    new NSIndexPath[] {
                            NSIndexPath.Create (new nuint[] {0, index })
                    },
                    UITableViewRowAnimation.Automatic
                );
            }

            // NOTE: 非同期アクセスされると動作不良が起きますが、　UI 操作はシングルスレッドに行われるのであまり気にしなくて良さそう。


            // 要素数が1つ増えたか減った場合にのみアニメーションする
            var delta = current.Count - old.Count;
            if (delta == 1)
            {
                var intersect = current.Intersect(old).ToHashSet();
                // 積集合のサイズが、古い方のサイズ であれば、要素が1つ増えていることが確定する。
                if (intersect.Count == old.Count)
                {
                    var added = current.Except(intersect).First();
                    ReloadInsert(added.PathName);
                    return;
                }
            }
            else if (delta == -1)
            {
                var intersect = old.Intersect(current).ToHashSet();
                // 積集合のサイズが、新しい方のサイズ であれば、要素が1つ減っていることが確定する。
                if (intersect.Count == current.Count)
                {
                    var subted = old.Except(intersect).First();
                    ReloadDelete(subted.PathName);
                    return;
                }
            }

            // 更新処理がここまで行われなかった場合はアニメなしで更新する。
            Reload();
        }


        // ---- 操作そのものを表すメソッド ----
        void Rename (PathName target, PathName next)
        {
            if (Source.Exists(next))
            {
                ShowRenameOverMessage(target, next);
                return;
            }

            Source.Rename(target, next);
            SyncAndReload();
        }

        void SaveAs (PathName path)
        {
            if(Source.Loaded.IsEmpty)
            {
                ShowEmptyFileMessage();
                return;
            }
            if (Source.Loaded.HasSerial)
            {
                ShowIncompatibleSerialMessage();
                return;
            }
            if (Source.Exists (path)) {
                ShowSaveOverMessage (path);
                return;
            }

            Source.SaveAs (path);
            SyncAndReload();
        }

        void SaveOver (PathName path)
        {
            if (Source.Loaded.IsEmpty)
            {
                ShowEmptyFileMessage();
                return;
            }
            if (Source.Loaded.HasSerial)
            {
                ShowIncompatibleSerialMessage();
                return;
            }
            Source.SaveOver (path);
            SyncAndReload();
        }

        void Load (PathName path)
        {
            Source.Load (path);
        }

        void Delete (PathName path)
        {
            if (!Source.Exists (path))
                return;
            Source.Delete (path);
            SyncAndReload();
        }



        // ---- アラートの表示 ----
        void ShowSaveAlert ()
        {
            if (Source.Loaded.IsEmpty)
            {
                ShowEmptyFileMessage();
                return;
            }
            if (Source.Loaded.HasSerial)
            {
                ShowIncompatibleSerialMessage();
                return;
            }
            var alertBuilder = new SaveAsAlertBuilder ();
            alertBuilder.TextFieldInit = (tf) =>
            {
                tf.Placeholder = "File name".Localize();
            };
            alertBuilder.Save = (action, textField) => {
                var text = textField.Text;
                var path = Source.GetFilePath (text);

                // var path = FilePathManager.TryConvertToPpgPath (text);

                if (path.HasValue()) {
                    SaveAs (path.Value());
                } else {
                    MyToast.ShowMessage("This name is not available.", duration: MyToast.Short);
                }
            };

            var alert = alertBuilder.Build ();
            PresentViewController (alert, true, () => {
                // completion
            });
        }


        void ShowSaveOverMessage(PathName path)
        {
            var alertBuilder = new SaveOverAlertBuilder();

            alertBuilder.SaveOver = action =>
            {
                SaveOver(path);
            };

            var alert = alertBuilder.Build();
            PresentViewController(alert, true, null);
        }


        //
        void ShowRenameAlert (PathName path)
        {
            var alertBuilder = new RenameAlertBuilder ();

            alertBuilder.TextFieldInit = textField => {
                textField.Placeholder = "File name".Localize();
                textField.Text = path.Simple;
           };

            alertBuilder.Rename = (action, textField) => {
                var fileName = textField.Text;
                var newPath = Source.GetFilePath (fileName);
                if (!newPath.HasValue()) {
                    MyToast.ShowMessage("This name is not available.", duration: MyToast.Short);
                }
                else if(newPath.Value().Full == path.Full){
                    // 何もしません。
                }
                else{
                    Rename(path, newPath.Value());
                }

            };

            var alert = alertBuilder.Build ();
            PresentViewController (alert, true, null);
        }

        void ShowRenameOverMessage(PathName target, PathName next)
        {
            var alertBuilder = new SaveOverAlertBuilder();

            alertBuilder.SaveOver = action => {
                Source.Delete(next);
                Rename(target, next);
            };

            var alert = alertBuilder.Build();
            PresentViewController(alert, true, null);
        }


        void ShowFileControlSheet (PathName path, NSIndexPath row)
        {
            var alertBuilder = new FileControlSheetBuilder ();

            alertBuilder.Title = path.Simple;

            alertBuilder.Read = (action) => {
                Load (path);
                MyToast.ShowMessage("Read successfully.", MyToast.Short);
                DismissAfterLoaded ();
            };

            alertBuilder.SaveOver = (action) => {
                SaveOver (path);
            };

            alertBuilder.Rename = (action) => {
                ShowRenameAlert (path);
            };

            alertBuilder.Clear = (action) => {
                Delete (path);
            };
            var view = TableView.CellAt (row).ContentView;
            var alert = alertBuilder.Build (sourceView: view);
            PresentViewController (alert, true, null);
        }

        void ShowEmptyFileMessage()
        {
            MyToast.ShowMessage("File is empty.", MyToast.Short);
        }

        void ShowIncompatibleSerialMessage()
        {
            MyToast.ShowMessage("msg_serialField_writeFileSelectorLocal", MyToast.Long);
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

        sealed class CellSource: AutoComparableValueType<CellSource>
        {
            static readonly string CellId = "fsFileMenuCell";

            public string   Label { get; }
            public PathName PathName { get; }
            public bool     IsEmpty { get; }

            // primary
            internal CellSource (string simple, PathName content, bool isEmpty)
            {
                Label    = simple;
                PathName = content;
                IsEmpty  = isEmpty;
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

            protected override ListValueType<object> GetValueList()
            {
                return ListValueType<object>.CreateBuilder()
                    .Add(PathName.Full)
                    .Build()
                    ;
            }

            public override int CompareTo(CellSource other)
            {
                if (other == null)
                    return 1;
                return PathName.Full.CompareTo(other.PathName.Full);
            }
        }
    }
}
