using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBSerialSettingsDataBinarizer
    {
        private List<byte> Data = new List<byte>();

        public MBSerialSettingsDataBinarizer (List<MBSerialData> serials)
        {
            foreach (var serial in serials)
            {
                Data.AddRange(serial.GetBytes());
            }
        }

        public byte[] GetBytes()
        {
            return Data.ToArray ();
        }
    }
}

