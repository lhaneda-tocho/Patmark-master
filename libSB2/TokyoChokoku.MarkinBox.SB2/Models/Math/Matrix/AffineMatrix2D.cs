using System;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class AffineMatrix2D
	{
		public Matrix<double> Elements { get; }


		public double ScaleX {
			get{ return Elements.At (0, 0); }
		}


		public double SkewX {
			get{ return Elements.At (0, 1); }
		}


		public double OriginX {
			get{ return Elements.At (0, 2); }
		}


		public double SkewY {
			get{ return Elements.At (1, 0); }
		}


		public double ScaleY {
			get{ return Elements.At (1, 1); }
		}


		public double OriginY {
			get{ return Elements.At (1, 2); }
		}




		public static AffineMatrix2D Entity { get; } = new AffineMatrix2D( 
			1, 0, 0,
			0, 1, 0
		);

		public static AffineMatrix2D NewTranslation(Position2D origin)
		{
			return new AffineMatrix2D (
				1, 0, origin.X,
				0, 1, origin.Y
			);
		}

		public static AffineMatrix2D NewTranslation(double x, double y)
		{
			return new AffineMatrix2D (
				1, 0, x,
				0, 1, y
			);
		}

		public static AffineMatrix2D NewRotation(double degrees)
		{
			var radians = UnitConverter.Degrees.ToRadians (degrees);
			var Sa = Math.Sin (radians);
			var Ca = Math.Cos (radians);

			return new AffineMatrix2D (
				Ca, -Sa, 0,
				Sa,  Ca, 0
			);
		}

		public static AffineMatrix2D NewScaling(Size2D scale)
		{
			return new AffineMatrix2D (
				scale.X,      0, 0,
				     0, scale.Y, 0
			);
		}

		public static AffineMatrix2D NewScaling(double sx, double sy)
		{
			return new AffineMatrix2D (
				sx, 0, 0,
				0, sy, 0
			);
		}

		public AffineMatrix2D (
			double scaleX, double  skewX, double originX,
			double  skewY, double scaleY, double originY)
		{
			Elements = Matrix<double>.Build.DenseOfArray(new double[,] {
				{ scaleX,  skewX, originX },
				{  skewY, scaleY, originY },
				{ 0, 0, 1 },
			});
		}


		private AffineMatrix2D (Matrix<double> raw)
		{
			Elements = raw;
		}


		public AffineMatrix2D Inverse()
		{
			var ans = Elements.Inverse ();
			return new AffineMatrix2D (ans);
		}


		public static AffineMatrix2D operator* (AffineMatrix2D left, AffineMatrix2D right)
		{
			var ans = left.Elements * right.Elements;
			return new AffineMatrix2D (ans);
		}


	}
}

