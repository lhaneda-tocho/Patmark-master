using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Value;


// FIXME: Tick数の計算の修正
namespace TokyoChokoku.Patmark.Presenter.Ruler
{
    public class CommonRulerView: PhantomView
    {
        public CommonRulerView(IViewProperties prop): base(prop) {}

        public IRulerViewContent Content { get; set; } = new EmptyRulerContent();

        public RulerInfo Horizontal
        {
            get
            {
                var mode = RulerMode.Horizontal;
                var info = new RulerInfo(mode, Content.Frame);
                info.Main = Content.Horizontal;
                info.Sub  = Content.HorizontalSub;
                return info;
            }
        }

		public RulerInfo Vertical
		{
			get
			{
                var mode = RulerMode.Vertical;
				var info = new RulerInfo(mode, Content.Frame);
				info.Main = Content.Vertical;
				info.Sub  = Content.VerticalSub;
				return info;
			}
		}

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
			DrawHorizontal(canvas);
            DrawVeritical(canvas);

            //Console.WriteLine(Content.Frame);
        }

        void DrawHorizontal(Canvas canvas)
        {
			var info = Horizontal;
			{
				var ticks = info.MainTicks;
				ShowHorizontalLabel(canvas, ticks, 10, 3);
				var lines = HorizontalToLine(ticks, 10);
				canvas.StrokeLines(lines);
			}
			{
				var lines = HorizontalToLine(info.SubTicks,   4);
				canvas.StrokeLines(lines);
			}
        }

        void ShowHorizontalLabel(Canvas canvas, RulerTicks ticks, double tickSizePt, double margin = 3)
        {
            var fsize = 12;
            var font  = FontFactory.CreateArialWithPoint(fsize);

            var bottom = Content.Frame.Y - tickSizePt - margin;
            var top = bottom - fsize;

            var left = Content.Frame.Left;
            var right = Content.Frame.Right;
            for (int i = 0; i < ticks.Count; ++i)
            {
                var tick = ticks[i];
				var label = tick.Graduation.ToString();
				var x = tick.Position;
				var s = font.TextSize(label);
				canvas.Use(() =>
				{
					canvas.Translate(x, top);
					StyledText.CreateAlignRight(label, font).Stroke(canvas);
					//if (i == ticks.Count -1)
					//	StyledText.CreateAlignRight(label, font).Stroke(canvas);
					//else
						//StyledText.CreateAlignCenter(label, font).Stroke(canvas);
				});
            }
        }

        void DrawVeritical(Canvas canvas)
        {
			var info = Vertical;
			{
                var ticks = info.MainTicks;
                ShowVerticalLabel(canvas, ticks, 10, 3);
				var lines = VerticalToLine(ticks, 10);
				canvas.StrokeLines(lines);
			}
			{
                var lines = VerticalToLine(info.SubTicks ,  4);
				canvas.StrokeLines(lines);
			}
        }

		void ShowVerticalLabel(Canvas canvas, RulerTicks ticks, double tickSizePt, double margin = 3)
		{
			var fsize = 12;
			var font = FontFactory.CreateArialWithPoint(fsize);

            var right = Content.Frame.X - tickSizePt - margin;
			var left = right - tickSizePt;
            var top = Content.Frame.Top;
            var bottom = Content.Frame.Bottom;

			for (int i = 0; i < ticks.Count; ++i)
			{
				var tick = ticks[i];
				var label = tick.Graduation.ToString();
				var x = tick.Position;
				var s = font.TextSize(label);
				canvas.Use(() =>
				{
                    canvas.Translate(right, x - s.H);//s.H);
                    StyledText.CreateAlignRight(label, font).Stroke(canvas);
				});
			}
		}

        Lines HorizontalToLine(RulerTicks ticks, double sizePt) 
        {
			var lines = new Line[ticks.Count];
			var bot = Content.Frame.Y;
            var top = bot - sizePt;
            for (int i = 0; i < ticks.Count; i++)
            {
                var t = ticks[i].Graduation;
                var p = ticks[i].Position;
                lines[i] = new Line(
                    p, top,
                    p, bot
                );
            }

            return new Lines(lines);
        }

		Lines VerticalToLine(RulerTicks ticks, double sizePt)
		{
			var lines = new Line[ticks.Count];
			var right = Content.Frame.X;
			var left  = right - sizePt;
			for (int i = 0; i < ticks.Count; i++)
			{
				var t = ticks[i].Graduation;
				var p = ticks[i].Position;
				lines[i] = new Line(
                    left , p,
					right, p
				);
			}

			return new Lines(lines);
		}
    }
}
