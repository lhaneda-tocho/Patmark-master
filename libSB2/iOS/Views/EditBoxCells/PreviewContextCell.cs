using System;

using Foundation;
using UIKit;
using ObjCRuntime;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class PreviewContextCell : EditBoxCell<CopyRemoveCellDelegate>
    {
        public static readonly NSString Key = new NSString("PreviewContextCell");
        public static readonly UINib Nib;

        IEditBoxCommonDelegate CommonDelegate { get; set; }

        string section = "Preview Context";

        public override string SectionName {
            get {
                return section;
            }
        }

        public override string Identifier {
            get {
                return SectionName;
            }
        }

        static PreviewContextCell()
        {
            Nib = UINib.FromName("PreviewContextCell", NSBundle.MainBundle);
        }

        protected PreviewContextCell(IntPtr handle) : base(handle)
        {

        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ReplicationButton.TouchUpInside += (sender, e) =>
            {
                Delegate.RequestToCopyEditing();
            };

            DeletionButton.TouchUpInside += (sender, e) =>
            {
                Delegate.RequestToDeleteEditing();
            };
        }

        void InitProperties (CopyRemoveCellDelegate copyDeleteDelegate, IEditBoxCommonDelegate cellDelegate)
        {
            Delegate = copyDeleteDelegate;
            CommonDelegate = cellDelegate;
        }

        public static PreviewContextCell Create(string index, CopyRemoveCellDelegate copyDeleteDelegate, IEditBoxCommonDelegate cellDelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib("PreviewContextCell", null, null);
            var cell = Runtime.GetNSObject<PreviewContextCell>(arr.ValueAt(0));

            cell.section = index;
            cell.InitProperties (copyDeleteDelegate, cellDelegate);

            cell.ReplicationButton.SetTitle(
                NSBundle.MainBundle.LocalizedString("ctrl-field-preview-replication.title", null),
                UIControlState.Normal
            );
            cell.DeletionButton.SetTitle(
                NSBundle.MainBundle.LocalizedString("ctrl-field-preview-deletion.title", null),
                UIControlState.Normal
            );

            return cell;
        }



        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            // No State.
        }
    }
}
