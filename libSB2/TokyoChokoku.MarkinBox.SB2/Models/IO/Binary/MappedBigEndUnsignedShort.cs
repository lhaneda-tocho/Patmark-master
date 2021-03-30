using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedBigEndUnsignedShort
	{
		private readonly byte[] target;
		private readonly int offset;

		public const int Size = sizeof(ushort);
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }

		public ushort Value {
			get{
				return MultiBitConverter.Big.ToUInt16 (target, offset);
			}
			set{
				MultiBitConverter.Big.DestinateUInt16 (target, offset, value);
			}
		}

		public MappedBigEndUnsignedShort (byte[] target, int offset)
		{
			InvalidateToThrow (target, offset);

			this.target = target;
			this.offset = offset;
		}

		public static MappedBigEndUnsignedShort[] newArray (byte[] target, int offset, int number)
		{
			MappedBigEndUnsignedShort[] array = new MappedBigEndUnsignedShort[number];

			for (int i = 0; i < array.Length; i++) {
				array [i] = new MappedBigEndUnsignedShort (target, offset+i);
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

