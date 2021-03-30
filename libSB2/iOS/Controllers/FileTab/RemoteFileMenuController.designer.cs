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
    [Register ("RemoteFileMenuController")]
    partial class RemoteFileMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FileTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ReloadButton { get; set; }

        [Action ("CancelButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (FileTable != null) {
                FileTable.Dispose ();
                FileTable = null;
            }

            if (ReloadButton != null) {
                ReloadButton.Dispose ();
                ReloadButton = null;
            }
        }
    }
}