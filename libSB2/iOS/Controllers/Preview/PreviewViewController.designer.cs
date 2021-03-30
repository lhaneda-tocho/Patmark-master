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
    [Register ("PreviewViewController")]
    partial class PreviewViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonPosReset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonZoomIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonZoomOut { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonZoomReset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FileNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.PropertyEditBox PropertyEditorsBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBar TabBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBarItem TabField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBarItem TabFile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBarItem TabMarking { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBarItem TabSettings { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.UITransparentView Viewport { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WifiStatusButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ButtonPosReset != null) {
                ButtonPosReset.Dispose ();
                ButtonPosReset = null;
            }

            if (ButtonZoomIn != null) {
                ButtonZoomIn.Dispose ();
                ButtonZoomIn = null;
            }

            if (ButtonZoomOut != null) {
                ButtonZoomOut.Dispose ();
                ButtonZoomOut = null;
            }

            if (ButtonZoomReset != null) {
                ButtonZoomReset.Dispose ();
                ButtonZoomReset = null;
            }

            if (FileNameLabel != null) {
                FileNameLabel.Dispose ();
                FileNameLabel = null;
            }

            if (PropertyEditorsBox != null) {
                PropertyEditorsBox.Dispose ();
                PropertyEditorsBox = null;
            }

            if (TabBar != null) {
                TabBar.Dispose ();
                TabBar = null;
            }

            if (TabField != null) {
                TabField.Dispose ();
                TabField = null;
            }

            if (TabFile != null) {
                TabFile.Dispose ();
                TabFile = null;
            }

            if (TabMarking != null) {
                TabMarking.Dispose ();
                TabMarking = null;
            }

            if (TabSettings != null) {
                TabSettings.Dispose ();
                TabSettings = null;
            }

            if (Viewport != null) {
                Viewport.Dispose ();
                Viewport = null;
            }

            if (WifiStatusButton != null) {
                WifiStatusButton.Dispose ();
                WifiStatusButton = null;
            }
        }
    }
}