using System;
using System.ComponentModel;
using Foundation;
using CoreGraphics;
using UIKit;

namespace TokyoChokoku.iOS.Custom
{
	[Register("BorderView"), DesignTimeVisible(true)]
    public class BorderView: UIView
    {
		#region Design Properties
		// 角丸の半径(0で四角形)
		[Export("CornerRadius"), Browsable(true)]
		public nfloat CornerRadius
		{
			get { return Layer.CornerRadius; }
			set
			{
				Layer.CornerRadius = (value > 0) ? value : 0;
				ClipsToBounds = (value > 0);
				SetNeedsDisplay();
			}
		}

		// 枠
		[Export("BorderColor"), Browsable(true)]
		public UIColor BorderColor
		{
			get { return new UIColor(Layer.BorderColor); }
			set
			{
				Layer.BorderColor = (value != null) ? value.CGColor : UIColor.Clear.CGColor;
				SetNeedsDisplay();
			}
		}

		[Export("BorderWidth"), Browsable(true)]
		public nfloat BorderWidth
		{
			get { return Layer.BorderWidth; }
			set
			{
				Layer.BorderWidth = (value > 0) ? value : 0;
				SetNeedsDisplay();
			}
		}
        #endregion

		#region constructor extension
        public BorderView()                              {Initialize();}
        public BorderView(NSCoder coder ) : base(coder)  {Initialize(); }
        public BorderView(NSObjectFlag t) : base(t)      {Initialize(); }
        public BorderView(IntPtr handle)  : base(handle) {Initialize(); }
        public BorderView(CGRect frame)   : base(frame)  {Initialize(); }

		// awake from nib
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}
		//

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		void Initialize()
		{
			CornerRadius = 0.0f;
			BorderWidth = 0.0f;
			BorderColor = UIColor.Clear;
		}
		#endregion


    }
}
