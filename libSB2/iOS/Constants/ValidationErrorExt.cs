using static TokyoChokoku.MarkinBox.Sketchbook.ValidationError;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// ValidationError の拡張クラス
    /// </summary>
    public static class ValidationErrorExt
    {
        public static string GetLocalizationId (this ValidationError error)
        {
            switch (error) {
            default:
                return "validation.undefined";

            case QrCodeTextInvalidCharacter:
                return "validation.qr-code.invalid-character";
            case QrCodeTextLengthOutOfRange:
                return "validation.qr-code.text-length-out-of-range";

            case DataMatrixTextInvalidCharacter:
                return "validation.data-matrix.invalid-character";
            case DataMatrixTextLengthOutOfRange:
                return "validation.data-matrix.text-length-out-of-range";

            case TextLengthOutOfRange:
                return "validation.common.text-length-out-of-range";
            }
        }
    }
}