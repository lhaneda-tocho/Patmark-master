using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [DesignTimeVisible (true)]
    public partial class UISpringView : UIView
    {
        public enum SquashDirection {
            Up, Down, Leading, Trailing
        }

        [Export ("CornerRadius"), Browsable (true)]
        public nfloat CornerRadius {
            get {
                return Layer.CornerRadius;
            }
            set {
                if (value < 0)
                    value = 0;
                Layer.CornerRadius = value;
                SetNeedsDisplay ();
            }
        }

        [Export ("BorderColor"), Browsable (true)]
        public UIColor BorderColor {
            get {
                return new UIColor (Layer.BorderColor);
            }
            set {
                if (value == null)
                    value = UIColor.Blue;
                Layer.BorderColor = value.CGColor;
                SetNeedsDisplay ();
            }
        }

        [Export ("BorderWidth"), Browsable (true)]
        public nfloat BorderWidth {
            get {
                return Layer.BorderWidth;
            }
            set {
                if (value < 0)
                    value = 0;
                Layer.BorderWidth = value;
                SetNeedsDisplay ();
            }
        }


        public UISpringView (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        override
        public void AwakeFromNib ()
        {
        }


        void Initialize ()
        {
            BorderColor = UIColor.Blue;
            BorderWidth = 0.5f;
            CornerRadius = 2.0f;
        }

        public void Squash ()
        {

        }

        public void Expand ()
        {

        }
    }
}