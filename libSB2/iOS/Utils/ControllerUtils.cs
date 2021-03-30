using System;

using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
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

        public static async Task<Nil> ActionWithLoadingOverlay(Func<Task> func)
        {
            var ctrl = FindTopViewController();

            var bounds = UIScreen.MainScreen.Bounds;
            var loadingOverlay = new LoadingOverlay(bounds);
            if (ctrl.View != null)
            {
                ctrl.View.Add(loadingOverlay);
            }
            await func.Invoke();
            loadingOverlay.Hide();
            return null;
        }

        /// <summary>
        /// Setups the navigation back button.
        /// </summary>
        /// <returns>The navigation back button.</returns>
        /// <param name="item">Item.</param>
        public static void SetupNavigationBackButton(UINavigationItem item)
        {
            item.BackBarButtonItem = new UIBarButtonItem(
                NSBundle.MainBundle.LocalizedString("ctrl-preferences.title", ""),
                UIBarButtonItemStyle.Plain,
                null
            );
        }
    }
}

