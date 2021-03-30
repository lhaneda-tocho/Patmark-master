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
    [Register ("FileMenuController")]
    partial class FileMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell BtnLatest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell BtnLocal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell BtnRemote { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnLatest != null) {
                BtnLatest.Dispose ();
                BtnLatest = null;
            }

            if (BtnLocal != null) {
                BtnLocal.Dispose ();
                BtnLocal = null;
            }

            if (BtnRemote != null) {
                BtnRemote.Dispose ();
                BtnRemote = null;
            }
        }
    }
}