using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MatrixContext
	{
		/// <summary>
		/// The matrix stack.
		/// </summary>
		Stack< AffineMatrix2D > MatrixStack;

		/// <summary>
		/// Gets or sets the matrix on stack.
		/// </summary>
		/// <value>The current matrix.</value>
		public AffineMatrix2D CurrentMatrix {
			get{
				return MatrixStack.Peek ();
			}

			set{
				MatrixStack.Pop ();
				MatrixStack.Push (value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TokyoChokoku.MarkinBox.Sketchbook.MatrixContext"/> class.
		/// </summary>
		public MatrixContext ()
		{
			MatrixStack = new Stack<AffineMatrix2D> ();
			MatrixStack.Push (AffineMatrix2D.Entity);
		}

		/// <summary>
		/// Concats rotation matrix.
		/// </summary>
		/// <param name="deg">Degrees of rotate angle.</param>
		public void Rotate(double deg)
		{
			CurrentMatrix = CurrentMatrix * AffineMatrix2D.NewRotation (degrees: deg);
		}

		/// <summary>
		/// Concats translation matrix.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void Translate(double x, double y)
		{
			CurrentMatrix = CurrentMatrix * AffineMatrix2D.NewTranslation (x, y);
		}


		public void Translate(Position2D origin)
		{
			CurrentMatrix = CurrentMatrix * AffineMatrix2D.NewTranslation (origin);
		}

		/// <summary>
		/// Concats scale matrix.
		/// </summary>
		/// <param name="sx">The x coordinate scale.</param>
		/// <param name="sy">The y coordinate scale.</param>
		public void Scale(double sx, double sy)
		{
			CurrentMatrix = CurrentMatrix * AffineMatrix2D.NewScaling (sx, sy);
		}

		public void Scale(Size2D scale)
		{
			CurrentMatrix = CurrentMatrix * AffineMatrix2D.NewScaling (scale);
		}

		/// <summary>
		/// Multiply affine matrix.
		/// </summary>
		/// <param name="scaleX">(0, 0) element.</param>
		/// <param name="skewX">(0, 1) element.</param>
		/// <param name="positionX">(0, 2) element.</param>
		/// <param name="skewY">(1, 0) element.</param>
		/// <param name="scaleY">(1, 1) element.</param>
		/// <param name="positionY">(1, 2) element.</param>
		public void MultiAssignAffine(
			double scaleX, double skewX, double originX,
			double skewY, double scaleY, double originY)
		{
			CurrentMatrix = CurrentMatrix * new AffineMatrix2D (
				scaleX, skewX, originX,
				skewY, scaleY, originY
			);
		}

		/// <summary>
		/// Set Inverse Matrix.
		/// </summary>
		public void Inverse()
		{
			CurrentMatrix = CurrentMatrix.Inverse ();
		}

		/// <summary>
		/// Sets the entity matrix.
		/// </summary>
		public void SetEntity()
		{
			CurrentMatrix = AffineMatrix2D.Entity;
		}

		public void SetAffine(
			double scaleX, double  skewX, double originX,
			double  skewY, double scaleY, double originY)
		{
			CurrentMatrix = new AffineMatrix2D (
				scaleX, skewX, originX,
				skewY, scaleY, originY
			);
		}

		/// <summary>
		/// Pushs the matrix.
		/// </summary>
		public void PushMatrix()
		{
			MatrixStack.Push (CurrentMatrix);
		}

		/// <summary>
		/// Pops the matrix.
		/// </summary>
		public void PopMatrix()
		{
			if (MatrixStack.Count <= 1) {
				throw new InvalidOperationException ("Matrix Stack must have one or more matrix.");
			}

			MatrixStack.Pop ();
		}

		/// <summary>
		/// Applies transform to the specified point.
		/// </summary>
		/// <param name="point">Point.</param>
		public Homogeneous2D Multiply(Homogeneous2D point)
		{
			return CurrentMatrix * point;
		}

		/// <summary>
		/// Applies transform to the specified points.
		/// </summary>
		/// <param name="points">Points.</param>
		public Homogeneous2D[] Multiply(Homogeneous2D[] points)
		{
			var result = (Homogeneous2D[]) points.Clone ();

			for (int i = 0; i < result.Length; i++) {
				result [i] = Multiply (result[i]);
			}

			return result;
		}
	}
}

