using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class FamousVectors
    {

        public static class XEntity {
            public static Homogeneous2D OfHomogeneous2D {
                get;
            }  = new Homogeneous2D (1, 0);

            public static Cartesian2D OfCartesian2D {
                get;
            }  = new Cartesian2D (1, 0);
        }

        public static class YEntity {
            public static Homogeneous2D OfHomogeneous2D {
                get;
            }  = new Homogeneous2D (0, 1);

            public static Cartesian2D OfCartesian2D {
                get;
            }  = new Cartesian2D (0, 1);
        }

        public static class Zero {
            public static Homogeneous2D OfHomogeneous2D {
                get;
            }  = new Homogeneous2D (0, 0);

            public static Cartesian2D OfCartesian2D {
                get;
            }  = new Cartesian2D (0, 0);
        }
    }
}

