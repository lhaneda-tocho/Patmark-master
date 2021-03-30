using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public enum FontMode : short
    {
        FontTC,
        FontBarcode,
        Font5x7Dot,
        FontPC
    }

    public static class FontModeExt
    {
        public static FontMode ValueOf (short value)
        {
            switch (value) {
            case   0:
                return FontMode.FontTC;
            case  50:
                return FontMode.FontBarcode;
            case  64:
                return FontMode.Font5x7Dot;
            case 128:
                return FontMode.FontPC;
            default:
                return FontMode.FontTC;
            }
        }

        public static short ToPrmFl (this FontMode value)
        {
            switch (value) {
            case FontMode.FontTC:
                return 0;
            case FontMode.FontBarcode:
                return 50;
            case FontMode.Font5x7Dot:
                return 64;
            case FontMode.FontPC:
                return 128;
            default:
                throw new ArgumentOutOfRangeException ();
            }
        }

        public static short ToPpgParameterFlag (this FontMode value)
        {
            switch (value) {
            case FontMode.FontTC:
                return 0;
            case FontMode.FontBarcode:
                return 0;
            case FontMode.Font5x7Dot:
                return 1;
            case FontMode.FontPC:
                return 4;
            default:
                throw new ArgumentOutOfRangeException ();
            }
        }
    }
}

