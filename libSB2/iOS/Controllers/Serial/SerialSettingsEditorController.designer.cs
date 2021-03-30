// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [Register ("SerialSettingsEditorController")]
    partial class SerialSettingsEditorController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField ClearingTimeHHTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField ClearingTimeMMTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl CounterClearingConditionSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField CurrentValueTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl FormatSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton Insert { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField MaxValueTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField MinValueTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField RepeatingCountTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.DoneKeyboardAccessoryView TextFieldInputAccessoryView { get; set; }

        [Action ("CancelButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("Insert_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Insert_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (ClearingTimeHHTextField != null) {
                ClearingTimeHHTextField.Dispose ();
                ClearingTimeHHTextField = null;
            }

            if (ClearingTimeMMTextField != null) {
                ClearingTimeMMTextField.Dispose ();
                ClearingTimeMMTextField = null;
            }

            if (CounterClearingConditionSelector != null) {
                CounterClearingConditionSelector.Dispose ();
                CounterClearingConditionSelector = null;
            }

            if (CurrentValueTextField != null) {
                CurrentValueTextField.Dispose ();
                CurrentValueTextField = null;
            }

            if (FormatSelector != null) {
                FormatSelector.Dispose ();
                FormatSelector = null;
            }

            if (Insert != null) {
                Insert.Dispose ();
                Insert = null;
            }

            if (MaxValueTextField != null) {
                MaxValueTextField.Dispose ();
                MaxValueTextField = null;
            }

            if (MinValueTextField != null) {
                MinValueTextField.Dispose ();
                MinValueTextField = null;
            }

            if (RepeatingCountTextField != null) {
                RepeatingCountTextField.Dispose ();
                RepeatingCountTextField = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (TextFieldInputAccessoryView != null) {
                TextFieldInputAccessoryView.Dispose ();
                TextFieldInputAccessoryView = null;
            }
        }
    }
}