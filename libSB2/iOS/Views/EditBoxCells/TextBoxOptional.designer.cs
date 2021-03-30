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
    [Register ("TextBoxOptional")]
    partial class TextBoxOptional
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton Calendar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton Serial { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.CellFrameView TitleFrame { get; set; }

        [Action ("Calender_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Calender_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton sender);

        [Action ("Serial_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Serial_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Calendar != null) {
                Calendar.Dispose ();
                Calendar = null;
            }

            if (Serial != null) {
                Serial.Dispose ();
                Serial = null;
            }

            if (TitleFrame != null) {
                TitleFrame.Dispose ();
                TitleFrame = null;
            }
        }
    }
}