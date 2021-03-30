using System;

namespace TokyoChokoku.FieldTextKit
{
    public enum FieldTextType
    {
        /// <summary>
        /// 単なる文字列を表すノードです．
        /// </summary>
        Text,

        /// <summary>
        /// ロゴを表すノードです．
        /// </summary>
        Logo,

        /// <summary>
        /// カレンダーを表すノードです．
        /// </summary>
        Calendar,

        /// <summary>
        /// シリアル番号を表すノードです．
        /// </summary>
        Serial
    }
}

