using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class CalendarSettingsMenuController : UITableViewController
	{
		public CalendarSettingsMenuController (IntPtr handle) : base (handle)
		{
		}

        public ViewModel Model { get; set; }

        public class ViewModel
        {
            public CalendarSettingsInsertionController.InsertionDelegate Insert;
            public ViewModel(CalendarSettingsInsertionController.InsertionDelegate insert)
            {
                this.Insert = insert;
            }
        }

        override
        public void ViewDidLoad()
        {
            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-calendar-menu.title", "");
            NavigationItem.BackBarButtonItem = new UIBarButtonItem ("navi.back".Localize (), UIBarButtonItemStyle.Plain, null);

            var parentCtrl = this.ParentViewController as CalendarSettingsViewController;
            this.Model = new ViewModel (parentCtrl.Model.Insert);

            this.DoneButton.Clicked += (object sender, EventArgs e) => {
                this.DismissViewController(true, null);
            };
        }

        override
        public void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == null) {
                return;
            }
            if (segue.Identifier.Equals ("ShowInsertion")) {
                var dstCtrl = (CalendarSettingsInsertionController)segue.DestinationViewController;
                dstCtrl.Model = new CalendarSettingsInsertionController.ViewModel (Model.Insert);
            }
        }

        /// <summary>
        /// Unwind segue's jump point.
        /// </summary>
        /// <param name="segue">Segue.</param>
        [Action ("OnCalendarSettingsInsertionCommited:")]
        public void OnCalendarSettingsInsertionCommited (UIStoryboardSegue segue) {
            var field = (CalendarSettingsInsertionController)segue.SourceViewController;
            this.DismissViewController(true, null);
            Console.WriteLine ("From Insertion");
        }
	}
}
