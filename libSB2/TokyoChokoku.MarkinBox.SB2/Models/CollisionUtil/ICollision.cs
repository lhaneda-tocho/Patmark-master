using System;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// 衝突判定を表すインターフェースです
    /// </summary>
	public interface ICollision
	{
        /// 点 との衝突判定
		bool At(Homogeneous2D point);
		/// <summary>
        /// 円との衝突判定
        /// </summary>
        /// <returns><c>true</c>, if circle was oned, <c>false</c> otherwise.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="radius">Radius.</param>
        bool OnCircle(Homogeneous2D origin, double radius);
        /// <summary>
        /// 箱との衝突判定
        /// </summary>
        /// <returns><c>true</c>, if box was oned, <c>false</c> otherwise.</returns>
        bool InBox (RectangleArea rect);

        ///
        /// ビジターを受け入れる
        ///
        R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args);
	}
}

