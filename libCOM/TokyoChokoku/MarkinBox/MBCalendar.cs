using System;
namespace TokyoChokoku.MarkinBox
{
    public static class MBCalendar
    {
        public const int NumOfShift   = 5;
        public const int WordsOfShift = 3;
        public const int BytesOfShift = 6;

        public const int CharsOfYearReplacements  = 10;
        public const int CharsOfMonthReplacements = 12;
        public const int CharsOfDayReplacements   = 31;

        //public const int ByteSize = (NumOfShift * BytesOfShift) + CharsOfYearReplacements + CharsOfMonthReplacements + CharsOfDayReplacements;
        public const int TotalBytesOfShifts = NumOfShift * BytesOfShift;
        public const int BytesOfAvailableShiftSize = 2;

        public const int TotalBytesOfYMD = CharsOfYearReplacements + CharsOfMonthReplacements + CharsOfDayReplacements;
    }
}
