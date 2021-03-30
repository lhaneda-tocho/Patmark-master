using System;
using TokyoChokoku.Patmark.Presenter.Ruler;
using TokyoChokoku.Patmark.RenderKit.Value;
namespace TokyoChokoku.Patmark.Presenter.Preview
{
    public class PreviewRulerContent: RulerViewContentAdapter
	{
		public PreviewRulerContent(CommonPreView view)
		{
			View = view;
		}

        CommonPreView View { get; }

        public override Pos2D StartTick
        {
            get
            {
                return Pos2D.Init(0, 0);
            }
        }

        public override Size2D TickRange
        {
            get
            {
                return Size2D.Init(View.Area.Widthmm, View.Area.Heightmm);
            }
        }

        public override Frame2D Frame
        {
            get
            {
                var f = View.Frame;
                return f;
            }
		}

		public override double Interval
		{
			get
			{
				return 5;
			}
		}

		public override double IntervalSub
		{
			get
			{
				return 1;
			}
		}
    }
}
