// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TokyoChokoku.Patmark.iOS
{
	[Register ("SettingsController")]
	partial class SettingsController
	{
		[Outlet]
		UIKit.UITableViewCell AppLanguage { get; set; }

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
			if (AppLanguage != null) {
				AppLanguage.Dispose ();
				AppLanguage = null;
			}

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
