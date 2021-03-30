using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class FontModeExt
    {
        public static string GetLocalizationId (this FontMode mode)
        {
            switch (mode) {
            case FontMode.FontTC:
                return "field-font.tc-font";
            case FontMode.FontBarcode:
                return "field-font.barcode";
            case FontMode.Font5x7Dot:
                return "field-font.5x7-font";
            case FontMode.FontPC:
                return "field-font.pc-font";
            default:
                throw new ArgumentOutOfRangeException ();
            }
        }
    }
}

