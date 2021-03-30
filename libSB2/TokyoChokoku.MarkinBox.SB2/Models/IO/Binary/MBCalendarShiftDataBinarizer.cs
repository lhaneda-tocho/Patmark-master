using System;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBCalendarShiftDataBinarizer
    {
        const int NumofShift =  Calendar.Consts.NumOfShift;

        private List<byte> Data = new List<byte>();

        public MBCalendarShiftDataBinarizer (MBCalendarData calendar)
        {
            Data.AddRange (BigEndianBitConverter.GetBytes ((char)calendar.AvailableShiftsCount));
            foreach (var shift in calendar.ShiftReplacements) {
                Data.AddRange (BigEndianBitConverter.GetBytes (shift.Code));
                Data.AddRange (BigEndianBitConverter.GetBytes (shift.StartingValue));
                Data.AddRange (BigEndianBitConverter.GetBytes (shift.EndingValue));
            }
        }

        public byte[] GetBytes()
        {
            return Data.ToArray ();
        }
    }
}

