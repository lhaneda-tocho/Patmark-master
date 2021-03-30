using System;

namespace TokyoChokoku.Structure.Binary.FileFormat
{
    public static class ModeBit
    {
        public static ushort CALENDER   = 0x0002;
        
        public static ushort SERIAL     = 0x0008;
        
        public static ushort OUTER_ARC  = 0x0020;
        public static ushort INNER_ARC  = 0x0040;


        public static ushort TRIANGLE   = 0x0200;
        public static ushort QR_CODE    = 0x0400;
        public static ushort DATA_MATRIX= 0x0800;
        public static ushort VERTICAL_Y = 0x1000;
        public static ushort VERTICAL_X = 0x2000;
        public static ushort RECTANGLE_LINE_TRIANGLE
                                        = 0x4000;

        
        public static ushort CIRCLE_ELLIPSE
                                        = 0x8000;

    }
}

