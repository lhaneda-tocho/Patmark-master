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
    [Register ("AppInformationViewController")]
    partial class AppInformationViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel VersionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WebSiteLinkButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (VersionLabel != null) {
                VersionLabel.Dispose ();
                VersionLabel = null;
            }

            if (WebSiteLinkButton != null) {
                WebSiteLinkButton.Dispose ();
                WebSiteLinkButton = null;
            }
        }
    }
}