/// @file PCFileModeData.cs
/// @brief クラス定義ファイル
using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 検証対象のPCファイルデータを表します。
    /// </summary>
    public class PCFileModeData
    {

        private Lazy<IList<MBData>> LazyStrictData { get; }

        /// <summary>
        /// PCファイルデータ (生)
        /// </summary>
        public IList<MBData> RawData { get; }

        /// <summary>
        /// 厳格なフォーマットに修正した PCファイルデータ
        /// </summary>
        public IList<MBData> StrictData => LazyStrictData.Value;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="raw">生の値</param>
        /// <param name="strict">生の値から厳格なフォーマットに変換する関数</param>
        public PCFileModeData(IList<MBData> raw, Func<IList<MBData>> strict)
        {
            RawData = raw ?? throw new ArgumentNullException(nameof(raw));
            LazyStrictData = new Lazy<IList<MBData>>(strict);
        }
    }
}