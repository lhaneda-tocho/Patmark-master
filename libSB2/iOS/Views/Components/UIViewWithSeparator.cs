using Foundation;
using System;
using UIKit;
using System.ComponentModel;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [Register ("UIViewWithSeparator")]
    [DesignTimeVisible (true)]
    public partial class UIViewWithSeparator : UIView
    {
        public enum SeparatorEmphasis
        {
            None   = 0,
            Top    = 1,
            Bottom = 2,
            Left   = 4,
            Right  = 8
        }

        [Export ("Emphasis"), Browsable (true)]
        public SeparatorEmphasis Emphasis {
            get; set;
        }

        [Export ("SeparatorWidth"), Browsable (true)]
        public nfloat SeparatorWidth {
            get; set;
        }

        [Export ("SeparatorColor"), Browsable (true)]
        public UIColor SeparatorColor {
            get; set;
        }

        public UIViewWithSeparator (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        override
        public void AwakeFromNib ()
        {
        }


        void Initialize ()
        {
            Emphasis = SeparatorEmphasis.Bottom;
            SeparatorWidth = 1.0f;
            SeparatorColor = UIColor.Blue;
        }


        public override void Draw (CGRect rect)
        {
            base.Draw (rect);
            using (var context = UIGraphics.GetCurrentContext ()) {
                context.SetLineWidth (SeparatorWidth);
                context.SetStrokeColor (SeparatorColor.CGColor);

                if (((int)Emphasis & (int)SeparatorEmphasis.Top) != 0) {
                    context.MoveTo (0, 0);
                    context.AddLineToPoint (Frame.Size.Width, 0);
                    context.StrokePath ();
                }
                if (((int)Emphasis & (int)SeparatorEmphasis.Bottom) != 0) {
                    context.MoveTo (0, Frame.Size.Height);
                    context.AddLineToPoint (Frame.Size.Width, Frame.Size.Height);
                    context.StrokePath ();
                }
                if (((int)Emphasis & (int)SeparatorEmphasis.Left) != 0) {
                    context.MoveTo (0, 0);
                    context.AddLineToPoint (0, Frame.Size.Height);
                    context.StrokePath ();
                }
                if (((int)Emphasis & (int)SeparatorEmphasis.Right) != 0) {
                    context.MoveTo (Frame.Size.Width, 0);
                    context.AddLineToPoint (Frame.Size.Width, Frame.Size.Height);
                    context.StrokePath ();
                }
            }
        }
    }
}