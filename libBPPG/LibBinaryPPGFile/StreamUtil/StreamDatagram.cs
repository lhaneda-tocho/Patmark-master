using System;

namespace TokyoChokoku.Bppg.StreamUtil
{
    /// <summary>
    /// ストリームから読み込まれたデータを表します。 
    /// </summary>
    internal class StreamDatagram
    {
        /// <summary>
        /// バイト数です. -1  の場合は EOF に到達したことを表します。
        /// </summary>
        public int    ByteCount { get; }

        /// <summary>
        /// データが格納されているバッファです。
        /// </summary>
        public byte[] Buffer    { get; }

        /// <summary>
        /// 読み取れたデータがない場合は true  を返します。
        /// EOF の場合も true を返します。
        /// </summary>
        public bool IsEmpty => ByteCount == 0 || IsEOF;

        /// <summary>
        /// EOF に到達したことを表す場合は true です。
        /// </summary>
        public bool IsEOF => ByteCount == -1;

        public StreamDatagram(byte[] buffer, int count)
        {
            buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException($"{nameof(count)} should be >= 0 && <= {nameof(buffer)}.Length.");

            ByteCount = count;
            Buffer = buffer;
        }
    }
}
