using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MBSerialCounterData
    {
        private readonly byte[] dataBin = new byte[16]{
            0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00
        };

        private readonly MappedBigEndShort serialNo;
        private readonly MappedBigEndShort repeatingCount;
        private readonly MappedBigEndInt currentValue;
        private readonly MappedBigEndInt lastUpdateDate;
        private readonly MappedBigEndShort lastUpdateTime;
        private readonly MappedBigEndShort fileNo;

        public MBSerialCounterData(){
            serialNo = new MappedBigEndShort (dataBin, 0);
            repeatingCount = new MappedBigEndShort (dataBin, serialNo.NextOffset);
            currentValue = new MappedBigEndInt (dataBin, repeatingCount.NextOffset);
            lastUpdateDate = new MappedBigEndInt (dataBin, currentValue.NextOffset);
            lastUpdateTime = new MappedBigEndShort (dataBin, lastUpdateDate.NextOffset);
            fileNo = new MappedBigEndShort (dataBin, lastUpdateTime.NextOffset);
        }

        public void SetBin(byte[] bin){
            for (var i = 0; i < dataBin.Length && i < bin.Length; i++) {
                dataBin [i] = bin [i];
            }
        }

        // シリアル設定番号
        public short SerialNo{
            get { return serialNo.Value; }
            set { serialNo.Value = value; }
        }

        // 繰り返し回数
        public short RepeatingCount{
            get { return repeatingCount.Value; }
            set { repeatingCount.Value = value; }
        }

        // 現在値
        public int CurrentValue{
            get { return currentValue.Value; }
            set { currentValue.Value = value; }
        }

        // 最終更新日
        public int LastUpdateDate{
            get { return lastUpdateDate.Value; }
            set { lastUpdateDate.Value = value; }
        }

        // 最終更新時刻
        public short LastUpdateTime{
            get { return lastUpdateTime.Value; }
            set { lastUpdateTime.Value = value; }
        }

        // ファイル番号
        public short FileNo{
            get { return fileNo.Value; }
            set { fileNo.Value = value; }
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

