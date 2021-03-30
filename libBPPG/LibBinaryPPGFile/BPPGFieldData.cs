using System;
namespace TokyoChokoku.Bppg
{
    /// <summary>
    /// フィールドのバイナリ表現です。データはビッグエンディアンで格納されます。
    /// </summary>
    public interface BPPGFieldData
    {
        /// <summary>
        /// サポートバージョン定数: BPPG_FIELD 1.0
        /// </summary>
        public static BPPGFormatVersion FormatVersion1_0 { get; } = new BPPGFormatVersion("BPPG_FIELD", 1, 0);

        /// <summary> 
        /// フォーマットバージョン
        /// </summary>
        BPPGFormatVersion FormatVersion { get; }

        /// <summary>
        /// データサイズ (バイト)
        /// </summary>
        public int ByteSize { get; }

        /// <summary>
        /// バイナリデータ
        /// </summary>
        public byte[] Data { get; }
    }
}
