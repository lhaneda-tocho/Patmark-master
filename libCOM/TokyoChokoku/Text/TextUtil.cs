using System;
using System.Collections.Generic;
namespace TokyoChokoku.Text
{
    using CharTable = Dictionary<string, string>;

    public static class TextUtil
    {
        private static readonly CharTable KanaTable = new CharTable {
            { "ｶﾞ", "ガ" }, { "ｷﾞ", "ギ" }, { "ｸﾞ", "グ" }, { "ｹﾞ", "ゲ" }, { "ｺﾞ", "ゴ" },
            { "ｻﾞ", "ザ" }, { "ｼﾞ", "ジ" }, { "ｽﾞ", "ズ" }, { "ｾﾞ", "ゼ" }, { "ｿﾞ", "ゾ" },
            { "ﾀﾞ", "ダ" }, { "ﾁﾞ", "ヂ" }, { "ﾂﾞ", "ヅ" }, { "ﾃﾞ", "デ" }, { "ﾄﾞ", "ド" },
            { "ﾊﾞ", "バ" }, { "ﾋﾞ", "ビ" }, { "ﾌﾞ", "ブ" }, { "ﾍﾞ", "ベ" }, { "ﾎﾞ", "ボ" },
            { "ﾊﾟ", "パ" }, { "ﾋﾟ", "ピ" }, { "ﾌﾟ", "プ" }, { "ﾍﾟ", "ペ" }, { "ﾎﾟ", "ポ" },
            { "ｳﾞ", "ヴ" },
            // 1文字構成の半角カナ
            { "ｱ", "ア" }, { "ｲ", "イ" }, { "ｳ", "ウ" }, { "ｴ", "エ" }, { "ｵ", "オ" },
            { "ｶ", "カ" }, { "ｷ", "キ" }, { "ｸ", "ク" }, { "ｹ", "ケ" }, { "ｺ", "コ" },
            { "ｻ", "サ" }, { "ｼ", "シ" }, { "ｽ", "ス" }, { "ｾ", "セ" }, { "ｿ", "ソ" },
            { "ﾀ", "タ" }, { "ﾁ", "チ" }, { "ﾂ", "ツ" }, { "ﾃ", "テ" }, { "ﾄ", "ト" },
            { "ﾅ", "ナ" }, { "ﾆ", "ニ" }, { "ﾇ", "ヌ" }, { "ﾈ", "ネ" }, { "ﾉ", "ノ" },
            { "ﾊ", "ハ" }, { "ﾋ", "ヒ" }, { "ﾌ", "フ" }, { "ﾍ", "ヘ" }, { "ﾎ", "ホ" },
            { "ﾏ", "マ" }, { "ﾐ", "ミ" }, { "ﾑ", "ム" }, { "ﾒ", "メ" }, { "ﾓ", "モ" },
            { "ﾔ", "ヤ" }, { "ﾕ", "ユ" }, { "ﾖ", "ヨ" },
            { "ﾗ", "ラ" }, { "ﾘ", "リ" }, { "ﾙ", "ル" }, { "ﾚ", "レ" }, { "ﾛ", "ロ" },
            { "ﾜ", "ワ" }, { "ｦ", "ヲ" }, { "ﾝ", "ン" },
            { "ｧ", "ァ" }, { "ｨ", "ィ" }, { "ｩ", "ゥ" }, { "ｪ", "ェ" }, { "ｫ", "ォ" },
            { "ｬ", "ャ" }, { "ｭ", "ュ" }, { "ｮ", "ョ" }, { "ｯ", "ッ" },
            { "｡", "。" }, { "｢", "「" }, { "｣", "」" }, { "､", "、" }, { "･", "・" },
            { "ｰ", "ー" }
        };

        private static bool TryPutDi(System.Text.StringBuilder sb, string text, int index)
        {
            if(text.Length-index >= 2) 
            {
                var di = text.Substring(index, 2);
                if (KanaTable.ContainsKey(di))
                {
                    sb.Append(KanaTable[di]);
                    return true;
                }
            }
            return false;
        }

        public static string ToFullForKana(string text)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < text.Length; i++) {
                if (TryPutDi(sb, text, i))
                {
                    i++;
                    continue;
                }
                var mono = text.Substring(i, 1);
                // 2文字カナに一致が存在
                if(KanaTable.ContainsKey(mono)) {
                    sb.Append(KanaTable[mono]);
                    i++; // 2つ進める必要があるため，ここで1文字進めておく.
                    continue;
                }
                // 2文字カナに一致が存在
                if(KanaTable.ContainsKey(mono)) {
                    sb.Append(KanaTable[mono]);
                    continue;
                }
                // それ以外は無視
                sb.Append(mono);
            }
            return sb.ToString();
        }
    }
}
