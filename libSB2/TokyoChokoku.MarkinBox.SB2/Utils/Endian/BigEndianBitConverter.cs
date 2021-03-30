using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	
    /// <summary>
    /// Big endian なバイト列を C# のデータ構造に変換するクラス
    /// </summary>
	public static class BigEndianBitConverter
	{
		private enum Endian
		{
			Little,
			Big
		}

		const Endian MyEndian = Endian.Big;

		private static BigEndianBytes ConvertBytesWithEndian(IEnumerable<byte> bytes, Endian endian)
		{
			if (BitConverter.IsLittleEndian ^ endian == Endian.Little)
				return new BigEndianBytes (bytes.Reverse ());
			else
				return new BigEndianBytes (bytes);
		}

        /// <summary>
        /// Byte to Byte. 何もせず通します．
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(byte value){
			return ConvertBytesWithEndian(new byte[]{value}, MyEndian);
		}

        /// <summary>
        /// Byte to Boolean
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">If set to <c>true</c> value.</param>
		public static BigEndianBytes GetBytes(bool value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 4 Byte (Big end) to uint 
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(uint value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 2 Byte (Big end) to ushort
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(ushort value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 8 Byte (Big end) to ulong
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(ulong value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 2 Byte (Big end) to short
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(short value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 4 Byte (Big end) to int
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(int value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 8 Byte (Big end) to long
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(long value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 4 Byte (Big end) to float
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(float value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 8 Byte (Big end) to double
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(double value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// 2 Byte (Big end) to Char
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="value">Value.</param>
		public static BigEndianBytes GetBytes(char value){
			return ConvertBytesWithEndian(BitConverter.GetBytes(value), MyEndian);
		}

        /// <summary>
        /// BigEndianBytes to Boolean
        /// </summary>
        /// <returns><c>true</c>, if boolean was toed, <c>false</c> otherwise.</returns>
        /// <param name="bytes">Bytes.</param>
		public static bool ToBoolean(BigEndianBytes bytes){
			return BitConverter.ToBoolean(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to Char
        /// </summary>
        /// <returns>The char.</returns>
        /// <param name="bytes">Bytes.</param>
		public static char ToChar(BigEndianBytes bytes){
			return BitConverter.ToChar(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to String
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="bytes">Bytes.</param>
		public static string ToString(BigEndianBytes bytes){
			return BitConverter.ToString(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to short
        /// </summary>
        /// <returns>The short.</returns>
        /// <param name="bytes">Bytes.</param>
		public static short ToShort(BigEndianBytes bytes){
			return BitConverter.ToInt16(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to int
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="bytes">Bytes.</param>
		public static int ToInt(BigEndianBytes bytes){
			return BitConverter.ToInt32(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to long
        /// </summary>
        /// <returns>The long.</returns>
        /// <param name="bytes">Bytes.</param>
		public static long ToLong(BigEndianBytes bytes){
			return BitConverter.ToInt64(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to float
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="bytes">Bytes.</param>
		public static float ToFloat(BigEndianBytes bytes){
			return BitConverter.ToSingle(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to double
        /// </summary>
        /// <returns>The double.</returns>
        /// <param name="bytes">Bytes.</param>
		public static double ToDouble(BigEndianBytes bytes){
			return BitConverter.ToDouble(bytes.ToSystemEndian().ToArray(), 0);
		}
	
        /// <summary>
        /// BigEndianBytes to ushort
        /// </summary>
        /// <returns>The US hort.</returns>
        /// <param name="bytes">Bytes.</param>
		public static ushort ToUShort(BigEndianBytes bytes){
			return BitConverter.ToUInt16(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// BigEndianBytes to uint
        /// </summary>
        /// <returns>The user interface nt.</returns>
        /// <param name="bytes">Bytes.</param>
		public static uint ToUInt(BigEndianBytes bytes){
			return BitConverter.ToUInt32(bytes.ToSystemEndian().ToArray(), 0);
		}

        /// <summary>
        /// bytes to char list
        /// </summary>
        /// <returns>The chars.</returns>
        /// <param name="bytes">Bytes.</param>
		public static List<char> ToChars(List<byte> bytes){
            var res = new List<char> ();
            for(var i = 0; i < bytes.Count; i += 2){
                res.Add (
                    ToChar(new BigEndianBytes (bytes.Skip (i).Take (2)))
                );
            }
            return res;
		}

        /// <summary>
        /// bytes the short list.
        /// </summary>
        /// <returns>The shorts.</returns>
        /// <param name="bytes">Bytes.</param>
        public static List<short> ToShorts(List<byte> bytes){
            var res = new List<short> ();
            for(var i = 0; i < bytes.Count; i += 2){
                res.Add (
                    ToShort(new BigEndianBytes (bytes.Skip (i).Take (2)))
                );
            }
            return res;
        }

        /// <summary>
        /// float map to byte list.
        /// </summary>
        /// <param name="endBytes">End bytes.</param>
        public static List<byte> Bond(List<BigEndianBytes> endBytes)
        {
            var bytes = new List<byte>();
            foreach (var endByte in endBytes)
            {
                bytes.AddRange(endByte.ToArray());
            }
            return bytes;
        }

        /// <summary>
        /// Binarizes the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="text">Text.</param>
        /// <param name="encode">Encode.</param>
        /// <param name="textLength">Text length.</param>
        public static byte[] BinarizeText(string text, TextEncode encode, int textLength)
        {
            var binSize = textLength * (encode == TextEncode.Byte2 ? 2 : 1);
            var dataBin = new byte[binSize];
            var mappedText = new MappedString(dataBin, 0, binSize);
            mappedText.SetStringAs(text, encode, textLength);

            Log.Debug(String.Format("[BigEndianBitConverter] 文字列「{0}」をバイナリに変換しました ... {1}", text, BitConverter.ToString(dataBin)));

            return dataBin;
        }

        /// <summary>
        /// Binarizes the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="text">Text.</param>
        /// <param name="encode">Encode.</param>
        /// <param name="textLength">Text length.</param>
        public static string DecodeText (byte [] dataBin, TextEncode encode)
        {
            return encode.GetString (dataBin);
        }

        public static byte[] ResponseCharArrayToByteArray (byte[] response)
        {
            byte [] byteArray = new byte [response.Count() / 2];
            for (int i = 1; i < response.Count(); i += 2)
                byteArray [i / 2] = response [i];
            return byteArray;
        }
	}
}

