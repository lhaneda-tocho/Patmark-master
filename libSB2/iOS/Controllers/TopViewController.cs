using System;
using System.CodeDom.Compiler;
using UIKit;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class TopViewController : UIViewController
	{
		public TopViewController (IntPtr handle) : base (handle)
		{
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad(){
			base.ViewDidLoad ();
			// sets app version.
			VersionLabel.Text = String.Format("ver {0}", NSBundle.MainBundle.InfoDictionary["CFBundleVersion"]);

			// waits 1 second and goes to preview page.
			TimerPlatform.SetTimeout (
				() => {
                    this.PerformSegue ("GotoPreview", this);
				}, 0 // 1000
			);
		}
	}
}
