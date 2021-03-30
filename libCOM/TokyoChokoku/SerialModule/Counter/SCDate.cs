using System;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Counter
{
    public struct SCDate
    {
        public UInt32 unknown;

        // for Unit Test.
        [Obsolete("これは単体テスト用の実装です")]
        public static SCDate InitWithInt32(UInt32 unknown)
        {
            var ins = new SCDate();
            ins.unknown = unknown;
            return ins;
        }

        public static SCDate Init()
        {
            var ins = new SCDate();
            ins.unknown = 0;
            return ins;
        }

        public static SCDate From(MBSerialDate date) {
            var ins = Init();
            ins.unknown = date.unknown;
            return ins;
        }

        public MBSerialDate ToMBForm() {
            return MBSerialDate.Init(unknown);
        }

        public override string ToString()
        {
            return string.Format("[unknown={0}]", unknown);
        }
    }
}
