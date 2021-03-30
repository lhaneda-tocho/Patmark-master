using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox
{
    /// <summary>
    /// シリアルデータ
    /// </summary>
    public class MBSerialData
    {
        // 表示形式　0:0詰め　1:右詰　2:左詰め
        public UInt16 Format { get; set; }

        // 現在値をリセットする条件　0:最大　1:年　2:月　3:日
        public UInt16 ResetRule { get; set; }

        // 最大カウンタ値
        public UInt32 MaxValue { get; set; }

        // 最小カウンタ値
        public UInt32 MinValue { get; set; }

        // 繰り返し回数（この回数打刻すると、カウントアップ）
        public byte   SkipNumber { get; set; }

        // 繰り返し回数（この回数打刻すると、カウントアップ）
        public byte   RepeatCount { get; set; }

        // 
        public UInt16 ResetTime { get; set; }

        /// <summary>
        /// Gets the reset time as MBSerialTime.
        /// </summary>
        /// <value>The reset time union.</value>
        public MBSerialTime ResetTimeUnion
        {
            get {
                return MBSerialTime.Init(ResetTime);
            }
            set {
                ResetTime = value.HHMM;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.MarkinBox.MBSerialData"/> class.
        /// </summary>
        public MBSerialData(){
            MaxValue = 9999;
        }


    }
}

