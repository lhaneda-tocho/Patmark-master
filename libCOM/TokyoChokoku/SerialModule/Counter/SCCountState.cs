using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Counter
{
    /// <summary>
    /// SCCount state. (Immutable class)
    /// </summary>
    public class SCCountState
    {
        public static readonly SCCountState Default;
        public static readonly IList<SCCountState> DefaultList;

        static SCCountState()
        {
            var count = MBSerial.NumOfSerial;

            Default = Data.Init().ToImmutable();

            var list = new List<SCCountState>(count);
            for (int i = 0; i < count; i++)
            {
                Data data = Default.ToMutable();
                data.SerialNo = (ushort)(i + 1);
                list.Add(data.ToImmutable());
            }

            DefaultList = list.ToImmutableList();
        }

        public struct Data
        {
            // シリアルNo (読み込みのみ)
            public UInt16 SerialNo;

            // 繰り返し回数
            public UInt16 RepeatingCount;

            // 現在値
            public UInt32 CurrentValue;

            // 最終更新日
            public SCDate LastUpdateDate;

            // 最終更新時刻
            public SCTime LastUpdateTime;

            // ファイル番号
            public UInt16 FileNo;

            public static Data From(MBSerialCounterData scdata) {
                var ins = Init(
                    repeatingCount: scdata.RepeatingCount,
                    currentValue: scdata.CurrentValue,
                    lastUpdateDate: SCDate.From(scdata.LastUpdateDateUnion),
                    lastUpdateTime: SCTime.From(scdata.LastUpdateTimeUnion),
                    fileNo: scdata.FileNo
                );
                ins.SerialNo = scdata.SerialNo;
                return ins;
            } 

            public static Data Init(ushort repeatingCount, uint currentValue, SCDate lastUpdateDate, SCTime lastUpdateTime, ushort fileNo) {
                var ins = new Data();

                ins.RepeatingCount = repeatingCount;
                ins.CurrentValue = currentValue;
                ins.LastUpdateDate = lastUpdateDate;
                ins.LastUpdateTime = lastUpdateTime;
                ins.FileNo = fileNo;

                return ins;
            }

            public static Data Init() {
                var ins = new Data();

                ins.RepeatingCount = 0;
                ins.CurrentValue   = 0;
                ins.LastUpdateDate = new SCDate();
                ins.LastUpdateTime = new SCTime();
                ins.FileNo         = 0;
                return ins;
            }

            public SCCountState ToImmutable() {
                return new SCCountState(ref this);
            }

            public MBSerialCounterData ToMBForm(ushort serialNo)
            {
                if(! MBSerialNumber.Verify(serialNo)) {
                    throw new ArgumentOutOfRangeException("SerialNo out of range : " + serialNo);
                }
                var ins = new MBSerialCounterData();
                ins.SerialNo            = serialNo;
                ins.RepeatingCount      = RepeatingCount;
                ins.CurrentValue        = CurrentValue;
                ins.LastUpdateDateUnion = LastUpdateDate.ToMBForm();
                ins.LastUpdateTimeUnion = LastUpdateTime.ToMBForm();
                ins.FileNo              = FileNo;
                return ins;
            }

            public override string ToString()
            {
                if(SerialNo != 0)
                    return string.Format("[Data: SerialNo={0}, RepeatingCount={1}, CurrentValue={2}, LastUpdateDate={3}, LastUpdateTime={4}, FileNo={5}]", SerialNo, RepeatingCount, CurrentValue, LastUpdateDate, LastUpdateTime, FileNo);
                return string.Format("[RepeatingCount={0}, CurrentValue={1}, LastUpdateDate={2}, LastUpdateTime={3}, FileNo={4}]", RepeatingCount, CurrentValue, LastUpdateDate, LastUpdateTime, FileNo);
            }
        }

        Data All;

        // 繰り返し回数
        public UInt16 RepeatingCount => All.RepeatingCount;

        // 現在値
        public UInt32 CurrentValue   => All.CurrentValue;

        // 最終更新日
        public SCDate LastUpdateDate => All.LastUpdateDate;

        // 最終更新時刻
        public SCTime LastUpdateTime => All.LastUpdateTime;

        // ファイル番号
        public UInt16 FileNo         => All.FileNo;

        /// <summary>
        /// バイナリからシリアル番号とSCCountState のタプルを返します)
        /// </summary>
        public static (int SerialNo, SCCountState CountState) From(MBSerialCounterData scdata) {
            var data = Data.From(scdata);
            return (scdata.SerialNo, new SCCountState(ref data));
        }

        public SCCountState(ref Data all)
        {
            All = all;
        }

        public Data ToMutable() => All;

        public MBSerialCounterData ToMBForm(ushort serialNo) => All.ToMBForm(serialNo);

        public override string ToString()
        {
            return All.ToString();
        }
    }
}
