using System;
using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedString
	{
		private readonly byte[] target;
		private readonly int offset;

		public readonly int Size;
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }


		public String GetAs(TextEncode encode, int charCount)
		{
			String data = encode.GetString(target.Take(Size).ToArray());
			return data.Substring (0, charCount);
		}

		public void SetStringAs(String data, TextEncode encode, int maxCharCount)
		{
			data = (data.Length < maxCharCount) ? data : data.Substring (0, maxCharCount);
			byte[] encoded = encode.GetBytes (data);

			int length;

			if (encoded.Length > Size)
				throw new IndexOutOfRangeException ();
			else
				length = encoded.Length;
			
			Array.ConstrainedCopy (encoded, 0, target, offset, length);
			for (int i = offset+length; i < Size; i++) {
				target [i] = 0x0;
			}

		}


		public MappedString (byte[] target, int offset, int useByteSize)
		{
			InvalidateToThrow (target, offset, useByteSize);

			this.target = target;
			this.offset = offset;
			this.Size = useByteSize;
		}

		public static bool Valid(byte[] target, int offset, int size)
		{
			int remaining = target.Length - offset;

			return size <= remaining;
		}

		private static void InvalidateToThrow(byte[] target, int offset, int size)
		{
			if (!Valid (target, offset, size)) {
				String mes = target.Length + ", " + size + ", " + offset;
				throw new IndexOutOfRangeException (mes);
			}
		}
	}
}

