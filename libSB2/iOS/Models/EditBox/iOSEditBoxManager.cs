using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// 編集ボックスの開閉を担当するクラスです．
    /// managerの内容の読み取りのみを行います．
    /// </summary>
    public class iOSEditBoxManager
    {
        PropertyEditBox TableView { get; }

        internal iOSEditBoxManager (PropertyEditBox tableView)
        {
            if (tableView == null)
                throw new System.NullReferenceException ();
            TableView = tableView;
        }

        /// <summary>
        /// フィールドをタップされた時に編集ボックスの開閉を行うイベントリスナです．
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        public void ListenAddFieldEvent (AddField eventArg)
        {
            Build (eventArg.FieldManager);
        }

        /// <summary>
        /// フィールドをタップされた時に編集ボックスの開閉を行うイベントリスナです．
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        public void ListenTapFieldEvent (TapField eventArg)
        {
            switch (eventArg.Action) {
                case TapField.To.SelectOther: {
                    Build (eventArg.FieldManager);
                    return;
                }
                case TapField.To.Deselect: {
                    Close ();
                    return;
                }
            }
        }

        public void ListenPanFieldEvent (PanField eventArg)
        {
            switch (eventArg.Action) {
                case PanField.To.SelectOther: {
                    Build (eventArg.FieldManager);
                    return;
                }
                case PanField.To.Deselect: {
                    Close ();
                    return;
                }
            }
        }

        /// <summary>
        /// 編集中のオブジェクトに対応する Editbox を作成します．
        /// </summary>
        /// <param name="manager">Manager.</param>
        public void Build (iOSFieldManager manager)
        {
            var edited = manager.Editing;

            if (edited == null) {
                TableView.Hidden = true;
                return;
            }

            // 編集領域の表示状態を切り替えます。
            TableView.Hidden = false;

            var visitor = new UIFieldEditBoxFactory ();
            var builder = new iOSEditBoxBuilder (manager, manager.CreateEditBoxCommonDelegate (this));

            builder.Append (edited);

            edited.Accept (visitor, builder);

            // Previewコンテキストを触るためのセルをビルドします。
            builder.Append (
                CreateCopyRemoveCellDelegate (manager),
                "Other"
            );

            var source = builder.Build ();

            // 表の設定
            TableView.SetPropertyEditBoxSource (source);

            // 操作モードを画面に反映
            TableView.UserInteractionEnabled = OperationModeManager.IsAdministrator ();
        }

        /// <summary>
        /// Editbox を閉じます．
        /// </summary>
        public void Close ()
        {
            TableView.Hidden = true;
            TableView.FindFirstResponder ().Do (active => {
                active.ResignFirstResponderWithAnimation (0.5);
            });
        }


        // ---- 追加・削除のトリガー作成 ----
        CopyRemoveCellDelegate CreateCopyRemoveCellDelegate (iOSFieldManager fieldManager)
        {
            return new CopyRemoveCellDelegate (fieldManager, this);
        }
    }
}

