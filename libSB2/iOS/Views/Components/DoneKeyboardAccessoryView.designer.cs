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
    [Register ("DoneKeyboardAccessoryView")]
    partial class DoneKeyboardAccessoryView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem DoneButton { get; set; }

        [Action ("DoneButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DoneButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }
        }
    }
}