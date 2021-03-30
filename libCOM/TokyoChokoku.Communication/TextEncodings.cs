using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace TokyoChokoku.Text
{
	public enum TextEncodings
	{
		Byte1, // R:word
		Byte2, // D:word
	}

	public static class TextEncodingsExt{



		static readonly Encoding Encoding1Byte = Encoding.GetEncoding ("ascii");
		static readonly Encoding Encoding2Byte = Encoding.GetEncoding ("shift_jis",
                                                                       new EncoderReplacementFallback("?"),
                                                                       DecoderFallback.ReplacementFallback);
        public static readonly Encoding ASCII  = Encoding.GetEncoding("ascii");
        public static readonly Encoding MS_DOS = Encoding.GetEncoding("shift_jis");

		public static Encoding GetEncoding(this TextEncodings encode){
			switch (encode) {
			case TextEncodings.Byte1:
				return Encoding1Byte;
			case TextEncodings.Byte2:
				return Encoding2Byte;
			default:
				throw new ArgumentOutOfRangeException("TextEncodeExt.GetEncoding - ケース設定に漏れがあります。");
			}
		}

        public static bool CompatByte2(char c) {
            if (c == '?')
                return true;
            if (c == '\\')
                return false;
            var bytes = Encoding2Byte.GetBytes(c.ToString());

            //{
            //    for (int i = 0; i < bytes.Length; i++) {
            //        Console.Write(bytes[i].ToString("X"));
            //        Console.Write(' ');
            //    }
            //    Console.WriteLine();
            //}

            if (bytes.Length != 1)
                return true;
            else
            {
                int first = ((int)bytes[0]) & 0xFF;
                var cond = 0x3F != first;
                return cond;
            }
        }

        /// <summary>
        /// 文字列をバイト配列に変換します．
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="encode">Encode.</param>
        /// <param name="text">Text.</param>
		public static byte[] GetBytes(this TextEncodings encode, string text){
            
			switch (encode) {
			case TextEncodings.Byte1:
				{
					return encode.GetEncoding ().GetBytes (text);
				}
			case TextEncodings.Byte2:
				{
					var encoded = new List<byte>();
                    var entry = new char[] { '\0' };
					foreach (var t in text) {
                        entry[0] = t;
                        var bytes = encode.GetEncoding().GetBytes (entry);
						if (bytes.Length == 1) {
							encoded.Add (0x00);
						}
						encoded.AddRange (bytes);
					}
					return encoded.ToArray();
				}
			default:
				throw new ArgumentOutOfRangeException ("TextEncodeExt.GetBytes - ケース設定に漏れがあります。");
			}
		}

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="encode">Encode.</param>
        /// <param name="textBytes">Text bytes.</param>
		public static string GetString(this TextEncodings encode, byte[] textBytes){

			switch (encode) {
			case TextEncodings.Byte1:
				{
					return encode.GetEncoding ().GetString (textBytes, 0, textBytes.Length);
				}
			case TextEncodings.Byte2:
				{
					var decoded = new List<byte> ();
					for (var i = 0; (i + 1) < textBytes.Length; i += 2) {
						if (textBytes [i] == 0x00) {
							decoded.Add (textBytes [i + 1]);
						} else {
							decoded.Add (textBytes [i]);
							decoded.Add (textBytes [i + 1]);
						}
					}
					return encode.GetEncoding ().GetString (decoded.ToArray(), 0, decoded.Count);
				}
			default:
				throw new ArgumentOutOfRangeException ("TextEncodeExt.GetString - ケース設定に漏れがあります。");
			}
		}

        public static string GetStringFromNoStride(this TextEncodings encode, byte[] text)
        {
            return encode.GetEncoding().GetString(text);
        }

        public static byte[] GetBytesNoStride(this TextEncodings encode, string text, int maxByteSize)
        {
            int byteSize = maxByteSize;

            // サイズが0の時は直ちに終了
            if (byteSize == 0)
                return new byte[0];
            // 負の場合はエラー
            else if (byteSize < 0)
                throw new ArgumentOutOfRangeException("byteSize property must be >= 0");
            
            switch (encode) {
                case TextEncodings.Byte1:
                    return encode.GetEncoding().GetBytes(text).Take(byteSize).ToArray();
                case TextEncodings.Byte2:
                    {
                        var encoded = new List<byte>(byteSize);
                        var store = new char[1];
                        foreach (var t in text)
                        {
                            // 1文字エンコード
                            store[0] = t;
                            var bytes = encode.GetEncoding().GetBytes(store);
                            // 現在のサイズ計算
                            var csize = encoded.Count;
                            // はみ出し判定
                            if (byteSize - csize >= bytes.Length)
                            {
                                // はみ出さない場合 (式の導出は以下の通り. オーバーフロー防止のため，引き算を利用している.)
                                // (要求サイズ)                        >= (現在のサイズ + 追加されるデータのサイズ)
                                // (要求サイズ - 追加されるデータのサイズ) >= (現在のサイズ)
                                encoded.AddRange(bytes);
                            } else {
                                break;
                            }
                        }
                        while(encoded.Count < maxByteSize) {
                            encoded.Add(0);
                        }
                        return encoded.ToArray();
                    }
                default:
                    throw new ArgumentOutOfRangeException("TextEncodeExt.GetString - ケース設定に漏れがあります。");
            }
        }

        public static byte[] GetBytesNoStride(this TextEncodings encode, string text)
        {
            return encode.GetEncoding().GetBytes(text);
        }
	}

}

