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
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl BarcodeNWaySelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell FinishAppCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl MachineModelSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl OperationModeSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl SolenoidTypeSelector { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell WifiSettingsCell { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BarcodeNWaySelector != null) {
                BarcodeNWaySelector.Dispose ();
                BarcodeNWaySelector = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (FinishAppCell != null) {
                FinishAppCell.Dispose ();
                FinishAppCell = null;
            }

            if (MachineModelSelector != null) {
                MachineModelSelector.Dispose ();
                MachineModelSelector = null;
            }

            if (OperationModeSelector != null) {
                OperationModeSelector.Dispose ();
                OperationModeSelector = null;
            }

            if (SolenoidTypeSelector != null) {
                SolenoidTypeSelector.Dispose ();
                SolenoidTypeSelector = null;
            }

            if (WifiSettingsCell != null) {
                WifiSettingsCell.Dispose ();
                WifiSettingsCell = null;
            }
        }
    }
}