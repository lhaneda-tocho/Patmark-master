using System;
using UIKit;



namespace TokyoChokoku.Patmark.iOS
{
    public class SaveAsAlertBuilder
    {
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

        Action<UITextField> textFieldInit = (tf) => { };
        Action<UIAlertAction, UITextField> save = (alert, tf) => { };

        public Action<UITextField> TextFieldInit {
            set {
                textFieldInit = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction, UITextField> Save {
            set {
                save = NullToEmpty (value);
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

        public UIAlertController Build ()
        {

            var alert = UIAlertController.Create (
                "Save as".Localize (),
                "Enter a file name.".Localize (),
                UIAlertControllerStyle.Alert
            );

            alert.AddTextField (textFieldInit);
            AddCancelButton (alert);
            AddButton (alert, "Save", action => {
                save (action, alert.TextFields [0]);
            });

            return alert;
        }

        static void AddCancelButton (UIAlertController alert)
        {
            alert.AddAction (UIAlertAction.Create (
                "Cancel".Localize (), UIAlertActionStyle.Cancel, (obj) => { }));
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

