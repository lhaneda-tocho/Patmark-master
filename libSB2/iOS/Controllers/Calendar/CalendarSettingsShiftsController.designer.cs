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
    [Register ("CalendarSettingsShiftsController")]
    partial class CalendarSettingsShiftsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ShiftsView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ShiftsView != null) {
                ShiftsView.Dispose ();
                ShiftsView = null;
            }
        }
    }
}