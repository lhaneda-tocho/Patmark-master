using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class CGPointExt
    {
        public static Position2D ToPosition2D (this CGPoint point)
        {
            return new Position2D (point.X, point.Y);
        }

        public static Cartesian2D ToCartesian2D(this CGPoint point)
        {
            return new Cartesian2D(point.X, point.Y);
        }
    }
}

