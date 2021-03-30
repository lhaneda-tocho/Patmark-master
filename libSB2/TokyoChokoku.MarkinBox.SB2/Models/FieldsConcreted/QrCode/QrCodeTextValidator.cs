using System;
using System.Text.RegularExpressions;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public abstract class QrCodeTextValidator : TextValidator
    {
        static readonly string regex = @"[^0-9A-Z \$\%\*\+\-\.\/\:]";

        protected abstract string Text { get; }

        public static QrCodeTextValidator CreateOfQrCode (IBaseQrCodeParameter param)
        {
            return new OfQrCodeParameter (param);
        }

        public ValidationResult ValidateText (string text)
        {
            var collection = Regex.Matches (text, regex);
            if (collection.Count != 0) {
                var builder = ValidationResult.CreateBuilder ();
                builder.PutError (
                    ValidationCategory.Text,
                    ValidationError.QrCodeTextInvalidCharacter,
                    "Invalid Characters.");
                return builder.Create ();
            }
            if (text.Length > 80) {
                var builder = ValidationResult.CreateBuilder ();
                builder.PutError (
                    ValidationCategory.Text,
                    ValidationError.QrCodeTextLengthOutOfRange,
                    "Character counts must be less than 80.");
                return builder.Create ();
            }
            return ValidationResult.Empty;
        }

        public ValidationResult ValidateText ()
        {
            return ValidateText (Text);
        }

        class OfQrCodeParameter : QrCodeTextValidator
        {
            IBaseQrCodeParameter p;

            public OfQrCodeParameter (IBaseQrCodeParameter p)
            {
                this.p = p;
            }

            protected override string Text {
                get {
                    return p.Text;
                }
            }
        }
    }
}

