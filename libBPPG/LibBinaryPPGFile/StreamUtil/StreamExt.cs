using System;
using System.IO;

namespace TokyoChokoku.Bppg.StreamUtil
{
    internal static class StreamExt
    {
        /// <summary>
        /// ストリームからバイトを取得します。
        /// </summary>
        /// <param name="self">読み取り可能ストリーム</param>
        /// <param name="limitByteSize">最大読み取りバイト数</param>
        /// <returns>読み取り結果 (nonnull) </returns>
        public static StreamDatagram ReadAllBytes(this Stream self, int limitByteSize)
        {
            var bytes = new byte[88 * 2];
            var count = self.Read(bytes);

            return new StreamDatagram(bytes, count);
        }
    }
}
