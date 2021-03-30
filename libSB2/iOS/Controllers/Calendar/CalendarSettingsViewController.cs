using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class CalendarSettingsViewController : UINavigationController
	{
        
		public CalendarSettingsViewController (IntPtr handle) : base (handle)
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
            // ナビゲーションのタイトル、戻るボタンをセットアップします。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-calendar-menu.title", "");
            ControllerUtils.SetupNavigationBackButton(NavigationItem);
        }


        override
        public void ViewDidDisappear (bool animated)
        {
            // カレンダー設定を保存します。
            Task.Run (async () => {
                await CalendarSettingsManager.Instance.Save ();
            });
        }


        public static void Popover (ViewModel model, CGSize size, UIView source) {
            var topCtrl = ControllerUtils.FindTopViewController();
            var storyboard = UIStoryboard.FromName("CalendarStoryboard", null);
            var ctrl = storyboard.InstantiateViewController ("CalendarSettingsViewController") as CalendarSettingsViewController;
            ctrl.Model = model;
            ctrl.Popover (topCtrl, size, source);
        }


        public void Popover (UIViewController parent, CGSize size, UIView source) {
            ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            //PreferredContentSize = size;
            //ContentSizeForViewInPopover = size;

            var presentationController = PopoverPresentationController;

            if (presentationController != null) {
                //presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Down;
                //presentationController.SourceView = source;
                //presentationController.SourceRect = source.Bounds;
                presentationController.Delegate = new MyDelegate ();
            }

            parent.PresentViewController(this, true, () => {
                
            });
        }


        private sealed class MyDelegate : UIPopoverPresentationControllerDelegate {
            override
            public UIModalPresentationStyle GetAdaptivePresentationStyle (UIPresentationController forPresentationController) {
                return UIModalPresentationStyle.None;
            }
        }
	}
}
