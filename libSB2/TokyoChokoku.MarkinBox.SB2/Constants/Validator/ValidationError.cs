using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public enum ValidationError
    {
        QrCodeTextInvalidCharacter,
        DataMatrixTextInvalidCharacter,

        QrCodeTextLengthOutOfRange,
        DataMatrixTextLengthOutOfRange,
        TextLengthOutOfRange,

        Undefined
    }
}

