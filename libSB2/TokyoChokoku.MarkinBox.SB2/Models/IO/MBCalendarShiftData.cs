using System;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;
using TokyoChokoku.MarkinBox.Sketchbook.Properties;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBCalendarShiftData
    {
        public char Code = (char)0; // Null文字,ASCII
        public byte StartingHour = (byte)0;
        public byte StartingMinute = (byte)0;
        const byte StartingSecond = (byte)0;
        public byte EndingHour = (byte)0;
        public byte EndingMinute = (byte)0;
        const byte EndingSecond = (byte)59;

        public MBCalendarShiftData ()
        {
        }

        public MBCalendarShiftData (byte[] data)
        {
            if (data.Length >= Calendar.Consts.BytesOfShift) {
                this.Code = BigEndianBitConverter.ToChar (new BigEndianBytes(data.Take(2).ToList()));
                var startingValue = BigEndianBitConverter.ToChar (new BigEndianBytes(data.Skip(2).Take(2).ToList()));
                this.StartingHour = (byte)Math.Floor((decimal)(startingValue * 0.01));
                this.StartingMinute = (byte)(startingValue % 100);
                var endingValue = BigEndianBitConverter.ToChar (new BigEndianBytes(data.Skip(4).Take(2).ToList()));
                this.EndingHour = (byte)Math.Floor((decimal)(endingValue * 0.01));
                this.EndingMinute = (byte)(endingValue % 100);
            }
        }

        public char StartingValue
        {
            get{
                return (char)(StartingHour * 100 + StartingMinute);
            }
        }

        public char EndingValue
        {
            get{
                return (char)(EndingHour * 100 + EndingMinute);
            }
        }

        public class MBCalendarShiftDataStore
        {
            //public CodeStore Code;
            public Store<byte> StartingHour;
            public Store<byte> StartingMinute;
            public Store<byte> EndingHour;
            public Store<byte> EndingMinute;

            public MBCalendarShiftDataStore(MBCalendarShiftData data){
                
            }
        }
    }
}

