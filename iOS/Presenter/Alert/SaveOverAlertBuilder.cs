using System;
using UIKit;


namespace TokyoChokoku.Patmark.iOS
{
    public class SaveOverAlertBuilder
    {
        static Action<UIAlertAction> EmptyAction {
            get {
                return (obj) => { };
            }
        }


        Action<UIAlertAction> saveOver = EmptyAction;

        public Action<UIAlertAction> SaveOver {
            set {
                saveOver = NullToEmpty (value);
            }
        }

        static Action<UIAlertAction> NullToEmpty (Action<UIAlertAction> value)
        {
            if (value == null)
                return EmptyAction;
            else
                return value;
        }

        public UIAlertController Build ()
        {
            var alert = UIAlertController.Create (
                "Confirm".Localize (),
                "An file has same name is already exists. Are you sure to overwrite?".Localize (),
                UIAlertControllerStyle.Alert);

            AddCancelButton (alert);
            AddButton (alert, "Save", saveOver);

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

