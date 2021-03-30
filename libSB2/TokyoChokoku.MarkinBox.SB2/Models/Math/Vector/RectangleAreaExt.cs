using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class RectangleAreaExt
    {
        public static bool inRect (this Position2D point, RectangleArea rect)
        {
            return point.Cartesian.inRect (rect);
        }

        public static bool inRect (this Homogeneous2D point, RectangleArea rect)
        {
            return inPoint (point.CartesianX, point.CartesianY, rect);
        }

        public static bool inRect (this Cartesian2D point, RectangleArea rect)
        {
            return inPoint (point.X, point.Y, rect);
        }

        static bool inPoint (double x, double y, RectangleArea rect)
        {
            return rect.X <= x && x <= (rect.X + rect.Width) && rect.Y <= y && y <= (rect.Y + rect.Height);
        }
    }
}

