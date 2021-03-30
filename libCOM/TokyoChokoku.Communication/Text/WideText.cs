using System;
using System.Linq;
using NativeCnv = System.BitConverter;
using TokyoChokoku.Communication;
using static BitConverter.EndianBitConverter;
using System.Collections;
using System.Collections.Generic;

using TokyoChokoku.Text;

namespace TokyoChokoku.Communication.Text
{
	/// <summary>
	/// 打刻機で扱う 2バイト文字列
	/// </summary>
    public class WideText: IEnumerable<Word>
    {
		static TextEncodings Encoding = TextEncodings.Byte2;
		Word[] Array;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { 
            get
            {
                return Array.Length;
            }
        }

        public WideText(Word[] array)
		{
			Array  = (Word[])array.Clone();
		}

		/// <summary>
		/// C# で扱う文字列に変換します
		/// </summary>
		public override string ToString()
		{
            var bytes = Array.SelectMany(
                (v) => BigEndian.GetBytes(v)
            ).ToArray();
			return Encoding.GetString(bytes);
		}

		/// <summary>
		/// 文字列を指定して MonoByteText を初期化します．
		/// shift_jis コードで扱えない文字は正しく変換できません．
		/// </summary>
		/// <returns>The encode.</returns>
		/// <param name="text">text.</param>
        public static WideText Encode(string text)
		{
			// shift_jis -> エンディアンなし
			// 今回はBigEndian で扱う
			var bytes = Encoding.GetBytes(text);
            var words = bytes.DiPack().Select(
                (v) =>
                {
                    return BigEndian.ToWord(v);
                }
            ).ToArray();
			return new WideText(words);
		}

        public IEnumerator<Word> GetEnumerator() {
            return Array.Cast<Word>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public WideText Substring(int start, int endExclusive)
        {
            var newSize = endExclusive - start;
            var newWords = new Word[newSize];
            System.Array.Copy(Array, start, newWords, 0, newSize);
            return new WideText(newWords);
        }
    }
}
