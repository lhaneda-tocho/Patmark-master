using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Serial
{
    public static class Consts
    {
        public const int NumOfSerial = 4;

        public const int WordsOfSerialSetting = 8;
        public const int BytesOfSerialSetting = WordsOfSerialSetting * 2;
        public const int WordsOfSerialCounter = 8;
        public const int BytesOfSerialCounter = WordsOfSerialCounter * 2;

        public const int RightJustifiedByFillingZero = 0;
        public const int RightJustified              = 1;
        public const int LeftJustified               = 2;

        public const string FormatRegEx = @"@S\[(\d|\s)+-\d+\]";
    }
}

