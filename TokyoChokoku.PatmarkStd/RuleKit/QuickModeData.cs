/// @file QuickModeData.cs
/// @brief クラス定義ファイル

using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 検証対象のクイックモードデータを表します。
    /// </summary>
    public class QuickModeData
    {
        private Lazy<IList<MBData>> LazySerialized { get; }

        /// <summary>
        /// 打刻データ
        /// </summary>
        public EmbossmentData Data { get; }

        /// <summary>
        /// シリアライズ可能な形式
        /// </summary>
        public IList<MBData> Serialized => LazySerialized.Value;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">打刻データです。</param>
        /// <param name="serialized"><c>data</c>からシリアライズ可能な形式に変換するメソッド</param>
        public QuickModeData(EmbossmentData data, Func<IList<MBData>> serialized)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            LazySerialized = new Lazy<IList<MBData>>(
                serialized ?? throw new ArgumentNullException(nameof(serialized))
            );
        }
    }
}