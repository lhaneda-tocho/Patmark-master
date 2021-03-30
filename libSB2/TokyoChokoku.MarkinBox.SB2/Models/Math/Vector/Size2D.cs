using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class Size2D
	{
		private Cartesian2D Cartesian { get; }

		public double X {
			get {
				return Cartesian.X;
			}
		}

		public double Y {
			get {
				return Cartesian.Y;
			}
		}

		public Size2D (double sx, double sy)
		{
			Cartesian = new Cartesian2D (sx, sy);
		}

		private Size2D (Cartesian2D scale)
		{
			Cartesian = scale;
		}

		public Size2D (double scale)
		{
			Cartesian = new Cartesian2D (scale, scale);
		}

		public static Size2D operator* (double left, Size2D right)
		{
			return new Size2D (left * right.Cartesian);
		}

		public static Size2D operator* (Size2D left, double right)
		{
			return new Size2D (left.Cartesian * right);
		}

		override
		public String ToString()
		{
			return 
				GetType ().FullName
				+ "( Sx = " + X
				+ ", Sy = " + Y + ")";
		}

        public Position2D ToPosition (Position2D start){
            return new Position2D (start.X + X, start.Y + Y);
        }
	}
}

