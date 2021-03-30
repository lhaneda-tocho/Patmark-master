using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// 点の衝突判定．
    /// </summary>
    public class PointCollision : ICollision
    {
        readonly Cartesian2D point;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="point">Point.</param>
        public PointCollision (Position2D point)
        {
            this.point = point.Cartesian;
        }

        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }

        /// <summary>
        /// 点との判定．OnCircle (point, x) を呼び出して判定します．
        /// </summary>
        /// <param name="point">Point.</param>
        public bool At (Homogeneous2D point)
        {
            return OnCircle (point, 0.01);
        }

        /// <summary>
        /// 円との判定
        /// </summary>
        /// <returns><c>true</c>, if circle was oned, <c>false</c> otherwise.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="radius">Radius.</param>
        public bool OnCircle (Homogeneous2D origin, double radius)
        {
            double distance = (point - origin.ToCartesian ()).Norm (2);
            return distance <= radius;
        }

        /// <summary>
        /// 箱との判定
        /// </summary>
        /// <returns><c>true</c>, if box was ined, <c>false</c> otherwise.</returns>
        /// <param name="rect">Rect.</param>
        public bool InBox (RectangleArea rect)
        {
            var sx = rect.X;
            var sy = rect.Y;
            var ex = sx + rect.Width;
            var ey = sy + rect.Height;

            return
                sx <= point.X && point.X <= ex &&
                sy <= point.Y && point.Y <= ey;
        }
    }
}

