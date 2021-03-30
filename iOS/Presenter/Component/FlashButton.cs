using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using TokyoChokoku.iOS.Custom;

namespace TokyoChokoku.Patmark.iOS.Presenter.Component
{
    [Register("FlashButton"), DesignTimeVisible(true)]
    public class FlashButton: FilledButton
    {
        public FlashButton(UIButtonType type) : base(type)
		{
			InitFlashButton();
        }

        public FlashButton()
		{
			InitFlashButton();
        }

        public FlashButton(Foundation.NSCoder coder) : base(coder)
		{
			InitFlashButton();
        }

        public FlashButton(IntPtr handle) : base(handle)
		{
			InitFlashButton();
        }

        public FlashButton(CoreGraphics.CGRect frame) : base(frame)
        {
            InitFlashButton();
        }

        void InitFlashButton()
        {
			var image = this.CurrentImage;
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            UIEdgeInsets insets;
            insets.Top = insets.Bottom = insets.Left = insets.Right = 4;
			ContentEdgeInsets = insets;
			SetImage(image, UIControlState.Normal);
            SetImage(image, UIControlState.Selected);
            SetImage(image, UIControlState.Highlighted);
            SetImage(image, UIControlState.Disabled);
        }
    }
}
