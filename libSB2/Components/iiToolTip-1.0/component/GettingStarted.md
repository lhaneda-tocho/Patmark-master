## Getting Started with iiToolTip ##

**iiToolTip** 

A lightweight and easy to use tooltip component that helps users be more informed about your app screens. Simply add the control onto your solution, pass 3 simple parameters and see the results instantly.



	using iiTooltip;

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
	}
