using System;
using System.Collections.Generic;

namespace TokyoChokoku.Bppg
{

    /// <summary>
    /// BPPGファイルです。
    /// </summary>
    public interface BPPGFile
    {
        /// <summary>
        /// サポートバージョン定数: BPPG_FIELD 1.0
        /// </summary>
        public static BPPGFormatVersion FormatVersion1_0 { get; } = new BPPGFormatVersion("BPPG_FILE", 1, 0);

        /// <summary>
        /// ファイルのフォーマットバージョンです。
        /// </summary>
        public BPPGFormatVersion FormatVersion { get; }

        /// <summary>
        /// BPPG フィールドリストです。
        /// </summary>
        public IList<BPPGFieldData> FieldList { get; }

    }
}
