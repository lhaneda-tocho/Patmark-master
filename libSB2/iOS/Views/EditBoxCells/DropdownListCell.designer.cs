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
    [Register ("DropdownListCell")]
    partial class DropdownListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DropdownButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.CellFrameView TitleFrame { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DropdownButton != null) {
                DropdownButton.Dispose ();
                DropdownButton = null;
            }

            if (TitleFrame != null) {
                TitleFrame.Dispose ();
                TitleFrame = null;
            }
        }
    }
}