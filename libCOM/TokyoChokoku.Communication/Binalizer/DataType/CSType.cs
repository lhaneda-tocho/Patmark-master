using System;
using System.Runtime.InteropServices;
using static TokyoChokoku.Communication.BinalizerUtil;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Communication
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Word
	{
		[FieldOffset(0)]
		public UInt16 UInt;

		[FieldOffset(0)]
		public Int16 SInt;

		public Byte[] ToBytesBigEndian()
		{
			return BigEndian.GetBytes(UInt);
		}

		public Byte[] ToBytesLittleEndian()
		{
			return LittleEndian.GetBytes(UInt);
		}

		public static Word Init(UInt16 v)
		{
			var w = new Word();
			w.UInt = v;
			return w;
		}

		public static Word Init(Int16 v)
		{
			var w = new Word();
			w.SInt = v;
			return w;
		}


	}

	[StructLayout(LayoutKind.Explicit)]
	public struct DWord
	{
		[FieldOffset(0)]
		public UInt32 UInt;

		[FieldOffset(0)]
		public Int32 SInt;

		[FieldOffset(0)]
		public Single Float;

		public static DWord Init(UInt32 v)
		{
			var w = new DWord();
			w.UInt = v;
			return w;
		}

		public static DWord Init(Int32 v)
		{
			var w = new DWord();
			w.SInt = v;
			return w;
		}

		public static DWord Init(Single v)
		{
			var w = new DWord();
			w.Float = v;
			return w;
		}
	}
}
