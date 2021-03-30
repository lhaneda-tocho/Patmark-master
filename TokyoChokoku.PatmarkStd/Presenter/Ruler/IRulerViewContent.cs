using System;
using TokyoChokoku.Patmark.RenderKit.Value;
namespace TokyoChokoku.Patmark.Presenter.Ruler
{
    public delegate TickInfo TickSource(double x, double w);

    public interface IRulerViewContent
    {
        TickSource Horizontal    { get; }
        TickSource HorizontalSub { get; }
        TickSource Vertical      { get; }
        TickSource VerticalSub   { get; }
        Frame2D Frame { get; }
    }

	public class EmptyRulerContent: IRulerViewContent
	{
        TickSource v = (x, w) => TickInfo.Init(x, w);
		public TickSource Horizontal {
            get { return v; }
		}
		public TickSource HorizontalSub
		{
            get { return v; }
		}
		public TickSource Vertical { 
            get { return v; }
        }
        public TickSource VerticalSub {
            get { return v; }
        }
        public Frame2D Frame 
        {
            get { return Frame2D.One(Pos2D.Zero()); }
        }
	}

    public abstract class RulerViewContentAdapter: IRulerViewContent
	{
		public TickSource Horizontal
		{
			get
			{
				return (x, w) =>
				{
					var info = TickInfo.Init(x, w);
					info.XTick = this.StartTick.X;
					info.WTick = this.TickRange.SX;
					info.Interval = Interval;
					return info;
				};
			}
		}

		public TickSource HorizontalSub
		{
			get
			{
				return (x, w) =>
				{
					var info = TickInfo.Init(x, w);
					info.XTick = this.StartTick.X;
					info.WTick = this.TickRange.SX;
					info.Interval = IntervalSub;
					return info;
				};
			}
		}

		public TickSource Vertical
		{
			get
			{
				return (y, h) =>
				{
					var info = TickInfo.Init(y, h);
					info.XTick = this.StartTick.Y;
					info.WTick = this.TickRange.SY;
					info.Interval = Interval;
					return info;
				};
			}
		}


        public TickSource VerticalSub
        {
            get
            {
                return (y, h) =>
                {
                    var info = TickInfo.Init(y, h);
                    info.XTick = this.StartTick.Y;
                    info.WTick = this.TickRange.SY;
                    info.Interval = IntervalSub;
                    return info;
                };
            }
        }

        public abstract Pos2D   StartTick  { get; }
		public abstract Size2D TickRange   { get; }
		public abstract double Interval    { get; }
		public abstract double IntervalSub { get; }
        public abstract Frame2D Frame      { get; }
    }
}
