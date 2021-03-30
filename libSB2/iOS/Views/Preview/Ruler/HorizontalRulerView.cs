using Foundation;
using System;
using System.ComponentModel;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [DesignTimeVisible (true)]
    public partial class HorizontalRulerView : UIViewWithSeparator
    {
        float gridSize = 6;
        float textPosition = 18;
        float textSize = 12;

        public float GridSize {
            get {
                return gridSize;
            }
            set {
                if (value < 0)
                    gridSize = 0;
                else
                    gridSize = value;
            }
        }

        RulerInfo rulerInfo;
        public RulerInfo RulerInfo {
            get {
                return rulerInfo;
            }
            set {
                rulerInfo = value;
                SetNeedsDisplay ();
            }
        }


        double Start {
            get {
                return RulerInfo.Start;
            }
        }

        double End {
            get {
                return RulerInfo.End;
            }
        }

        double Scale {
            get {
                return RulerInfo.GridScale;
            }
        }

        public HorizontalRulerView (IntPtr handle) : base (handle)
        {
            Emphasis = SeparatorEmphasis.Bottom;
            RulerInfo = new RulerInfo (0, 10);
        }


        public override void Draw (CGRect rect)
        {
            base.Draw (rect);

            using (var context = UIGraphics.GetCurrentContext ()) {
                context.SaveState ();

                context.SetLineWidth (1.0f);
                context.SetStrokeColor (UIColor.DarkGray.CGColor);

                Grid (context);
                Number (context);

                context.RestoreState ();
            }
        }

        void Grid (CGContext context)
        {
            var bottom = Bounds.Height;
            var top = bottom - GridSize;
            var right = Bounds.Right;

            foreach (double grid in RulerInfo) {
                var x = (nfloat) RulerInfo.GridToView (grid, 0, right);

                context.MoveTo (x, bottom);
                context.AddLineToPoint (x, top);
                context.StrokePath ();
            }
        }

        void Number (CGContext context)
        {
            foreach (double grid in RulerInfo) {
                if (RulerInfo.IsStartInView (grid, 5, Bounds.Left, Bounds.Right)) {
                    DrawGridLabelStartSide((nfloat) grid);
                    continue;
                }
                if (RulerInfo.IsEndInView (grid, 5, Bounds.Left, Bounds.Right)) {
                    DrawGridLabelEndSide ((nfloat)grid); 
                    continue;
                }
                DrawGridLabel ((nfloat) grid, (nfloat) RulerInfo.GridToView (grid, 0, Bounds.Width));
            }
        }

        NSAttributedString GridLabel (string str)
        {
            var font = UIFont.FromName ("Helvetica", textSize);

            var style = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy ();
            style.Alignment = UITextAlignment.Center;

            return new NSAttributedString (str, new UIStringAttributes {
                Font = font,
                ParagraphStyle = style
            });
        }

        void DrawGridLabel (nfloat grid, nfloat centerX)
        {
            var str = grid.ToString ();
            using (var ns = GridLabel (str)) {
                var size = ns.Size;
                var sx = centerX - size.Width / 2;
                var sy = Bounds.Bottom - textPosition - size.Height / 2;
                var rect = new CGRect (new CGPoint (sx, sy), size);
                ns.DrawString (rect);
            }
        }

        void DrawGridLabelStartSide (nfloat grid)
        {
            var str = grid.ToString ();
            using (var ns = GridLabel (str)) {
                var size = ns.Size;
                var sy = Bounds.Bottom - textPosition - size.Height / 2;
                var rect = new CGRect (new CGPoint (0, sy), size);
                ns.DrawString (rect);
            }
        }

        void DrawGridLabelEndSide (nfloat grid)
        {
            var str = grid.ToString ();
            using (var ns = GridLabel (str)) {
                var size = ns.Size;
                var sx = Bounds.Width - size.Width;
                var sy = Bounds.Bottom - textPosition - size.Height / 2;
                var rect = new CGRect (new CGPoint (sx, sy), size);
                ns.DrawString (rect);
            }
        }
    }
}