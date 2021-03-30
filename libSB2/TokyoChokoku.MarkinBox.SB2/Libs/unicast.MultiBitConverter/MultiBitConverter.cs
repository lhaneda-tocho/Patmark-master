using System;


namespace TokyoChokoku.MarkinBox.Sketchbook
{
	


	public sealed class MultiBitConverter
	{

		private readonly Endian mode;

		public Endian Mode
		{
			get{ return mode; }
		}


		public static readonly MultiBitConverter Little = new MultiBitConverter(Endian.LITTLE);

		public static readonly MultiBitConverter Big = new MultiBitConverter(Endian.BIG);


		private MultiBitConverter (Endian endian)
		{
			mode=endian;
		}



		public Int16  ToInt16(Byte[] raw, int offset)
		{
			Word tmp = default(Word);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.Int16;
		}

		public void DestinateInt16(Byte[] dest, int offset, Int16 src)
		{
			Word tmp = default(Word);
			tmp.Int16 = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}

		public UInt16 ToUInt16(Byte[] raw, int offset)
		{
			Word tmp = default(Word);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.UInt16;
		}

		public void DestinateUInt16(Byte[] dest, int offset, UInt16 src)
		{
			Word tmp = default(Word);
			tmp.UInt16 = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}

		public UInt16 ToChar(Byte[] raw, int offset)
		{
			Word tmp = default(Word);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.Char;
		}

		public void DestinateToChar(Byte[] dest, int offset, Char src)
		{
			Word tmp = default(Word);
			tmp.Char = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}

		public Int32  ToInt32(Byte[] raw, int offset)
		{
			DWord tmp = default(DWord);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.Int32;
		}

		public void DestinateInt32(Byte[] dest, int offset, Int32 src)
		{
			DWord tmp = default(DWord);
			tmp.Int32 = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}

		public UInt32 ToUInt32(Byte[] raw, int offset)
		{
			DWord tmp = default(DWord);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.UInt32;
		}

		public void DestinateUInt32(Byte[] dest, int offset, UInt32 src)
		{
			DWord tmp = default(DWord);
			tmp.UInt32 = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}

		public float ToFloat32(Byte[] raw, int offset)
		{
			DWord tmp = default(DWord);

			if (mode == Endian.LITTLE)
				tmp.SetLittleEndian (raw, offset);
			else
				tmp.SetBigEndian (raw, offset);

			return tmp.Float;
		}

		public void DestinateFloat32(Byte[] dest, int offset, float src)
		{
			DWord tmp = default(DWord);
			tmp.Float = src; 

			if (mode == Endian.LITTLE)
				tmp.DestinateAsLittleEndian (dest, offset);
			else
				tmp.DestinateAsBigEndian (dest, offset);
		}
	}
}

