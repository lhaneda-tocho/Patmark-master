using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class DataMatrixConstant
    {
        public static readonly DotCount2D DotCount18x8  = new DotCount2D (18, 8);
        public static readonly DotCount2D DotCount32x8  = new DotCount2D (32, 8);
        public static readonly DotCount2D DotCount10x10 = new DotCount2D (10, 10);
        public static readonly DotCount2D DotCount12x12 = new DotCount2D (12, 12);
        public static readonly DotCount2D DotCount26x12 = new DotCount2D (26, 12);
        public static readonly DotCount2D DotCount36x12 = new DotCount2D (36, 12);
        public static readonly DotCount2D DotCount14x14 = new DotCount2D (14, 14);
        public static readonly DotCount2D DotCount16x16 = new DotCount2D (16, 16);
        public static readonly DotCount2D DotCount36x16 = new DotCount2D (36, 16);
        public static readonly DotCount2D DotCount48x16 = new DotCount2D (48, 16);
        public static readonly DotCount2D DotCount18x18 = new DotCount2D (18, 18);
        public static readonly DotCount2D DotCount20x20 = new DotCount2D (20, 20);
        public static readonly DotCount2D DotCount22x22 = new DotCount2D (22, 22);
        public static readonly DotCount2D DotCount24x24 = new DotCount2D (24, 24);
        public static readonly DotCount2D DotCount26x26 = new DotCount2D (26, 26);
        public static readonly DotCount2D DotCount32x32 = new DotCount2D (32, 32);
        public static readonly DotCount2D DotCount36x36 = new DotCount2D (36, 36);
        public static readonly DotCount2D DotCount40x40 = new DotCount2D (40, 40);

        public static DotCount2D [] DotCountArray ()
        {
            return new DotCount2D [] {
                DotCount18x8,
                DotCount32x8,
                DotCount26x12,
                DotCount36x12,
                DotCount36x16,
                DotCount48x16,

                DotCount10x10,
                DotCount12x12,
                DotCount14x14,
                DotCount16x16,
                DotCount18x18,
                DotCount20x20,
                DotCount22x22,
                DotCount24x24,
                DotCount26x26,
                DotCount32x32,
                DotCount36x36,
                DotCount40x40
            };
        }
    }
}

