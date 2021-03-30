using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBSerialCountersDataBinarizer
    {
        private List<byte> Data = new List<byte>();

        public MBSerialCountersDataBinarizer(List<MBSerialCounterData> counters)
        {
            foreach (var counter in counters)
            {
                Data.AddRange(counter.GetBytes());
            }
        }

        public byte[] GetBytes()
        {
            return Data.ToArray ();
        }
    }
}

