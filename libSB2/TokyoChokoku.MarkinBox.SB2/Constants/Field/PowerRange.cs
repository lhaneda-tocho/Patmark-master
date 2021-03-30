namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class PowerRange
    {
        public static readonly short Min = 0;
        public static readonly short Max = 99;

        public static readonly short Default = 50;

        public static short [] ToArray ()
        {
            short [] range = new short [Max - Min + 1];
            for (short i = 0; i < range.Length; i++)
                range [i] = (short)(Min + i);
            return range;
        }
    }
}

