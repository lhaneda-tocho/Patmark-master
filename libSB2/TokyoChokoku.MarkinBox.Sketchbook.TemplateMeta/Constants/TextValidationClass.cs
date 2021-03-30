using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public enum TextValidationClass
    {
        Markup,
        QrCode,
        DataMatrix
    }

    public static class TextValidationClassExt {

        public static string GetName (this TextValidationClass me)
        {
            switch (me) {
            case TextValidationClass.Markup:
                return "Markup" + ValidationCategory.Text.Suffix ();

            case TextValidationClass.QrCode:
                return "QrCode" + ValidationCategory.Text.Suffix ();

            case TextValidationClass.DataMatrix:
                return "DataMatrix" + ValidationCategory.Text.Suffix ();

            default:
                throw new ArgumentOutOfRangeException ();
            }
        }
    }
}

