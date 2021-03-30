using System;

using Foundation;
using UIKit;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class AlartTextFieldAgency
    {
        static AlartTextFieldAgency()
        {
        }

        public delegate void OnTextSubmit(String text);

        public static void Show(String title, String message, String initialValue, OnTextSubmit onTextSubmit)
        {
            //Create Alert

            var textInputAlertController = UIAlertController.Create(
                title,
                message,
                UIAlertControllerStyle.Alert
            );

            textInputAlertController.AddTextField(uiTextField =>
            {
                if (initialValue != null)
                {
                    uiTextField.Text = initialValue;
                }
            });

            var cancelAction = UIAlertAction.Create(
                NSBundle.MainBundle.LocalizedString("Cancel", ""),
                UIAlertActionStyle.Cancel,
                null);

            var okayAction = UIAlertAction.Create(
                NSBundle.MainBundle.LocalizedString("OK", ""),
                UIAlertActionStyle.Default, (action) =>
                {
                    var textField = textInputAlertController.TextFields[0];
                    onTextSubmit(textField.Text);
                });

            textInputAlertController.AddAction(cancelAction);
            textInputAlertController.AddAction(okayAction);

            //Present Alert

            ControllerUtils.FindTopViewController()
                           .PresentViewController(
                               textInputAlertController,
                               true,
                               null
                              );
        }
    }
}

