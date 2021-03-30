using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox
{
    public class MBSerialCounterData
    {
        public MBSerialCounterData(){
        }

        // シリアル設定番号
        public UInt16 SerialNo { get; set; }

        // 繰り返し回数
        public UInt16 RepeatingCount { get; set; }

        // 現在値
        public UInt32 CurrentValue { get; set; }

        // 最終更新日
        public UInt32 LastUpdateDate { get; set; }

        // 最終更新時刻
        public UInt16 LastUpdateTime { get; set; }

        // ファイル番号
        public UInt16 FileNo { get; set; }

        public MBSerialDate LastUpdateDateUnion {
            get {
                return MBSerialDate.Init(LastUpdateDate);
            }
            set {
                LastUpdateDate = value.unknown;
            }
        }

        public MBSerialTime LastUpdateTimeUnion {
            get {
                return MBSerialTime.Init(LastUpdateTime);
            }
            set {
                LastUpdateTime = value.HHMM;
            }
        }
    }
}

