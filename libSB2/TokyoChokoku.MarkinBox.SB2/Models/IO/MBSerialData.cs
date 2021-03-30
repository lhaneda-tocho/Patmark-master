using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBSerialData
    {
        private readonly byte[] dataBin = new byte[16]{
            0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00
        };

        private readonly MappedBigEndShort format;
        private readonly MappedBigEndShort clearingCondition;
        private readonly MappedBigEndInt maxValue;
        private readonly MappedBigEndInt minValue;
        private readonly MappedByte skipNumber;
        private readonly MappedByte repeatingCount;
        private readonly MappedBigEndShort clearingTime;

        public MBSerialData(){
            format = new MappedBigEndShort (dataBin, 0);
            clearingCondition = new MappedBigEndShort (dataBin, format.NextOffset);
            maxValue = new MappedBigEndInt (dataBin, clearingCondition.NextOffset);
            minValue = new MappedBigEndInt (dataBin, maxValue.NextOffset);
            skipNumber = new MappedByte(dataBin, minValue.NextOffset);
            repeatingCount = new MappedByte(dataBin, skipNumber.NextOffset);
            clearingTime = new MappedBigEndShort (dataBin, repeatingCount.NextOffset);

            MaxValue = 9999;
        }

        public void SetBin(byte[] bin){
            for (var i = 0; i < dataBin.Length && i < bin.Length; i++) {
                dataBin [i] = bin [i];
            }
        }

        // 表示形式　0:0詰め　1:右詰　2:左詰め
        public short Format{
            get { return format.Value; }
            set { format.Value = value; }
        }

        // 現在値をリセットする条件　0:最大　1:年　2:月　3:日
        public short ClearingCondition{
            get { return clearingCondition.Value; }
            set { clearingCondition.Value = value; }
        }

        // 最大カウンタ値
        public int MaxValue{
            get { return maxValue.Value; }
            set { maxValue.Value = value; }
        }

        // 最小カウンタ値
        public int MinValue{
            get { return minValue.Value; }
            set { minValue.Value = value; }
        }

        // 繰り返し回数（この回数打刻すると、カウントアップ）
        public byte SkipNumber
        {
            get { return skipNumber.Value; }
            set { skipNumber.Value = value; }
        }

        // 繰り返し回数（この回数打刻すると、カウントアップ）
        public byte RepeatingCount{
            get { return repeatingCount.Value; }
            set { repeatingCount.Value = value; }
        }

        // リセット時間(HH)
        public short ClearingTimeHH{
            get {
                var val = new ValueDividedHHMM ();
                val.Value = clearingTime.Value;
                return (short)val.HH;
            }
            set {
                var val = new ValueDividedHHMM ();
                val.Value = clearingTime.Value;
                val.HH = value;
                clearingTime.Value = val.Value;
            }
        }

        public short ClearingTimeMM{
            get {
                var val = new ValueDividedHHMM ();
                val.Value = clearingTime.Value;
                return (short)val.MM;
            }
            set {
                var val = new ValueDividedHHMM ();
                val.Value = clearingTime.Value;
                val.MM = value;
                clearingTime.Value = val.Value;
            }
        }

        public class ValueDividedHHMM
        {
            public int H10;
            public int H1;
            public int M10;
            public int M1;

            public int HH {
                get { return (H10 * 10 + H1); }
                set {
                    H10 = (int)Math.Floor ((decimal)(value / 10));
                    H1 = (value % 10);
                }
            }

            public int MM {
                get { return M10 * 10 + M1; }
                set {
                    M10 = (int)Math.Floor ((decimal)(value / 10));
                    M1 = (value % 10);
                }
            }

            public short Value{
                get {
                    return (short)(
                        (H10 * Math.Pow (16, 3)) +
                        (H1 * Math.Pow (16, 2)) +
                        (M10 * Math.Pow (16, 1)) +
                        (M1)
                    );
                }
                set {
                    var v = (int)(value);
                    H10 = (int)(v / Math.Pow (16, 3));
                    v = (v % (int)Math.Pow (16, 3));
                    H1 = (int)(v / Math.Pow (16, 2));
                    v = (v % (int)Math.Pow (16, 2));
                    M10 = (int)(v / Math.Pow (16, 1));
                    v = (v % (int)Math.Pow (16, 1));
                    M1 = v;
                }
            }
        }

        /// <summary>
        /// Binarize
        /// </summary>
        /// <returns>The bytes.</returns>
        public byte[] GetBytes()
        {
            var result = new byte[] { };
            Array.Resize(ref result, dataBin.Length);
            dataBin.CopyTo(result, 0);
            return result;
        }

    }
}

