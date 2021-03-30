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
    [Register ("CalendarSettingsShiftController")]
    partial class CalendarSettingsShiftController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton ClearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField CodeTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField EndHourTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField EndMinuteTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton OKButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField StartHourTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.MyTextField StartMinuteTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClearButton != null) {
                ClearButton.Dispose ();
                ClearButton = null;
            }

            if (CodeTextField != null) {
                CodeTextField.Dispose ();
                CodeTextField = null;
            }

            if (EndHourTextField != null) {
                EndHourTextField.Dispose ();
                EndHourTextField = null;
            }

            if (EndMinuteTextField != null) {
                EndMinuteTextField.Dispose ();
                EndMinuteTextField = null;
            }

            if (OKButton != null) {
                OKButton.Dispose ();
                OKButton = null;
            }

            if (StartHourTextField != null) {
                StartHourTextField.Dispose ();
                StartHourTextField = null;
            }

            if (StartMinuteTextField != null) {
                StartMinuteTextField.Dispose ();
                StartMinuteTextField = null;
            }
        }
    }
}