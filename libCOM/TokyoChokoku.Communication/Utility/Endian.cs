using System;
using BitConverter;
using static BitConverter.EndianBitConverter;
namespace TokyoChokoku.Communication
{
    public enum Endian
    {
        Big, Little
    }


	/// <summary>
	/// ProgrammerSideEndianに対応したBitConverterを返します．
	/// </summary>
	/// <returns>The bit converter.</returns>
	public static class EndianExt {
        public static EndianBitConverter GetConverter(this Endian self) {
            switch (self)
			{
				case Endian.Big:
					return BigEndian;
				case Endian.Little:
					return LittleEndian;
                default:
                    throw new ArgumentOutOfRangeException();
			}
        }
    }
}
