using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using TokyoChokoku.Text;
namespace TokyoChokoku.Communication.Text
{

    /// <summary>
    /// 打刻機で扱う 1バイト文字列
    /// </summary>
    public class MonoByteText: IEnumerable<Byte>
    {
        static TextEncodings Encoding = TextEncodings.Byte1;
        Byte[] Array;

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				return Array.Length;
			}
		}

        public MonoByteText(Byte[] array)
        {
            Array = (Byte[])array.Clone();
        }

        /// <summary>
        /// C# で扱う文字列に変換します
        /// </summary>
        public override string ToString()
        {
            return Encoding.GetString(Array);
        }

		/// <summary>
		/// 文字列を指定して MonoByteText を初期化します．
		/// Ascii コードで扱えない文字は正しく変換できません．
		/// </summary>
		/// <returns>The encode.</returns>
		/// <param name="text">text.</param>
		public static MonoByteText Encode(string text)
        {
            var bytes = Encoding.GetBytes(text);
            return new MonoByteText(bytes);
        }

        public IEnumerator<Byte> GetEnumerator() 
        {
            return Array.Cast<Byte>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public MonoByteText Substring(int start, int endExclusive)
		{
			var newSize = endExclusive - start;
			var newBytes = new Byte[newSize];
			System.Array.Copy(Array, start, newBytes, 0, newSize);
			return new MonoByteText(newBytes);
		}
    }
}
