using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public sealed class Direction2D
	{
        public static Direction2D Zero { get; } = new Direction2D (0, 0);

		public Homogeneous2D Homogeneous { get; }
		public Cartesian2D Cartesian { get; }

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

		public Direction2D (double x, double y)
		{
			Homogeneous = new Homogeneous2D (x, y, 0);
			Cartesian = new Cartesian2D (x, y);
		}

		public Direction2D (Homogeneous2D dir)
		{
			Homogeneous = dir;
			Cartesian = new Cartesian2D (dir.X, dir.Y);
		}

		public Direction2D (Cartesian2D dir)
		{
			Homogeneous = new Homogeneous2D (dir.X, dir.Y);
			Cartesian = dir;
		}


		public Position2D ToPosition(double entityScale) {
			return new Position2D (entityScale * X, entityScale * Y);
		}


		public static Direction2D operator+ (Direction2D left, Direction2D right)
		{
			var cartdir = left.Cartesian + right.Cartesian;
			return new Direction2D (cartdir);
		}

		public static Direction2D operator- (Direction2D left, Direction2D right)
		{
			var cartdir = left.Cartesian - right.Cartesian;
			return new Direction2D (cartdir);
		}

		public static double operator* (Direction2D left, Direction2D right)
		{
			var dot = left.Cartesian * right.Cartesian;
			return dot;
		}

		public static Direction2D operator* (double leftScale, Direction2D right)
		{
			var scaled = leftScale * right.Cartesian;
			return new Direction2D(scaled);
		}

		public static Direction2D operator* (Direction2D left, double rightScale)
		{
			var scaled = left.Cartesian * rightScale;
			return new Direction2D(scaled);
		}

		public static Direction2D operator* (AffineMatrix2D affine, Direction2D left)
		{
			var ans = affine * left.Homogeneous;
			return new Direction2D (ans);
		}




		override
		public String ToString()
		{
			return 
				GetType ().FullName +
				"(x = " + X +
				", y = " + Y + ")";
		}
	}
}

