using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Value;
namespace TokyoChokoku.Patmark.Presenter
{
    public class PhantomView
    {
		public PhantomView(IViewProperties prop)
		{
			Properties = prop;
		}


		public IViewProperties Properties { get; }

		public Frame2D Bounds { get { return Properties.Bounds; } }
        public Frame2D Frame  { get { return Properties.Frame; } }



        public virtual void Draw(Canvas canvas)
		{}
	}
}
