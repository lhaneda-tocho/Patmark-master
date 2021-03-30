using Foundation;
using System;
using UIKit;
using System.ComponentModel;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{

    [DesignTimeVisible(true)]
    public partial class CellFrameView : UIView
    {
        string title = "Title";

        [Export ("Title"), Browsable (true)]
        public string Title {
            get {
                return title;
            }
            set {
                if (value == null)
                    value = "";
                title = value;
            }
        }

        nfloat fontSize = 17;
        string fontFamily = "Helvetica";

        [Export ("CornerRadius"), Browsable (true)]
        public nfloat CornerRadius {
            get; set;
        } = 2;

        [Export ("BorderWidth"), Browsable (true)]
        public nfloat BorderWidth {
            get; set;
        } = 1;

        [Export ("TextColor"), Browsable (true)]
        public UIColor TextColor {
            get; set;
        } = UIColor.DarkGray;

        [Export ("BorderColor"), Browsable (true)]
        public UIColor BorderColor {
            get; set;
        } = UIColor.DarkGray;

        [Export ("TitleHeader"), Browsable (true)]
        public bool TitleHeader {
            get; set;
        } = true;

        [Export ("RemoveTitleHeader"), Browsable (true)]
        public bool RemoveTitleHeader {
            get; set;
        } = false;

        CGRect TextFrame (CGSize textSize)
        {
            if (TitleHeader) {
                return new CGRect (new CGPoint (35, 0), textSize);
            }
            return new CGRect (
                new CGPoint (35, Bounds.Height / 2 - textSize.Height / 2), textSize);
        }

        CGRect BorderFrame {
            get {
                return new CGRect (
                    Bounds.X + 3,
                    Bounds.Y + 3,
                    Bounds.Width  - 6,
                    Bounds.Height - 6
                );
            }
        }

        NSMutableParagraphStyle paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy ();

        NSAttributedString TitleAttributedString {
            get {
                return new NSAttributedString (Title, new UIStringAttributes {
                    Font = UIFont.FromName (fontFamily, fontSize),
                    ParagraphStyle = paragraphStyle,
                    StrokeColor = TextColor,
                    ForegroundColor = TextColor
                });
            }
        }



        public CellFrameView (IntPtr handle) : base (handle)
        {
        }

        [Export ("initWithCoder:")]
        public CellFrameView (NSCoder coder) : base (coder)
        {
            initCellFrame ();
        }

        [Export ("initWithFrame:")]
        public CellFrameView (CGRect rect) : base (rect)
        {
            initCellFrame ();
        }

        public void initCellFrame ()
        {
            paragraphStyle.Alignment = UITextAlignment.Center;
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            SetNeedsDisplay ();
        }

        public override void Draw (CGRect rect)
        {
            base.Draw (rect);

            var border = BorderFrame;

            using (UIBezierPath path = UIBezierPath.FromRoundedRect (border, CornerRadius)) {
                path.LineWidth = BorderWidth;
                BorderColor.SetStroke ();
                path.Stroke ();
            }

            if (RemoveTitleHeader)
                return;
            
            NSAttributedString str = TitleAttributedString;
            var size = str.Size;
            var labelFrame = TextFrame (size);
            DrawLabelBg (str, labelFrame);
            str.DrawString (labelFrame);

        }

        public override bool DrawViewHierarchy (CGRect rect, bool afterScreenUpdates)
        {
            return base.DrawViewHierarchy (rect, afterScreenUpdates);
        }

        void DrawLabelBg (NSAttributedString str, CGRect frame)
        {
            nfloat x = frame.X - 5, 
                   y = frame.Y,
                   w = frame.Width + 10,
                   h = frame.Height;
            var labelBgFrame = new CGRect (x, y, w, h);

            using (CGContext context = UIGraphics.GetCurrentContext ()) {
                context.SaveState ();
                UIColor.White.SetFill ();
                context.FillRect (labelBgFrame);
                context.RestoreState ();
            }
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing) {
                paragraphStyle.Dispose ();
            }
            base.Dispose (disposing);
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
    }
}