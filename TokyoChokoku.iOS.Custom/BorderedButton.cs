using System;
using System.ComponentModel;
using Foundation;
using CoreGraphics;
using UIKit;

namespace TokyoChokoku.iOS.Custom
{
	[Register("BorderedButton"), DesignTimeVisible(true)]
    public class BorderedButton : UIButton //, IComponent
    {
        #region Design Properties
        // 角丸の半径(0で四角形)
        [Export("CornerRadius"), Browsable(true)]
        public nfloat CornerRadius
        { 
            get { return Layer.CornerRadius; }
            set {
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
            set {
				Layer.BorderColor = (value != null) ? value.CGColor : UIColor.Clear.CGColor;
                SetNeedsDisplay();
            }
        }

		[Export("BorderWidth"), Browsable(true)]
		public nfloat BorderWidth { 
            get { return Layer.BorderWidth; }
            set { 
                Layer.BorderWidth = (value > 0) ? value : 0; 
                SetNeedsDisplay();
            }
        }
		#endregion


		//#region IComponent implementation
		//public ISite Site { get; set; }
		//public event EventHandler Disposed;
		//#endregion


		#region constructor extension
		public BorderedButton(CoreGraphics.CGRect frame) : base(frame)  { Initialize(); }
		public BorderedButton(NSCoder coder)             : base(coder)  { Initialize(); }
		public BorderedButton(IntPtr handle)             : base(handle) { Initialize(); }

        public BorderedButton()                          : base()       { Initialize(); }
		public BorderedButton(UIButtonType type)         : base(type)   { Initialize(); }
		#endregion


		// awake from nib
		public override void AwakeFromNib()
		{
            base.AwakeFromNib();
		}
        //

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize() {
            CornerRadius = 0.0f;
            BorderWidth  = 0.0f;
            BorderColor  = UIColor.Clear;
		}
    }
}
