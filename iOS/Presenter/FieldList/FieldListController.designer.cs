// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TokyoChokoku.Patmark.iOS
{
    [Register ("FieldListController")]
    partial class FieldListController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FieldList { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FieldList != null) {
                FieldList.Dispose ();
                FieldList = null;
            }
        }
    }
}