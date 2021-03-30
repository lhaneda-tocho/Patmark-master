
using System;

using Foundation;
using UIKit;
using iiTooltip;

namespace iiToolTip.Unified
{
    public partial class HomeScreen : UIViewController
    {
        CreateTooltip tooltip;

        public HomeScreen()
            : base("HomeScreen", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIImage image = UIImage.FromBundle("tooltipimage");
            UILabel label = new UILabel(new CoreGraphics.CGRect(0, 100, 320, 20));
            label.Text = "Click Me";
            label.TextAlignment = UITextAlignment.Center;
            View.AddSubview(label);


            UITapGestureRecognizer labelTap = new UITapGestureRecognizer(() =>
                {
                    tooltip = new iiTooltip.CreateTooltip("Testing tooltip sample", this.View, image);
                    tooltip.Show(label.Frame);
                });
            label.UserInteractionEnabled = true;
            label.AddGestureRecognizer(labelTap);
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}

