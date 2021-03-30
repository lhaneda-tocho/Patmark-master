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

namespace TokyoChokoku.Patmark.iOS
{
    [Register ("LocalFileMenuController")]
    partial class LocalFileMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem AddButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FileTable { get; set; }

        [Action ("AddButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("CancelButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddButton != null) {
                AddButton.Dispose ();
                AddButton = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (FileTable != null) {
                FileTable.Dispose ();
                FileTable = null;
            }
        }
    }
}