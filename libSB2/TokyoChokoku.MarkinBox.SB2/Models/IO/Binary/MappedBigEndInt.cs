using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedBigEndInt
	{
		private readonly byte[] target;
		private readonly int offset;

		public const int Size = sizeof(int);
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }

		public int Value {
			get{
				return MultiBitConverter.Big.ToInt32 (target, offset);
			}
			set{
				MultiBitConverter.Big.DestinateInt32 (target, offset, value);
			}
		}

        public MappedBigEndInt (byte[] target, int offset)
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

