//using System;
//using MathNet.Numerics.LinearAlgebra;
//
//
//namespace sketchbook_touch
//{
//	public static class VectorUtil
//	{
//		public static double OuterProduct2D(Vector<double> left, Vector<double> right)
//		{
//			if (left.Count != 2 || right.Count != 2) {
//				String mes = "Vectors must have a length of 2.";
//				throw new ArgumentException ();
//			}
//
//			var result = left.At (0) * right.At (1) - left.At (1) * right.At (0);
//
//			return result;
//		}
//
//		public static Vector<double> ToHomogeneous(Vector<double> cartesian)
//		{
//			var elm = new double[cartesian.Count + 1];
//
//			for (int i = 0; i < elm.Length; i++) {
//				elm [i] = cartesian.At (i);
//			}
//
//			elm [elm.Length - 1] = 1;
//
//			return Vector<double>.Build.DenseOfArray(elm);
//		}
//
//
//		public static Vector<double>[] ToHomogeneous(Vector<double>[] cartesians)
//		{
//			var result = (Vector<double>[]) cartesians.Clone ();
//
//			for (int i = 0; i < result.Length; i++) {
//				result [i] = ToHomogeneous (result [i]);
//			}
//
//			return result;
//		}
//
//		public static Vector<double> ToCartesian(Vector<double> homogeneous)
//		{
//			var w = homogeneous.At (homogeneous.Count - 1);
//			var elm = new double[homogeneous.Count - 1];
//
//			for (int i = 0; i < elm.Length; i++) {
//				elm [i] = homogeneous.At (i) / w;
//			}
//
//			return Vector<double>.Build.DenseOfArray(elm);
//		}
//
//
//		public static Vector<double>[] ToCartesian(Vector<double>[] homogeneous)
//		{
//			var result = (Vector<double>[]) homogeneous.Clone ();
//
//			for (int i = 0; i < result.Length; i++) {
//				result [i] = ToCartesian (result [i]);
//			}
//
//			return result;
//		}
//
//	}
//}
//
