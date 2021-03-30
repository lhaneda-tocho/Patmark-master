using System;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class Cartesian2DExt
    {
        /// <summary>
        /// CGPoint を Cartesian2D に変換します．
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static CGPoint ToCGPoint(this Cartesian2D self) => new CGPoint((nfloat)self.X, (nfloat)self.Y);
    }
}
