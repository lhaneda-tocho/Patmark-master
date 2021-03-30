//using System;
//using MathNet.Numerics.LinearAlgebra;
//
//namespace sketchbook_touch
//{
//	public class AffineMatrices
//	{
//		private AffineMatrices()
//		{
//		}
//
//
//
//
//		public static Matrix<double> Identity2D {
//			get;
//		} = Matrix<double>.Build.DenseOfArray(new double[,] {
//			{ 1, 0, 0 },
//			{ 0, 1, 0 },
//			{ 0, 0, 1 },
//		});
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="angle">degrees を表します</param>
//		public static Matrix<double> Rotation2D(double angle)
//		{
//			var radians = UnitConverter.Degrees.ToRadians (angle);
//			return Matrix<double>.Build.DenseOfArray(new double[,] {
//				{ Math.Cos(radians), - Math.Sin(radians), 0 },
//				{ Math.Sin(radians), Math.Cos(radians), 0 },
//				{ 0, 0, 1 },
//			});
//		}
//
//		public static Matrix<double> Scaling2D(double sx, double sy)
//		{
//			return Matrix<double>.Build.DenseOfArray(new double[,] {
//				{ sx, 0, 0 },
//				{ 0, sy, 0 },
//				{ 0, 0, 1 },
//			});
//		}
//
//		public static Matrix<double> Translation2D(double x, double y)
//		{
//			return Matrix<double>.Build.DenseOfArray(new double[,] {
//				{ 1, 0, x },
//				{ 0, 1, y },
//				{ 0, 0, 1 },
//			});
//		}
//
//		public static Matrix<double> Affine2D(
//			double scaleX, double skewX,  double positionX,
//			double skewY,  double scaleY, double positionY
//		) {
//			return Matrix<double>.Build.DenseOfArray(new double[,] {
//				{ scaleX,  skewX, positionX },
//				{  skewY, scaleY, positionY },
//				{ 0, 0, 1 },
//			});
//		}
//	}
//}
//
