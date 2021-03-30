using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialNode : IFieldTextNode
    {
        /// <summary>
        /// 現在値．文字列から読み取ったものです．
        /// </summary>
        /// <value>The current value.</value>
        public int CurrentValue { get; }

        /// <summary>
        /// シリアル設定値．文字列から読み取ったものです．
        /// </summary>
        /// <value>The serial number.</value>
        public int SerialNumber { get; }

        /// <summary>
        /// 有効なシリアルインデックスを返します。
        /// </summary>
        public int? SerialIndexOrNull
        {
            get
            {
                return SerialNumberIsValid ? (int?)(SerialNumber - 1) : null;
            }
        }

        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Serial;
            }
        }

        public bool SerialNumberIsValid {
            get {
                return
                    0            <  SerialNumber &&
                    SerialNumber <= Serial.Consts.NumOfSerial;
            }
        }

        /// <summary>
        /// 解析したSerialNumberと一致するSerialDataを返します．
        /// 存在しなければ null を返します．
        /// </summary>
        public MBSerialData SerialData {
            get {
                var settings = SerialSettingsManager.Instance.Settings;
                if (!SerialNumberIsValid)
                    return null;
                if (settings.Count != Serial.Consts.NumOfSerial)
                    return null;
                return settings [(int)SerialIndexOrNull];
            }
        }

        /// <summary>
        /// カウンターを取得します．
        /// カウンタが存在しなければ null を返します．
        /// </summary>
        public MBSerialCounterData Counter {
            get {
                var counters = SerialSettingsManager.Instance.Counters;
                if (!SerialNumberIsValid)
                    return null;
                if (counters.Count != Serial.Consts.NumOfSerial)
                    return null;
                return SerialSettingsManager.Instance.Counters [(int)SerialIndexOrNull];
            }
        }

        private int GetValidCount ()
        {
            return CurrentValue;
            /*int count = CurrentValue;
            var serial = SerialData;
            if (serial == null)
                return count;
            if (count < serial.MinValue || count > serial.MaxValue)
                return Counter.CurrentValue;
            return count;*/
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="current">Current.</param>
        /// <param name="serial">Serial.</param>
        public SerialNode (int current, int serial)
        {
            CurrentValue = current;
            SerialNumber = serial ;
        }


        /// <summary>
        /// シリアル設定を適用した後の文字数を取得します．
        /// シリアル設定を取得できない場合は Tag.Length を代わりに返します．
        /// </summary>
        public int CharCount {
            get {
                return ToString().Length;
            }
        }

        /// <summary>
        /// シリアル設定を適用した後の文字列を取得します．
        /// シリアル設定を取得できない場合は Tag を代わりに返します
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString ()
        {
            var serial  = SerialData;
            if (serial == null)
                return Tag;
            var format   = serial.Format;
            var maxValue = serial.MaxValue;
            switch (format) {
                case Serial.Consts.RightJustifiedByFillingZero: {
                    int width = maxValue.ToString ().Length;
                    int count = GetValidCount ();
                    return count.ToString ().PadLeft (width, '0');
                }
                case Serial.Consts.RightJustified: {
                    int width = maxValue.ToString ().Length;
                    int count = GetValidCount ();
                    return count.ToString ().PadLeft (width, ' ');
                }
                case Serial.Consts.LeftJustified: {
                    int count = GetValidCount ();
                    return count.ToString ();
                }
                default: {
                    throw new ArgumentOutOfRangeException ();
                }
            }
        }

        /// <summary>
        /// 以下の形式で文字列を返します．
        /// 
        /// @S[v-n] 
        /// v: 現在値， n: シリアル設定値
        /// 
        /// </summary>
        /// <value>The tag.</value>
        public string Tag
        {
            get {
                var sb = new System.Text.StringBuilder ();
                sb.Append ("@S[").Append (CurrentValue).Append ('-').Append (SerialNumber).Append (']');
                return sb.ToString ();
            }
        }


    }
}

