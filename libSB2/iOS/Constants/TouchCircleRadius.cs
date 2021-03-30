using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class TouchCircleRadius
    {
        private static double PixelPerMilli = 10;
        private static double IOS6s = 20;


        public static double Get (double scale, double pixelPerMilli)
        {
            return IOS6s / (scale*pixelPerMilli);
        }
    }
}

