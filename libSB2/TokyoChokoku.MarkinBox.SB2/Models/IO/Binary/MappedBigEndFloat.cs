using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class MappedBigEndFloat
	{
		private readonly byte[] target;
		private readonly int offset;

		public const int Size = sizeof(float);
		public int Offset{ get{ return offset; } }
		public int NextOffset{ get{ return offset + Size; } }

		public float Value {
			get{
				return MultiBitConverter.Big.ToFloat32(target, offset);
			}
			set{
				MultiBitConverter.Big.DestinateFloat32 (target, offset, value);
			}
		}

		public MappedBigEndFloat (byte[] target, int offset)
		{
			InvalidateToThrow (target, offset);

			this.target = target;
			this.offset = offset;
		}

		public static MappedBigEndFloat[] NewArray (byte[] target, int offset, int number)
		{
			MappedBigEndFloat[] array = new MappedBigEndFloat[number];

			for (int i = 0; i < array.Length; i++) {
				array [i] = new MappedBigEndFloat (target, offset + i*Size);
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

