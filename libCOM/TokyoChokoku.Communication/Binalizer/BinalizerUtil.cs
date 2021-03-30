using System;
using System.Linq;
using BitConverter;

namespace TokyoChokoku.Communication
{
    public static class BinalizerUtil
    {
        public static string NativeEndian() {
			if (System.BitConverter.IsLittleEndian)
			{
                return "LittleEndian";
			}
			else
			{
                return "BigEndian";
			}
        }

        public static EndianBitConverter NativeBitConverter()
		{
            if (System.BitConverter.IsLittleEndian) {
                return EndianBitConverter.LittleEndian;
            } else {
                return EndianBitConverter.BigEndian;
            }
		}

        public static void DumpBytes(byte[] array)
        {
            Console.WriteLine(BytesToString(array));
        }

        public static string BytesToString(byte[] array)
		{
			if (array == null)
			{
                return "null";
			}
            return string.Join("-",
                from i in array
                select i.ToString("X2")
            );
        }
    }

	
}