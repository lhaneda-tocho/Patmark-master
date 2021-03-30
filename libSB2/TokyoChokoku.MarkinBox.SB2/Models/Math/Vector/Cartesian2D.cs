using System;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class Cartesian2D
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

		private Cartesian2D(Vector<double> raw)
		{
			this.Elements = raw;
		}

		public Cartesian2D (double x, double y) : this (
			Vector<double>.Build.DenseOfArray(
				new double[] { x, y }
			)
		) {
			
		}


		public Cartesian2D Normalize(double p)
		{
			var ans = Elements.Normalize (p);
			return new Cartesian2D (ans);
		}


		public double Norm(double p)
		{
			return Elements.Norm (p);
		}



		public Homogeneous2D ToHomogeneous()
		{
			return new Homogeneous2D (X, Y, 1);
		}



		public double OuterProduct(Cartesian2D right)
		{
			var left = this;

			var result = left.X * right.Y - left.Y * right.X;

			return result;
		}

		public static Cartesian2D operator+ (Cartesian2D left, Cartesian2D right)
		{
			var ans = left.Elements + right.Elements;
			return new Cartesian2D (ans);
		}


		public static Cartesian2D operator- (Cartesian2D left, Cartesian2D right)
		{
			var ans = left.Elements - right.Elements;
			return new Cartesian2D (ans);
		}


		public static double operator* (Cartesian2D left, Cartesian2D right)
		{
			return left.Elements * right.Elements;
		}



		public static Cartesian2D operator* (Cartesian2D left, double rightScalar)
		{
			var ans = left.Elements * rightScalar;
			return new Cartesian2D (ans);
		}

        public static Cartesian2D operator/ (Cartesian2D left, double rightScalar)
        {
            var ans = left.Elements / rightScalar;
            return new Cartesian2D (ans);
        }

		public static Cartesian2D operator* (double leftScalar, Cartesian2D right)
		{
			var ans = leftScalar * right.Elements;
			return new Cartesian2D (ans);
		}

        public static Cartesian2D operator- (Cartesian2D me) {
            return new Cartesian2D (-me.Elements);
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

