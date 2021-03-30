using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	namespace UnitConverter
	{
		public static class Radians {
			public static double ToDegrees(double radians)
			{
				return radians * 180d / Math.PI;
			}
		}

		public static class Degrees {
			public static double ToRadians(double degrees)
			{
				return degrees * Math.PI / 180d;
			}
		}
	}
}

