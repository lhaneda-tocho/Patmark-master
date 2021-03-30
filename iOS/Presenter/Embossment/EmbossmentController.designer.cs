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

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    [Register ("EmbossmentController")]
    partial class EmbossmentController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ClearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FieldListContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FilesButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PreviewContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SettingsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WifiStatusButton { get; set; }

        [Action ("WifiStatusButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void WifiStatusButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ClearButton != null) {
                ClearButton.Dispose ();
                ClearButton = null;
            }

            if (FieldListContainer != null) {
                FieldListContainer.Dispose ();
                FieldListContainer = null;
            }

            if (FilesButton != null) {
                FilesButton.Dispose ();
                FilesButton = null;
            }

            if (PreviewContainer != null) {
                PreviewContainer.Dispose ();
                PreviewContainer = null;
            }

            if (SettingsButton != null) {
                SettingsButton.Dispose ();
                SettingsButton = null;
            }

            if (UIContainer != null) {
                UIContainer.Dispose ();
                UIContainer = null;
            }

            if (WifiStatusButton != null) {
                WifiStatusButton.Dispose ();
                WifiStatusButton = null;
            }
        }
    }
}