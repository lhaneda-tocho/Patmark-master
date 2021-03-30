using System;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class UITextFieldExt
    {
        public static void AddEndEditing (this UITextField t, Action<UITextField> action)
        {
            t.Ended += (sender, arg) => {
                action (t);
            };
        }

        public static UITextField [] arrayOf (params UITextField[] ts)
        {
            return ts;
        }

        public static void AddEndEditing (this UITextField [] ts, Action<UITextField> action)
        {
            foreach (var t in ts)
                t.AddEndEditing (action);
        }

        public static void AddInputAccessoryView (this UITextField [] ts, UIView view)
        {
            foreach (var t in ts)
                t.InputAccessoryView = view;
        }
    }
}
