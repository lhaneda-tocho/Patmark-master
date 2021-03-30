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
    [Register ("FileMenuController")]
    partial class FileMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton ControllerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.FileMenuView FileMenuView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton LocalButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton NewestButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ControllerButton != null) {
                ControllerButton.Dispose ();
                ControllerButton = null;
            }

            if (FileMenuView != null) {
                FileMenuView.Dispose ();
                FileMenuView = null;
            }

            if (LocalButton != null) {
                LocalButton.Dispose ();
                LocalButton = null;
            }

            if (NewestButton != null) {
                NewestButton.Dispose ();
                NewestButton = null;
            }
        }
    }
}