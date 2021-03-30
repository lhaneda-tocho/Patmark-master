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
    [Register ("SettingsController")]
    partial class SettingsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell AppVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.Settings.MachineModelNoCell ModelNoSetting { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell RomVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AppVersion != null) {
                AppVersion.Dispose ();
                AppVersion = null;
            }

            if (ModelNoSetting != null) {
                ModelNoSetting.Dispose ();
                ModelNoSetting = null;
            }

            if (RomVersion != null) {
                RomVersion.Dispose ();
                RomVersion = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}