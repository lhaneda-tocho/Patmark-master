using System;
using UIKit;

namespace TokyoChokoku.Patmark.iOS
{
    public class DoYouChangeMachineModelAlert
    {

        public event Action OnClickFitButton = ()=>{};


        public UIAlertController Build ()
        {
            var alert = UIAlertController.Create (
                "Change of machine model".Localize (),
                "Do you change machine model?".Localize (),
                UIAlertControllerStyle.Alert);

            AddIgnoreButton (alert);
            AddFitButton(alert, OnClickFitButton);
            return alert;
        }

        static void AddIgnoreButton (UIAlertController alert)
        {
            alert.AddAction (UIAlertAction.Create (
                "Cancel".Localize (), UIAlertActionStyle.Cancel, null));
        }

        static void AddFitButton (UIAlertController alert, Action action)
        {
            alert.AddAction (
                UIAlertAction.Create (
                    "Apply".Localize(),
                    UIAlertActionStyle.Default,
                    (_)=>action?.Invoke() )
            );
        }
    }
}

