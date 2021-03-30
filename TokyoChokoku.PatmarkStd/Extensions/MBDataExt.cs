using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark
{
    public static class MBDataExt
    {
        static FieldType? FieldTypeOrNull(byte value)
        {
            if (!Enum.IsDefined(typeof(FieldType), value))
            {
                return null;
            }
            return (FieldType)value;
        }

        /// <summary>
        /// バーコードの場合は true を返します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsZebraCrossField(this MBData data)
        {
            var type = FieldTypeOrNull(data.Type);
            if (type == null)
                return false;
            return type == FieldType.DataMatrix
                || type == FieldType.QrCode
                ;
        }

        /// <summary>
        /// バーコードを除いた、テキストを表示するフィールドの場合に true を返します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsTextField(this MBData data)
        {
            var type = FieldTypeOrNull(data.Type);
            if (type == null)
                return false;
            return type == FieldType.HorizontalText
                || type == FieldType.XVerticalText
                || type == FieldType.YVerticalText
                || type == FieldType.InnerArcText
                || type == FieldType.OuterArcText
                ;
        }

        /// <summary>
        /// 表示内容にテキストデータが関わるフィールドの場合は true を返します。
        ///
        /// 直接テキストを表示するフィールド、バーコードのように、テキストを別の形式で表示する
        /// フィールドがこれに当たります。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsTextContainerField(this MBData data)
        {
            return data.IsTextField() || data.IsZebraCrossField();
        }

        /// <summary>
        /// 指定された MBData が、シリアル設定を所有可能であれば、 true を返します。そうでなければ false です。
        /// </summary>
        /// <param name="data">this</param>
        /// <returns>指定された MBData が、有効なシリアル設定を所有可能であれば、 true を返します。そうでなければ false です。</returns>
        public static bool IsHavableSerial(this MBData data)
        {
            return data.IsTextContainerField();
        }

        /// <summary>
        /// 指定された MBData が、有効なシリアル設定を持つ場合は true を返します。そうでなければ false です。
        ///
        /// 以下の2つの条件を満たす場合にシリアルが存在すると判定されます。
        ///
        /// 1. テキスト中にシリアル構文が見つかる
        /// 2. IsHavableSerial(MBData) が true を返す。
        ///
        /// シリアル設定を持たない、無視するフィールドでは必ず false を返します。
        /// </summary>
        /// <param name="data">this</param>
        /// <returns>
        /// 指定された MBData が、有効なシリアル設定を持つ場合は true を返します。そうでなければ false です。
        /// </returns>
        public static bool HasSerial(this MBData data)
        {
            if (!data.IsHavableSerial())
                return false;

            var rootNode = data.ParseText();
            return rootNode.HasSerial;
        }

        static RootFieldTextNode ParseText(this MBData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            return FieldTextParser.ParseText(data.Text);
        }


        //public static bool HasCalendar(this MBData data)
        //{
        //    if (!data.IsTextContainerField())
        //        return false;

        //    var rootNode = data.ParseText();
        //    return rootNode.HasCalendar;
        //}
    }
}
