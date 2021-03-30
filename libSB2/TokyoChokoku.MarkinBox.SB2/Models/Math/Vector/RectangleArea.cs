using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// 矩形領域を表す変数です
    /// </summary>
    public class RectangleArea
    {
        /// <summary>
        /// 矩形の位置
        /// </summary>
        /// <value>The position.</value>
        public Position2D position { get; }
        /// <summary>
        /// 矩形のサイズ
        /// </summary>
        /// <value>The size.</value>
        public Size2D     size     { get; }

        /// <summary>
        /// 矩形のX座標
        /// </summary>
        /// <value>The x.</value>
        public double X {
            get {
                return position.X;
            }
        }

        /// <summary>
        /// 矩形のY座標
        /// </summary>
        /// <value>The y.</value>
        public double Y {
            get {
                return position.Y;
            }
        }

        /// <summary>
        /// 矩形の幅
        /// </summary>
        /// <value>The width.</value>
        public double Width {
            get {
                return size.X;
            }
        }

        /// <summary>
        /// 矩形の高さ
        /// </summary>
        /// <value>The height.</value>
        public double Height {
            get {
                return size.Y;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="size">Size.</param>
        public RectangleArea (Position2D position, Size2D size)
        {
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public RectangleArea (double x, double y, double width, double height)
        {
            this.position = new Position2D (x, y);
            this.size = new Size2D (width, height);
        }

        /// <summary>
        /// 行列による座標変換を行います．
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="transform">Transform.</param>
        public  Position2D[] ApplyTransform (MatrixContext transform) {
            return ApplyTransform (transform.CurrentMatrix);
        }

        /// <summary>
        /// 行列による座標変換を行います．
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="transform">Transform.</param>
        public Position2D[] ApplyTransform (AffineMatrix2D transform) {
            var start = this.position;
            var end = this.size.ToPosition (position);

            var startEnd = new Position2D (start.X, end.Y);
            var endStart = new Position2D (end.X, start.Y);

            return new Position2D[] {
                transform * start,
                transform * startEnd,
                transform * end,
                transform * endStart,
            };
        }

        /// <summary>
        /// 矩形の図心を返します．
        /// </summary>
        /// <returns>The center.</returns>
        public Position2D GetCenter ()
        {
            return new Position2D (X + 0.5 * Width, Y + 0.5 * Height);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:TokyoChokoku.MarkinBox.Sketchbook.RectangleArea"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:TokyoChokoku.MarkinBox.Sketchbook.RectangleArea"/>.</returns>
        public override string ToString ()
        {
            return string.Format ("[RectangleArea: X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
        }
    }
}

