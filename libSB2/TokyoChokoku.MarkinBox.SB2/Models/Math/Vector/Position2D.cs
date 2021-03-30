using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public sealed class Position2D
	{
        public static Position2D Zero { get; } 
            = new Position2D ( FamousVectors.Zero.OfHomogeneous2D, FamousVectors.Zero.OfCartesian2D );

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

		public Position2D (double x, double y)
		{
			Homogeneous = new Homogeneous2D (x, y);
			Cartesian = new Cartesian2D (x, y);
		}

		public Position2D (Homogeneous2D pos)
		{
			Homogeneous = pos;
			Cartesian = pos.ToCartesian ();
		}

		public Position2D (Cartesian2D pos)
		{
			Homogeneous = pos.ToHomogeneous ();
			Cartesian = pos;
		}
         
        private Position2D (Homogeneous2D posH, Cartesian2D posC) {
            Homogeneous = posH;
            Cartesian   = posC;
        }


		public static Position2D operator+ (Position2D left, Position2D right)
		{
			var cartpos = left.Cartesian + right.Cartesian;
			return new Position2D (cartpos);
		}

		public static Position2D operator- (Position2D left, Position2D right)
		{
			var cartpos = left.Cartesian - right.Cartesian;
			return new Position2D (cartpos);
		}


		public static double operator* (Position2D left, Position2D right)
		{
			var dot = left.Cartesian * right.Cartesian;
			return dot;
		}

		public static Position2D operator* (double leftScalar, Position2D right)
		{
			var scaled = leftScalar * right.Cartesian;
			return new Position2D(scaled);
		}

		public static Position2D operator* (Position2D left, double rightScalar)
		{
			var scaled = left.Cartesian * rightScalar;
			return new Position2D(scaled);
		}

        public static Position2D operator/ (Position2D left, double rightScalar)
        {
            var scaled = left.Cartesian / rightScalar;
            return new Position2D(scaled);
        }

		public static Position2D operator* (AffineMatrix2D affine, Position2D left)
		{
			var ans = affine * left.Homogeneous;
			return new Position2D (ans);
		}

        public static Position2D operator- (Position2D me) {
            return new Position2D (-me.Cartesian);
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

