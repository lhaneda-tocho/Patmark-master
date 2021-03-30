using System;
using UIKit;



namespace TokyoChokoku.Patmark.iOS
{
    public class RenameAlertBuilder
    {
        public string ErrorMessage = "";
        public string PreviousText = null;

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

        [Obsolete("表示後に必要となる設定ができないため，非推奨となりました.")]
        public UIAlertController Build () {
            var alert = UIAlertController.Create (
                "Rename".Localize (),
                String.Format("{0}{1}{2}", "Enter a file name.".Localize (), Environment.NewLine, ErrorMessage),
                UIAlertControllerStyle.Alert
            );

            AddTextField(alert);

            AddCancelButton (alert);

            AddButton (alert, "Rename", action => {
                rename (action, alert.TextFields [0]);
                alert.DismissViewController(true, () => { });
            });

            return alert;
        }


        UIAlertController BuildOnly() {
            var alert = UIAlertController.Create(
                "Rename".Localize(),
                String.Format("{0}{1}{2}", "Enter a file name.".Localize(), Environment.NewLine, ErrorMessage),
                UIAlertControllerStyle.Alert
            );

            AddTextField(alert);

            AddCancelButton(alert);

            AddButton(alert, "Rename", action => {
                rename(action, alert.TextFields[0]);
                alert.DismissViewController(true, () => { });
            });

            return alert;
        }

        public UIAlertController BuildAndShow(Action<UIAlertController> showingNow) {
            var alert = BuildOnly();
            showingNow(alert);

            var textField = alert.TextFields[0];

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                textField.Superview.Layer.BorderColor = UIColor.Red.CGColor;
                textField.Superview.Layer.BorderWidth = (nfloat)1.5;
            }

            return alert;
        }

        void AddTextField(UIAlertController alert) {
            alert.AddTextField((tf) => {
                // TextFieldの初期化
                if (PreviousText != null)
                    tf.Text = PreviousText;
                textFieldInit(tf);
            });
        }

        static void AddCancelButton (UIAlertController alert)
        {
            alert.AddAction (UIAlertAction.Create (
                "Cancel".Localize (), UIAlertActionStyle.Cancel, null));
        }

        static void AddButton (
            UIAlertController alert, string label, Action<UIAlertAction> action)
        {
            var addingAction = UIAlertAction.Create(
                    label.Localize(),
                    UIAlertActionStyle.Default,
                    action
            );
            alert.AddAction (
                addingAction
            );
        }
    }
}

