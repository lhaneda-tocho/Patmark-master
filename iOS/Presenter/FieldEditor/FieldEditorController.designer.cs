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

namespace TokyoChokoku.Patmark.iOS.Presenter.FieldEditor
{
    [Register ("FieldEditorController")]
    partial class FieldEditorController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.Component.FlashButton NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RootView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TextField { get; set; }

        [Action ("NextButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NextButton_TouchUpInside (TokyoChokoku.Patmark.iOS.Presenter.Component.FlashButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (RootView != null) {
                RootView.Dispose ();
                RootView = null;
            }

            if (TextField != null) {
                TextField.Dispose ();
                TextField = null;
            }
        }
    }
}