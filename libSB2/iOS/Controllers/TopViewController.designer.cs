// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [Register ("TopViewController")]
    partial class TopViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView VersionLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (VersionLabel != null) {
                VersionLabel.Dispose ();
                VersionLabel = null;
            }
        }
    }
}