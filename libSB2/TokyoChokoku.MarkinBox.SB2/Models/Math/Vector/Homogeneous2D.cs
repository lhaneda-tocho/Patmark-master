using System;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class Homogeneous2D
	{
		public Vector<double> Elements { get; }

		public double X{
			get{
				return Elements.At (0);
			}
		}

		public double Y{
			get{
				return Elements.At (1);
			}
		}

		public double W{
			get{
				return Elements.At (2);
			}
		}

		public double CartesianX{
			get{
				return X / W;
			}
		}

		public double CartesianY{
			get{
				return Y / W;
			}
		}

		private Homogeneous2D(Vector<double> raw)
		{
			this.Elements = raw;
		}

		public Homogeneous2D (double x, double y, double w)
		{
			this.Elements = Vector<double>.Build.DenseOfArray (
				new double[] { x, y, w }
			);
		}

		public Homogeneous2D (double x, double y) : this (x, y, 1) {

		}


		public Homogeneous2D NormalizeW ()
		{
			return new Homogeneous2D ( CartesianX, CartesianY, 1 );
		}


		public Homogeneous2D Translate(Homogeneous2D to)
		{
			return Translate (to.CartesianX, to.CartesianY);
		}

		public Homogeneous2D Translate(double x, double y)
		{
			var trans = AffineMatrix2D.NewTranslation (x, y);
			var ans = trans.Elements * Elements;
			return new Homogeneous2D (ans);
		}

		public Homogeneous2D Rotate(double degrees)
		{
			var rot = AffineMatrix2D.NewRotation (degrees);
			var ans = rot.Elements * Elements;
			return new Homogeneous2D (ans);
		}

		public Homogeneous2D Scale(double sx, double sy)
		{
			var scale = AffineMatrix2D.NewScaling (sx, sy);
			var ans = scale.Elements * Elements;
			return new Homogeneous2D (ans);
		}


		public Cartesian2D ToCartesian()
		{
			return new Cartesian2D (CartesianX, CartesianY);
		}



		public static Homogeneous2D operator+ (Homogeneous2D left, Homogeneous2D right)
		{
			var ans = left.Elements + right.Elements;
			return new Homogeneous2D (ans);
		}


		public static Homogeneous2D operator- (Homogeneous2D left, Homogeneous2D right)
		{
			var ans = left.Elements - right.Elements;
			return new Homogeneous2D (ans);
		}


		public static double operator* (Homogeneous2D left, Homogeneous2D right)
		{
			return left.Elements * right.Elements;
		}


		public static Homogeneous2D operator* (AffineMatrix2D left, Homogeneous2D right)
		{
			var ans = left.Elements * right.Elements;
			return new Homogeneous2D (ans);
		}


		override
		public String ToString()
		{
			return 
				GetType ().FullName +
				"(x = " + X +
				", y = " + Y +
				", w = " + W +  ")";
		}


	}
}

