using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBCalendarDataBinarizer
    {
        const int NumofShift =  Calendar.Consts.NumOfShift;

        private List<byte> Data = new List<byte>();

        public MBCalendarDataBinarizer (MBCalendarData calendar)
        {
            foreach (var pair in calendar.YearReplacements) {
                Data.AddRange(BigEndianBitConverter.GetBytes((byte)pair.Value));
            }
            foreach (var pair in calendar.MonthReplacements) {
                Data.AddRange(BigEndianBitConverter.GetBytes((byte)pair.Value));
            }
            foreach (var pair in calendar.DayReplacements) {
                Data.AddRange(BigEndianBitConverter.GetBytes((byte)pair.Value));
            }
        }

        public byte[] GetBytes()
        {
            return Data.ToArray ();
        }
    }
}

