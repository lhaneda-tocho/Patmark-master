using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class CopyRemoveCellDelegate : IEditBoxCellDelegate
    {
        readonly iOSFieldManager   manager;
        readonly iOSEditBoxManager editbox;

        public CopyRemoveCellDelegate (iOSFieldManager manager, iOSEditBoxManager editbox)
        {
            if (manager == null)
                throw new NullReferenceException ();
            this.manager = manager;
            this.editbox = editbox;
        }

        public void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
        {
            // 変化する状態を持っていません
        }

        // ---- 追加・削除のトリガー ----
        public void RequestToCopyEditing ()
        {
            manager.Copy ();
        }

        public void RequestToDeleteEditing ()
        {
            manager.CheckThenDelete (editbox);
        }

        public void RequestToDeleteAll ()
        {
            manager.CheckThenDeleteAll ();
        }
    }
}

