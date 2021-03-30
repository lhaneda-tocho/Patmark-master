using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.RenderKit.Value;
namespace TokyoChokoku.Patmark.Presenter.Ruler
{
    public class RulerInfo
    {
        public RulerMode   Mode   { get; }
        public Frame2D     Frame  { get; }

        // arg1 : X
        // arg2 : W
        public TickSource Main { get; set; } = DefaultTick();
        public TickSource Sub  { get; set; } = DefaultTick();

        public RulerInfo(RulerMode mode, Frame2D frame)
        {
            Mode  = mode;
            Frame = frame;
        }

        public RulerTicks MainTicks 
        {
            get {
                return Tick(Main);
            }
        }

        public RulerTicks SubTicks
        {
            get {
                return Tick(Sub);
            }
        }

        RulerTicks Tick(TickSource f)
        {
			return Mode.Match(
                horizontal: (it) =>
                {
					return f(Frame.X, Frame.W).Tick();
				},
				vertical: (it) =>
				{
					return f(Frame.Y, Frame.H).Tick();
				}
            );
        }

        static TickSource DefaultTick()
        {
            return (x, w) =>
            {
                return TickInfo.Init(x,w);
            };
        }
    }

    public enum RulerMode
    {
        Horizontal, Vertical
    }

    public static class RulerModeExt 
    {
        public static R Match<R>(this RulerMode it,
            Func<RulerMode, R> horizontal,
            Func<RulerMode, R> vertical
        ) {
            switch(it) {
                case RulerMode.Horizontal: return horizontal(it);
                case RulerMode.Vertical  : return vertical(it);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }


}
