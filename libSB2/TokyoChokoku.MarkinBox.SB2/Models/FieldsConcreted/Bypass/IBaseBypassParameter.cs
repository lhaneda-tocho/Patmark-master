using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public partial class IBaseBypassParameter
    {
        public RectangleArea Bounds {
            get {
                return CalcBoundingBox ();
            }
        }

        public RectangleArea CalcBoundingBox () {
            return new RectangleArea ((double) X, (double)Y, 0, 0);
        }
    }
}
