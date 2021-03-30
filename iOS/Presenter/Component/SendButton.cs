using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using TokyoChokoku.iOS.Custom;

namespace TokyoChokoku.Patmark.iOS.Presenter.Component
{
    [Register("SendButton"), DesignTimeVisible(true)]
    public class SendButton: FilledButton
    {
        public SendButton(UIButtonType type) : base(type)
		{
			InitSendButton();
        }

        public SendButton()
		{
			InitSendButton();
        }

        public SendButton(Foundation.NSCoder coder) : base(coder)
		{
			InitSendButton();
        }

        public SendButton(IntPtr handle) : base(handle)
		{
			InitSendButton();
        }

        public SendButton(CoreGraphics.CGRect frame) : base(frame)
        {
            InitSendButton();
        }

        void InitSendButton()
        {
            var image = UIImage.FromBundle("Send");
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
