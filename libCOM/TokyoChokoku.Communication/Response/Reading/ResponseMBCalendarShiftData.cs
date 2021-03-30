using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox;
using BN = TokyoChokoku.Structure.Binary;

namespace TokyoChokoku.Communication
{
    public class ResponseMBCalendarShiftData: AbstractResponse
    {
        private BN.MBCalendarShiftDataBinarizer bin;

        public ResponseMBCalendarShiftData(ReadResponse raw): base(raw)
        {
            bin = new BN.MBCalendarShiftDataBinarizer(Data);
        }

        public List<MBCalendarShiftData> Value
        {
            get
            {
                return bin.ConstructObject();
            }
        }

        public ushort AvailableCount
        {
            get
            {
                return bin.AvailableShiftsCount;
            }
        }
    }
}
