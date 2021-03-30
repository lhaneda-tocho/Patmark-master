using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class IntSize2D
    {
        private readonly int sx;
        private readonly int sy;

        public int X {
            get {
                return sx;
            }
        }

        public int Y {
            get {
                return sy;
            }
        }

        public IntSize2D (int sx, int sy)
        {
            this.sx = sx;
            this.sy = sy;
        }

        public IntSize2D (int scale)
        {
            this.sx = scale;
            this.sy = scale;
        }

        public Size2D ToFloat () {
            return new Size2D (sx, sy);
        }

        override
        public String ToString()
        {
            return 
                GetType ().FullName
                + "( Sx = " + X
                + ", Sy = " + Y + ")";
        }
    }
}

