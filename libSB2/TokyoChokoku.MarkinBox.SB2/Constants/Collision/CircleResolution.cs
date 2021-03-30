using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class CircleResolution
    {
        public static double DegreesPerStep {
            get {
                return 15.0d;
            }
        }

        public static double RadiansPerStep {
            get {
                return UnitConverter.Degrees.ToRadians (DegreesPerStep);
            }
        }
    }
}

