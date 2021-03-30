using System;
using MathNet.Numerics.LinearAlgebra;

namespace sketchbook_touch
{
	public class VectorFactory
	{
		private VectorFactory()
		{
		}

		public static Vector<double> Point2D (double x, double y)
		{
			return Vector<double>.Build.DenseOfArray(new double[]{ x, y, 1 });
		}

	}
}

