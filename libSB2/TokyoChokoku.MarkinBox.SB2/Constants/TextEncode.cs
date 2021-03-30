using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public enum TextEncode
	{
		Byte1, // R:word
		Byte2, // D:word
	}

	public static class TextEncodeExt{

		private static readonly System.Text.Encoding Encoding1Byte = System.Text.Encoding.GetEncoding ("ascii");
        private static readonly System.Text.Encoding Encoding2Byte = System.Text.Encoding.GetEncoding ("shift_jis");
				
		public static System.Text.Encoding GetEncoding(this TextEncode encode){
			switch (encode) {
			case TextEncode.Byte1:
				return Encoding1Byte;
			case TextEncode.Byte2:
				return Encoding2Byte;
			default:
				throw new ArgumentOutOfRangeException("TextEncodeExt.GetEncoding - ケース設定に漏れがあります。");
			}
		}

		public static byte[] GetBytes(this TextEncode encode, string text){

			switch (encode) {
			case TextEncode.Byte1:
				{
					return encode.GetEncoding ().GetBytes (text);
				}
			case TextEncode.Byte2:
				{
					var encoded = new List<byte> ();
					foreach (var t in text) {
						var bytes = encode.GetEncoding ().GetBytes (new char[]{ t });
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

		public static string GetString(this TextEncode encode, byte[] textBytes){

			switch (encode) {
			case TextEncode.Byte1:
				{
					return encode.GetEncoding ().GetString (textBytes, 0, textBytes.Length);
				}
			case TextEncode.Byte2:
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
	}

}

