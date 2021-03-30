using System;

using ToastIOS;
using Foundation;
using UIKit;
using ObjCRuntime;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class TextBoxCell : EditBoxCell<TextBoxCellSource>
    {
        public static readonly NSString Key = new NSString ("TextBoxCell");
        public static readonly UINib Nib;
        public static nfloat Height {
            get {
                return 100;
            }
        }


        public event Action<string, IEditBoxInteractionDelegate> RequestInteraction;


        public override TextBoxCellSource Delegate {
            set {
                base.Delegate = value;
                if (value != null)
                    TextBox.Text = value.GetText ();
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

        static TextBoxCell ()
        {
            Nib = UINib.FromName ("TextBoxCell", NSBundle.MainBundle);
        }


        protected TextBoxCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            // Xボタンで 1文字削除
            DeleteButton.TouchUpInside += (sender, e) => {
                if (Delegate == null)
                    return;
                DeleteOneCharacter ();
                Delegate.ChangedText (TextBox.Text);
            };

            // Xボタンで連続削除
            DeleteButton.AddGestureRecognizer (
                CreateStandardRepeater (() => {
                    if (Delegate == null)
                        return;
                    DeleteOneCharacter ();
                    Delegate.ChangedText (TextBox.Text);
                })
            );

            // テキストの内容を変更している時
            TextBox.EditingChanged += (sender, e) => {
                var result = Delegate.ChangedText (TextBox.Text);

                if (result.HasError)
                    PopupErrorMessage (result);
            };

            // テキストの内容の変更が終了した時．
            TextBox.EditingDidEnd += (sender, e) => {
                var result = Delegate.ChangedText (TextBox.Text);

                if (result.HasError)
                    PopupErrorMessage (result);
                
                TextBox.Text = Delegate.GetText();
            };
        }

        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            TextBox.Text = Delegate.GetText ();
        }


        public static TextBoxCell Create (string title, TextBoxCellSource myDelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib ("TextBoxCell", null, null);
            var cell = Runtime.GetNSObject<TextBoxCell> (arr.ValueAt (0));

            cell.TitleFrame.Title = title;
            cell.FontTitle.Text = myDelegate.GetFontTitle();
            cell.Delegate = myDelegate;
            return cell;
        }


        public void DeleteOneCharacter ()
        {
            var text = TextBox.Text;

            if (text.Length > 0) {
                TextBox.Text = text.Remove (text.Length - 1);
            }
        }


        public void TryInsertText(string text)
        {
            text = TextBox.Text + text;
            var result = Delegate.ChangedText(text);

            if (result.HasError)
            {
                TextBox.Text = Delegate.GetText();
                PopupErrorMessage(result);
                return;
            }

            TextBox.Text = Delegate.GetText();
        }

        public void PopupErrorMessage (ValidationResult result)
        {
            string message = Localize (result);
            Toast.MakeText (message)
                 .SetFontSize (15)
                 .SetDuration (3000)
                 .SetGravity (ToastGravity.Center)
                 .Show (ToastType.Error);
        }

        public UITextField GetTextBox ()
        {
            return TextBox;
        }

        public void ClearListeners ()
        {
            RequestInteraction = null;
        }

        string Localize (ValidationResult result)
        {
            var sb = new System.Text.StringBuilder ();
            bool start = true;
            foreach (var e in result.ErrorCodes) {
                if (!start)
                    sb.AppendLine ();
                sb.Append (e.GetLocalizationId ().Localize ());
            }
            return sb.ToString ();
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
