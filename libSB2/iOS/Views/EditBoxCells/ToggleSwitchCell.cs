using System;
using ObjCRuntime;

using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class ToggleSwitchCell : EditBoxCell<ToggleSwitchCellSource>
    {
        public static readonly NSString Key = new NSString ("ToggleSwitchCell");
        public static readonly UINib Nib;
        public static nfloat Height {
            get {
                // xib に合わせる．
                return 40;
            }
        }

        public override ToggleSwitchCellSource Delegate {
            set {
                base.Delegate = value;
                if (value != null) {
                    ToggleSwitch.On = value.CurrentState ();
                }
            }
        }

        public override string SectionName {
            get {
                return TitleFrame.Title;
            }
        }

        public override string Identifier {
            get {
                return SectionName;
            }
        }

        static ToggleSwitchCell ()
        {
            Nib = UINib.FromName ("ToggleSwitchCell", NSBundle.MainBundle);
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            ToggleSwitch.On = false;
            ToggleSwitch.ValueChanged += (sender, e) => {
                if (Delegate == null)
                    return;
                Delegate.ChangedState (ToggleSwitch.On);
            };
        }


        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            ToggleSwitch.SetState (Delegate.CurrentState(), true);
        }


        protected ToggleSwitchCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static ToggleSwitchCell Create (string title, ToggleSwitchCellSource mydelegate)
        {
            var arr  = NSBundle.MainBundle.LoadNib ("ToggleSwitchCell", null, null);
            var cell = Runtime.GetNSObject<ToggleSwitchCell> (arr.ValueAt (0));

            cell.TitleFrame.Title = title;
            cell.Delegate = mydelegate;

            return cell;
        }
    }
}
