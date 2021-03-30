using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace TokyoChokoku.iOS.Custom
{
    [Register("TransparentView"), DesignTimeVisible(true)]
    public class TransparentView: UIView, IComponent
    {
        #region constructor extension
        public TransparentView()
        {
            InitTransparentView();
        }

        public TransparentView(Foundation.NSCoder coder) : base(coder)
        {
            InitTransparentView();
        }

        public TransparentView(Foundation.NSObjectFlag t) : base(t)
        {
            InitTransparentView();
        }

        public TransparentView(IntPtr handle) : base(handle)
        {
            InitTransparentView();
        }

        public TransparentView(CoreGraphics.CGRect frame) : base(frame)
        {
            InitTransparentView();
        }

        public void InitTransparentView() {
            BackgroundColor = UIColor.Clear;
        }
        #endregion

        #region IComponent implementation
        public ISite Site { get; set; }
        public event EventHandler Disposed;
        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        /// <summary>
        /// 自身を タッチできないようにする．
        /// </summary>
        /// <returns>The test.</returns>
        /// <param name="point">Point.</param>
        /// <param name="uievent">Uievent.</param>
        public override UIView HitTest(CoreGraphics.CGPoint point, UIEvent uievent)
		{
			UIView view = base.HitTest(point, uievent);
			if (view == this)
				return null;
			else
				return view;
		}

        public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);
			if (Site != null && Site.DesignMode)
			{
                var style = NSParagraphStyle.Default.MutableCopy() as NSMutableParagraphStyle;
                style.Alignment = UITextAlignment.Center;
				var attr = new UIStringAttributes
				{
                    ForegroundColor = UIColor.Gray,
					Font = UIFont.SystemFontOfSize(20),
                    ParagraphStyle = style
				};
				var str = new NSString("Transparent View");
                str.WeakDrawString(Bounds, attr.Dictionary);
            }
    //        else {
				//var style = NSParagraphStyle.Default.MutableCopy() as NSMutableParagraphStyle;
				//style.Alignment = UITextAlignment.Center;
				//var attr = new UIStringAttributes
				//{
				//	ForegroundColor = UIColor.Gray,
				//	Font = UIFont.SystemFontOfSize(20),
				//	ParagraphStyle = style
				//};
				//var str = new NSString("Transparent View");
				//str.WeakDrawString(Bounds, attr.Dictionary);
            //}
        }
    }
}
