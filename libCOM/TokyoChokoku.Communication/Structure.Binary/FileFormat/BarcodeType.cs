using System;

namespace TokyoChokoku.Structure.Binary.FileFormat
{
    public enum BarcodeType : byte
    {
        DataMatrix = 0,
        QrCode     = 1
    }

    public static class BarcodeTypeExt
    {
        public static bool IsDefined(byte i)
        {
            return Enum.IsDefined ( typeof(BarcodeTypeExt), i );
        }

        public  static String GetName (this BarcodeType type)
        {
            return Enum.GetName (typeof(BarcodeTypeExt), (int)type);
        }
    }
}

