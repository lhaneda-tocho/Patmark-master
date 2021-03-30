using System;


using Foundation;
using UIKit;
using ObjCRuntime;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class UpDownTextBoxCell : EditBoxCell <UpDownTextBoxCellSource>
    {
        public static readonly NSString Key = new NSString ("UpDownTextBoxCell");
        public static readonly UINib Nib;

        public override UpDownTextBoxCellSource Delegate {
            set {
                base.Delegate = value;
                if (value != null) {
                    TextBox.Text = value.GetAsString ();
                    TextBox.Placeholder = value.GetDefaultValue ();
                }
            }
        }

        public override string SectionName {
            get { return TitleFrame.Title; }
        }

        public override string Identifier {
            get { return SectionName; }
        }

        static UpDownTextBoxCell ()
        {
            Nib = UINib.FromName ("UpDownTextBoxCell", NSBundle.MainBundle);
        }

        protected UpDownTextBoxCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            AddButton.TouchUpInside += (sender, arg) => {
                if (Delegate == null)
                    return;
                Delegate.TouchedIncrease (TextBox.Text);
                TextBox.Text = Delegate.GetAsString ();
            };

            AddButton.AddGestureRecognizer (CreateStandardRepeater ( ()=> {
                if (Delegate == null)
                    return;
                Delegate.TouchedIncrease (TextBox.Text);
                TextBox.Text = Delegate.GetAsString ();
            }));

            SubButton.TouchUpInside += (sender, arg) => {
                if (Delegate == null)
                    return;
                Delegate.TouchedDecrease (TextBox.Text);
                TextBox.Text = Delegate.GetAsString ();
            };

            SubButton.AddGestureRecognizer (CreateStandardRepeater (() => {
                if (Delegate == null)
                    return;
                Delegate.TouchedDecrease (TextBox.Text);
                TextBox.Text = Delegate.GetAsString ();
            }));

            //TextBox.EditingDidBegin += (sender, e) => {
            //};

            TextBox.EditingChanged += (sender, arg) => {
                if (Delegate == null) {
                    TextBox.Text = "";
                    return;
                }
                if (string.IsNullOrEmpty (TextBox.Text)) {
                    var def = Delegate.GetDefaultValue ();
                    Delegate.ChangedText (def);
                    return;
                }
                Delegate.ChangedText (TextBox.Text);
            };

            TextBox.EditingDidEnd += (sender, e) => {
                if (Delegate == null) {
                    TextBox.Text = "";
                    return;
                }
                if (string.IsNullOrEmpty (TextBox.Text)) {
                    var def = Delegate.GetDefaultValue ();
                    Delegate.ChangedText (def);
                    TextBox.Text = Delegate.GetAsString ();
                    return;
                }
                Delegate.ChangedText (TextBox.Text);
                TextBox.Text = Delegate.GetAsString ();
            };
        }


        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            TextBox.Text = Delegate.GetAsString ();
        }


        public static UpDownTextBoxCell Create (string title, UpDownTextBoxCellSource mydelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib ("UpDownTextBoxCell", null, null);
            var cell = Runtime.GetNSObject<UpDownTextBoxCell> (arr.ValueAt (0));

            cell.TitleFrame.Title  = title;
            cell.Unit.Text   = mydelegate.GetUnit();
            cell.Delegate    = mydelegate;
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
