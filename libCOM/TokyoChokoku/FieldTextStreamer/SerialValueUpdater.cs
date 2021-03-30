using System;
using System.Collections.Generic;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.FieldTextStreamer
{
    public class SerialValueUpdater
    {
        SCSettingList       SettingList { get; }
        SCCountStateList    StateList   { get; }
        ISet<int>           NoSet       { get; }

        public SerialValueUpdater(
            SCSettingList       settingList,
            SCCountStateList    countStateList
        )
        {
            // Assertion
            if(settingList.Count != countStateList.Count)
                throw new InvalidProgramException("AssertionError: settingList.Count and countStateList.Count is not same length.");

            SettingList = settingList ?? throw new ArgumentNullException(nameof(settingList));
            StateList   = countStateList ?? throw new ArgumentNullException(nameof(countStateList));
            NoSet       = new HashSet<int>(settingList.SerialNoList);
        }

        /// <summary>
        /// 引数のシリアル番号が適切であれば true, そうでなければ false を返します。
        /// </summary>
        bool ValidNo(int no)
        {
            return NoSet.Contains(no);
        }

        /// <summary>
        /// フィールドテキスト文字列を置換します．
        /// シリアルNo が定義域外のもの、置換できないもの, 変換できないもの (ロゴなど) はそのままにします．
        /// </summary>
        public string Replace(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var stream = new FieldTextStream(text);
            var enu = stream.Select(
                onText  : OnText,
                onSerial: OnSerial
            );
            return string.Join("", enu);
        }


        string OnText(TextPart part)
        {
            return part.Text;
        }

        string OnSerial(SerialPart part)
        {
            if (part == null)
                throw new ArgumentNullException(nameof(part));

            var id = part.ID;

            if (!ValidNo(id))
                return part.ToString();
            var state   = StateList[id];
            var setting = SettingList[id];

            return part.Copy(value: (int)state.CurrentValue).ToString(setting);
        }
    }
}
