using System;
using System.ComponentModel;
using UIKit;
using Foundation;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [Register ("BorderButton"), DesignTimeVisible (true)]
    public class BorderButton : UIButton
    {
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

        [Export ("ButtonColor"), Browsable (true)]
        public UIColor ButtonColor {
            get; set;
        }

        [Export ("HighlightColor"), Browsable (true)]
        public UIColor HighlightColor {
            get; set;
        }

        [Export ("DisabledColor"), Browsable (true)]
        public UIColor DisabledColor {
            get; set;
        }

        public override bool Enabled {
            get {
                return base.Enabled;
            }
            set {
                base.Enabled = value;
                if (value)
                    EnabledButton ();
                else
                    DisabledButton ();
            }
        }

        public BorderButton (UIButtonType type) : base (type)
        {
            Initialize ();
        }

        public BorderButton ()
        {
            Initialize ();
        }

        public BorderButton (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        public BorderButton (NSObjectFlag t) : base (t)
        {
            Initialize ();
        }

        public BorderButton (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        public BorderButton (CGRect frame) : base (frame)
        {
            Initialize ();
        }

        /// <summary>
        /// Storyboardの値を設定し終えた直後に呼び出されます．
        /// </summary>
        public override void AwakeFromNib ()
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            if (ButtonColor != null)
                BackgroundColor = ButtonColor;
            else
                ButtonColor = BackgroundColor;
        }

        /// <summary>
        /// インスタンス化された直後に呼び出されます
        /// </summary>
        void Initialize ()
        {
            BorderColor     = UIColor.Blue;
            ButtonColor     = null;
            DisabledColor   = null;
            HighlightColor  = null;
            BorderWidth  = 0.5f;
            CornerRadius = 2.0f;
        }

        public override bool Highlighted {
            get {
                return base.Highlighted;
            }
            set {
                base.Highlighted = value;
                if (Highlighted)
                    HighlightBackground ();
                else
                    UnhighlightBackground ();
            }
        }

        void HighlightBackground ()
        {
            if (HighlightColor != null) {
                BeginAnimations ("BorderButtonHighlight");
                BackgroundColor = HighlightColor;
                CommitAnimations ();
            }
        }

        void UnhighlightBackground ()
        {
            if (HighlightColor != null) {
                BeginAnimations ("BorderButtonHighlight");
                BackgroundColor = ButtonColor;
                CommitAnimations ();
            }
        }

        void EnabledButton ()
        {
            if (ButtonColor != null) {
                BeginAnimations ("BorderButtonActivation");
                BackgroundColor = ButtonColor;
                CommitAnimations ();
            }
        }

        void DisabledButton ()
        {
            if (DisabledColor != null) {
                BeginAnimations ("BorderButtonActivation");
                BackgroundColor = DisabledColor;
                CommitAnimations ();
            }
        }
    }
}

