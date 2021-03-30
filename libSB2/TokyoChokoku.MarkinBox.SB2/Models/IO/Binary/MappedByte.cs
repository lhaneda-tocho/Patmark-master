using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedByte
	{
		private readonly byte[] target;
		private readonly int offset;

		public const int Size = sizeof(byte);
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }

		public byte Value {
			get{
				return target [offset];
			}
			set{
				target [offset] = value;
			}
		}

		public MappedByte (byte[] target, int offset)
		{
			InvalidateToThrow (target, offset);

			this.target = target;
			this.offset = offset;
		}

		public static MappedByte[] newArray (byte[] target, int offset, int number)
		{
			MappedByte[] array = new MappedByte[number];

			for (int i = 0; i < array.Length; i++) {
				array [i] = new MappedByte (target, offset+i);
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

