using System;
namespace TokyoChokoku.MarkinBox
{
    public struct MBSerialDate
    {
        public UInt32 unknown;

        public static MBSerialDate Init(UInt32 date) {
            var ins = new MBSerialDate();
            ins.unknown = date;
            return ins;
        }
    }
}
