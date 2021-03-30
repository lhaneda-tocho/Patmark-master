using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class AppInformationViewController : UIViewController
	{
		public AppInformationViewController (IntPtr handle) : base (handle)
		{
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad(){
			base.ViewDidLoad ();

            // ナビゲーションのタイトルをセットアップします。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-about-this-app.title", null);

			// sets app version.
            VersionLabel.Text = String.Format("ver {0}", NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"]);

			// 
			WebSiteLinkButton.TouchUpInside += (object sender, EventArgs e) => {
				UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.tokyo-chokoku.co.jp"));
			};
            WebSiteLinkButton.Enabled = !CommunicationClientManager.Instance.IsOnline();
		}
	}
}
