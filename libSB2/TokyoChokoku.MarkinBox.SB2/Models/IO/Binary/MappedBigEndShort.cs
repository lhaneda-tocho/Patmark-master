using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedBigEndShort
	{
		private readonly byte[] target;
		private readonly int offset;

		public const int Size = sizeof(short);
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }

		public short Value {
			get{
				return MultiBitConverter.Big.ToInt16 (target, offset);
			}
			set{
				MultiBitConverter.Big.DestinateInt16 (target, offset, value);
			}
		}

		public MappedBigEndShort (byte[] target, int offset)
		{
			InvalidateToThrow (target, offset);

			this.target = target;
			this.offset = offset;
		}

		public static MappedBigEndShort[] newArray (byte[] target, int offset, int number)
		{
			MappedBigEndShort[] array = new MappedBigEndShort[number];

			for (int i = 0; i < array.Length; i++) {
				array [i] = new MappedBigEndShort (target, offset+i);
			}

			return array;
		}

		public static bool Valid(byte[] target, int offset)
		{
			int remaining = target.Length - offset;

			return Size <= remaining;
		}

		private static void InvalidateToThrow(byte[] target, int offset)
		{
			if (!Valid (target, offset)) {
				String mes = "" + (target.Length + Size - 1);
				throw new IndexOutOfRangeException (mes);
			}
		}

	}
}

