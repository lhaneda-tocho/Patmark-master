using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SpeedRange
    {
        public static readonly short Min = 0;
        public static readonly short Max = 98;

        public static readonly short Default = 49;

        public static short [] ToArray ()
        {
            short [] range = new short [Max - Min + 1];
            for (short i = 0; i < range.Length; i++)
                range [i] = (short)(Min + i);
            return range;
        }
    }
}

