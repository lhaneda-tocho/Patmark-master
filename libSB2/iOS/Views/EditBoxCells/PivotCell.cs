using System;

using Foundation;
using UIKit;
using ObjCRuntime;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class PivotCell : EditBoxCell<PivotCellSource>
    {
        public static readonly NSString Key = new NSString ("PivotCell");
        public static readonly UINib Nib;

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

        static PivotCell ()
        {
            Nib = UINib.FromName ("PivotCell", NSBundle.MainBundle);
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            CCWButton.TouchUpInside += (sender, e) => {
                if (Delegate == null)
                    return;
                Delegate.TouchedCCW ();
            };

            CCWButton.AddGestureRecognizer(CreateStandardRepeater (() => {
                if (Delegate == null)
                    return;
                Delegate.TouchedCCW ();
            }));

            CWButton.TouchUpInside += (sender, e) => {
                if (Delegate == null)
                    return;
                Delegate.TouchedCW ();
            };

            CWButton.AddGestureRecognizer(CreateStandardRepeater (() => {
                if (Delegate == null)
                    return;
                Delegate.TouchedCW ();
            }));
        }

        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            // No state.
        }

        protected PivotCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static PivotCell Create (string title, PivotCellSource mydelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib ("PivotCell", null, null);
            var cell = Runtime.GetNSObject<PivotCell> (arr.ValueAt (0));

            cell.TitleFrame.Title = title;
            cell.Delegate = mydelegate;
            cell.CWButton.SetTitle(
                NSBundle.MainBundle.LocalizedString("ctrl-field-pivot-cw.title", null),
                UIControlState.Normal
            );
            cell.CCWButton.SetTitle(
                NSBundle.MainBundle.LocalizedString("ctrl-field-pivot-ccw.title", null),
                UIControlState.Normal
            );

            return cell;
        }

        protected static UILongPressGestureRecognizer CreateStandardRepeater (Action action)
        {
            var recognizer = CreateLongPressRepeater (interval: 0.2, action: action);
            recognizer.MinimumPressDuration = 1.0;
            return recognizer;
        }


        protected static UILongPressGestureRecognizer CreateLongPressRepeater (double interval, Action action)
        {

            bool repeat = false;
            Action<NSTimer> repeatAction = (timer) => {
                if (repeat) {
                    action ();
                } else {
                    timer.Invalidate ();
                    timer.Dispose ();
                }
            };

            return new UILongPressGestureRecognizer ((sender) => {
                switch (sender.State) {
                case UIGestureRecognizerState.Began: {
                        repeat = true;
                        var myTimer = NSTimer.CreateRepeatingTimer (interval, repeatAction);
                        NSRunLoop.Current.AddTimer (myTimer, NSRunLoopMode.Default);
                        break;
                    }

                case UIGestureRecognizerState.Changed:
                    break;

                default:
                    repeat = false;
                    break;

                }
            });
        }
    }
}
