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
    [Register ("PickerViewController")]
    partial class PickerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem DoneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView Picker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.UITransparentView RootView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }

            if (Picker != null) {
                Picker.Dispose ();
                Picker = null;
            }

            if (RootView != null) {
                RootView.Dispose ();
                RootView = null;
            }
        }
    }
}