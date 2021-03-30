using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// 空のフィールドを除去します。
    /// </summary>
    public static class EmptyFieldRemover
    {
        public static IEnumerable<MBData> FilterNotEmpty(IEnumerable<MBData> src)
        {
            src = src ?? throw new ArgumentNullException();
            return src.Where(it =>
            {
                var isEmpty =  IsEmptyTextField(it)
                            || IsEmptyZebraCrossField(it)
                            ;
                return !isEmpty;
            });
        }


        static bool IsZebraCrossField(MBData data)
        {
            return data.IsZebraCrossField();
        }


        static bool IsTextField(MBData data)
        {
            return data.IsTextField();
        }


        static bool IsEmptyZebraCrossField(MBData data)
        {
            if (!IsZebraCrossField(data))
                return false;
            return string.IsNullOrEmpty(data.Text);
        }


        static bool IsEmptyTextField(MBData data)
        {
            if (!IsTextField(data))
                return false;
            return string.IsNullOrWhiteSpace(data.Text);
        }
    }
}
