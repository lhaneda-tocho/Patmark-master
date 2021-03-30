using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public static class Homogeneous2DArrayExt
	{

		public static Cartesian2D[] ToCartesian(this Homogeneous2D[] arrays)
		{
			var newIns = new Cartesian2D [arrays.Length];

			for (int i = 0; i < newIns.Length; i++) {
				newIns [i] = arrays [i].ToCartesian ();
			}

			return newIns;
		}
	}
}

