using System;
using MathNet.Numerics.LinearAlgebra;

namespace sketchbook_touch
{
	public class Point2D
	{
		public double X = 0d;
		public double Y = 0d;

		public Point2D (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Vector<double> ToVector()
		{
			return VectorFactory.Point2D (X, Y);
		}
	}
}

