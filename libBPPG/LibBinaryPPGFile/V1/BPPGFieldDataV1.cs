using System;
namespace TokyoChokoku.Bppg.V1
{
    /// <summary>
    /// 初期に定義されたフィールドデータです。
    /// </summary>
    public class BPPGFieldDataV1 : BPPGFieldData
    {
        /// <summary>
        /// フィールドサイズは 88 word で固定する。
        /// </summary>
        const int MAX_MIN_SIZE_BYTE = 88 * 2;


        public BPPGFormatVersion FormatVersion => BPPGFieldData.FormatVersion1_0;

        public int ByteSize => MAX_MIN_SIZE_BYTE;

        public byte[] Data =>   (byte[]) data.Clone();

        // ファイルから読み取った 88word 分のデータを格納する。
        readonly byte[] data;

        /// <summary>
        /// 88word 分のバイナリデータを指定することで初期化します。
        /// 新しいオブジェクトには、引数の配列をコピーした物が渡されます。
        /// </summary>
        /// <param name="binary">バイナリ配列。 88word サイズのみ受け付けます。</param>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数のバイナリサイズが異常値の時</exception>
        public BPPGFieldDataV1(byte[] binary)
        {
            binary = binary ?? throw new ArgumentNullException(nameof(binary));
            if (binary.Length != MAX_MIN_SIZE_BYTE)
                throw new ArgumentException($"フィールドバイナリサイズは {MAX_MIN_SIZE_BYTE} [Byte] である必要があります。");
            data = (byte[]) binary.Clone();
        }

    }
}
