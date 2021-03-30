using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	namespace AddBytesConverterToByteArray{

		/// <summary>
		/// byte配列を任意のビット数の整数型で取得するメソッドを提供します。
		/// </summary>
		public static class BytesConverter
		{
			//------------------------------------------------------------
			// getter
			//------------------------------------------------------------

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 16ビット整数型に変換して取得します。 
			/// </summary>
			public static Int16 ToInt16(this byte[] bytes, int startIndex)
			{
				return System.BitConverter.ToInt16 (bytes, startIndex);
			}

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 16ビット整数型に変換して取得します。 
			/// </summary>
			public static List<Int16> ToInt16(this byte[] bytes, int startIndex, int size)
			{
				var result = new List<Int16>();
				for (var i = 0; i < size; i++) {
					result.Add (bytes.ToInt16 (startIndex + (i * sizeof(Int16))));
				}
				return result;
			}

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 32ビット整数型に変換して取得します。 
			/// </summary>
			public static int ToInt32(this byte[] bytes, int startIndex)
			{
				return System.BitConverter.ToInt32 (bytes, startIndex);
			}

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 32ビット整数型に変換して取得します。 
			/// </summary>
			public static List<int> ToInt32(this byte[] bytes, int startIndex, int size)
			{
				var result = new List<int>();
				for (var i = 0; i < size; i++) {
					result.Add (bytes.ToInt32(startIndex+ (i * sizeof(int))));
				}
				return result;
			}

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 32ビット浮動小数点数型に変換して取得します。 
			/// </summary>
			public static float ToFloat32(this byte[] bytes, int startIndex)
			{
				return (float)
					(bytes [startIndex] << 24) +
					(bytes [startIndex + 1] << 16) +
					(bytes [startIndex + 2] << 8) +
					(bytes [startIndex + 3]);
			}

			/// <summary>
			/// bytes[startIndex] ~ 指定したサイズ までを 32ビット浮動小数点数型に変換して取得します。 
			/// </summary>
			public static List<float> ToFloat32(this byte[] bytes, int startIndex, int size)
			{
				var result = new List<float>();
				for (var i = 0; i < size && (startIndex + i + 3) < bytes.Length; i++) {
					result.Add (bytes.ToFloat32 (startIndex + i));
				}
				return result;
			}

			//------------------------------------------------------------
			// setter
			//------------------------------------------------------------

			/// <summary>
			/// bytes[startIndex] ~ に 16ビット整数型 を 2バイトに分割して設定します。
			/// </summary>
			public static void SetInt16(this byte[] bytes, Int16 value, int startIndex)
			{
				 byte[] valueBytes = BitConverter.GetBytes (value);
				for (var i = 0; i < 2; i++) {
					bytes [startIndex + i] = valueBytes[(BitConverter.IsLittleEndian ? (1-i) : i)];
				}
			}

			/// <summary>
			/// bytes[startIndex] ~ に 16ビット浮動小数点型 を 4バイトに分割して設定します。
			/// </summary>
			public static void SetFloat16(this byte[] bytes, float value, int startIndex)
			{
				 byte[] valueBytes = BitConverter.GetBytes (value);
				for (var i = 0; i < 2; i++) {
					bytes [startIndex + i] = valueBytes[(BitConverter.IsLittleEndian ? (1-i) : i)];
				}
			}

			/// <summary>
			/// bytes[startIndex] ~ に 32ビット整数型 を 4バイトに分割して設定します。
			/// </summary>
			public static void SetInt32(this byte[] bytes, int value, int startIndex)
			{
				 byte[] valueBytes = BitConverter.GetBytes (value);
				for (var i = 0; i < 4; i++) {
					bytes [startIndex + i] = valueBytes[(BitConverter.IsLittleEndian ? (3-i) : i)];
				}
			}

		}
	}
}

