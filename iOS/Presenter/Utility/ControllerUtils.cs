using System;

using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace TokyoChokoku.Patmark.iOS.Presenter.Utility
{
    public static class ControllerUtils
    {
        static ControllerUtils()
        {
        }

        public static UIViewController FindTopViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }
            return vc;
        }

        ///// <summary>
        ///// Setups the navigation back button.
        ///// </summary>
        ///// <returns>The navigation back button.</returns>
        ///// <param name="item">Item.</param>
        //public static void SetupNavigationBackButton(UINavigationItem item)
        //{
        //    item.BackBarButtonItem = new UIBarButtonItem(
        //        NSBundle.MainBundle.LocalizedString("ctrl-preferences.title", ""),
        //        UIBarButtonItemStyle.Plain,
        //        null
        //    );
        //}
    }
}

