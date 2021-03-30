using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.iOS.Custom
{
    [Register("NavigationMargin"), DesignTimeVisible(true)]
    public class NavigationMargin : TransparentView, IComponent
    {
        public nfloat MarginSize { get; private set; } = 10;
        public UINavigationBar Bar { get; set; } = null;

        NSLayoutConstraint SizeConstraints;
        Observation MyObservation;


        #region init
        public NavigationMargin()
        {
            Initialize();
        }

        public NavigationMargin(NSCoder coder) : base(coder)
		{
			Initialize();
        }

        public NavigationMargin(NSObjectFlag t) : base(t)
		{
			Initialize();
        }

        public NavigationMargin(IntPtr handle) : base(handle)
		{
			Initialize();
        }

        public NavigationMargin(CoreGraphics.CGRect frame) : base(frame)
		{
			Initialize();
        }
        #endregion

        void Initialize() 
        {
            SizeConstraints = NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, MarginSize);
			AddConstraint(SizeConstraints);
			SizeConstraints.Active = true;

			MyObservation = StatusBarSizeObservation.Register(() =>
			{
                SetNeedsUpdateConstraints();
			});
        }

        void UpdateMarginSize(CGRect statusBarFrame, bool statusBarVisible)
        {
            nfloat BarHeight    = 0;
            nfloat StatusHeight = (statusBarVisible) ? statusBarFrame.Height : 0;

            if (Bar == null)
                BarHeight = 0;
            else
                BarHeight = Bar.Bounds.Height;

			MarginSize = BarHeight + StatusHeight;
			Console.WriteLine("Margin Update TOTAl : " + MarginSize);
			Console.WriteLine("Margin Update STATUS: " + StatusHeight);
            Console.WriteLine("Margin Update NAVI  : " + BarHeight);
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();
            Console.WriteLine("Update Height");
            UpdateMarginSize(UIApplication.SharedApplication.StatusBarFrame, !UIApplication.SharedApplication.StatusBarHidden);
            SizeConstraints.Constant = MarginSize;
        }


        [Export("requiresConstraintBasedLayout")]
        public static bool RequiresConstraintBasedLayout()
        {
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                MyObservation.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
