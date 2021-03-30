using System;
namespace TokyoChokoku.Patmark.Common
{
    public class Scale2D
    {
        public double sx { get; }
        public double sy { get; }

		public double wRate { get { return sx; } }
		public double hRate { get { return sy; } }

        public Scale2D(double sx, double sy)
        {
            this.sx = sx;
            this.sy = sy;

        }
    }
}
