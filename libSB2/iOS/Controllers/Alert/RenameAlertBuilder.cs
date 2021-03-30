using System;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class RenameAlertBuilder
    {
        public string ErrorMessage { get; set; } = "";
        public string PreviousText { get; set; } = null;
            
        static Action<UIAlertAction, UITextField> EmptyAction {
            get {
                return (obj1, obj2) => { };
            }
        }

        static Action<UITextField> EmptyTextFieldInit {
            get {
                return (obj) => { };
            }
        }

        Action<UITextField> textFieldInit;
        Action<UIAlertAction, UITextField> rename;

        public Action<UITextField> TextFieldInit {
            set {
                textFieldInit = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction, UITextField> Rename {
            set {
                rename = NullToEmpty (value);
            }
        }

        static Action<UIAlertAction, UITextField> NullToEmpty (Action<UIAlertAction, UITextField> value)
        {
            if (value == null)
                return EmptyAction;
            else
                return value;
        }

        static Action<UITextField> NullToEmpty (Action<UITextField> value)
        {
            if (value == null)
                return EmptyTextFieldInit;
            else
                return value;
        }

        [Obsolete("BuildAndShowに置き換えられました.このメソッドでは, 表示した後で行う必要のある初期化処理を行うことができません.")]
        public UIAlertController Build () {
            return BuildOnly(ErrorMessage, PreviousText);
        }

        UIAlertController BuildOnly(string errorMessage, string previousText)
        {
            var alert = UIAlertController.Create(
                "Edit name".Localize(),
                String.Format("{0}{1}{2}", "Enter a file name.".Localize(), System.Environment.NewLine, errorMessage),
                UIAlertControllerStyle.Alert
            );

            alert.AddTextField((it) => {
                if (previousText != null)
                    it.Text = previousText;
                textFieldInit(it);
            });
            AddCancelButton(alert);
            AddButton(alert, "Rename", action => {
                rename(action, alert.TextFields[0]);
            });

            return alert;
        }

        public UIAlertController BuildAndShow(Action<UIAlertController> showingNow)
        {
            var errMsg = ErrorMessage;
            var preText = PreviousText;
            var alert = BuildOnly(errMsg, preText);
            showingNow(alert);
            var textField = alert.TextFields[0];
            if(!string.IsNullOrEmpty(errMsg)) {
                textField.Superview.Layer.BorderColor = UIColor.Red.CGColor;
                textField.Superview.Layer.BorderWidth = (nfloat)1.5;
            }
            return alert;
        }

        static void AddCancelButton (UIAlertController alert)
        {
            alert.AddAction (UIAlertAction.Create (
                "Cancel".Localize (), UIAlertActionStyle.Cancel, null));
        }

        static void AddButton (
            UIAlertController alert, string label, Action<UIAlertAction> action)
        {
            alert.AddAction (
                UIAlertAction.Create (
                    label.Localize (),
                    UIAlertActionStyle.Default,
                    action)
            );
        }
    }
}

