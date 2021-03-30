using System;
using System.Runtime.InteropServices;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Word{
		[FieldOffset(0)]
		public Int16  Int16;

		[FieldOffset(0)]
		public UInt16 UInt16;

		[FieldOffset(0)]
		public Char Char;

		[FieldOffset(0)]
		public byte Byte0;

		[FieldOffset(1)]
		public byte Byte1;



		public void SetBytes(Byte[] raw, int offset)
		{
			Byte0 = raw [offset + 0];
			Byte1 = raw [offset + 1];
		}

		public void SetBytesReverse(Byte[] raw, int offset)
		{
			Byte0 = raw [offset + 1];
			Byte1 = raw [offset + 0];
		}

		public void DestinateBytes(Byte[] dist, int offset)
		{
			dist [offset + 0] = Byte0;
			dist [offset + 1] = Byte1;
		}

		public void DestinateBytesReverse(Byte[] dist, int offset)
		{
			dist [offset + 0] = Byte1;
			dist [offset + 1] = Byte0;
		}

		public void SetBigEndian(Byte[] raw, int offset)
		{
			if (BitConverter.IsLittleEndian)
				SetBytesReverse (raw, offset);
			else
				SetBytes (raw, offset);
		}

		public void SetLittleEndian(Byte[] raw, int offset)
		{
			if (BitConverter.IsLittleEndian)
				SetBytes (raw, offset);
			else
				SetBytesReverse (raw, offset);
		}

		// TODO: テストしておくこと
		public void DestinateAsLittleEndian(Byte[] dist, int offset)
		{
			if (BitConverter.IsLittleEndian)
				DestinateBytes (dist, offset);
			else
				DestinateBytesReverse (dist, offset);
		}

		// TODO: テストしておくこと
		public void DestinateAsBigEndian(Byte[] dist, int offset)
		{
			if (BitConverter.IsLittleEndian)
				DestinateBytesReverse (dist, offset);
			else
				DestinateBytes (dist, offset);
		}
	}
}

