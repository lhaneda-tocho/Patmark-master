using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.UnitKit;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
    public class StyledText
    {
        public string     Text  { get; }
        public CommonFont Font  { get; }
        public ITextStyle  Style { get; }

        public Frame2D Bounds { get { return Font.TextBounds(Text); } }


        public static StyledText CreateAlignLeft(string text, CommonFont font)
        {
            return new StyledText(text, font, new MutableTextStyle());
		}

		public static StyledText CreateAlignCenter(string text, CommonFont font)
		{
			var style = new MutableTextStyle();
			style.HorizontalAlignment = HorizontalAlignment.Center;
			return new StyledText(text, font, style);
		}

		public static StyledText CreateAlignRight(string text, CommonFont font)
		{
			var style = new MutableTextStyle();
            style.HorizontalAlignment = HorizontalAlignment.Right;
			return new StyledText(text, font, style);
		}

        public StyledText(String text, CommonFont font, ITextStyle style)
        {
            Text = text;
            Font = font;
            Style = style;
        }

        public void Stroke(Canvas canvas)
        {
            canvas.Use(()=>
            {
                var f = Bounds;
                canvas.Translate(f.X, f.Y);
                switch(Style.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left  : StrokeLeft  (canvas, f); return;
                    case HorizontalAlignment.Center: StrokeCenter(canvas, f); return;
                    case HorizontalAlignment.Right : StrokeRight (canvas, f); return;
                    default: throw new ArgumentOutOfRangeException();
                }
            });
		}

        void StrokeLeft(Canvas canvas, Frame2D frame)
		{
            //canvas.FillLines(frame.ToStrip());
            canvas.ShowStringAt(Pos2D.Init(0, 0), Text, Size2D.Fill(Font.SizePt));
		}

		void StrokeCenter(Canvas canvas, Frame2D frame)
		{
			//canvas.FillLines(frame.ToStrip());
            canvas.ShowStringAt(Pos2D.Init(-frame.W / 2, 0), Text, Size2D.Fill(Font.SizePt));
		}

		void StrokeRight(Canvas canvas, Frame2D frame)
		{
			//canvas.FillLines(frame.ToStrip());
            canvas.ShowStringAt(Pos2D.Init(-frame.W, 0), Text, Size2D.Fill(Font.SizePt));
		}
    }

    #region Style
    public interface ITextStyle
    {
        HorizontalAlignment HorizontalAlignment { get; }

    }

    public class MutableTextStyle : ITextStyle
    {
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

        public MutableTextStyle Copy ()
        {
            var style = new MutableTextStyle();
            style.HorizontalAlignment = HorizontalAlignment;
            return style;
        }
    }

    public enum HorizontalAlignment
    {
        Left, Center, Right
    }
#endregion
} 
