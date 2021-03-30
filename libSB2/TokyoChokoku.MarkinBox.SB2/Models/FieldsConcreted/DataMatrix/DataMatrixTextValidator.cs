using System;
using System.Text.RegularExpressions;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{

    public abstract class DataMatrixTextValidator : TextValidator
    {
        static readonly string regex = @"[^0-9a-zA-Z !-/:-@≠\[-`{-~]";
        protected abstract string Text { get; }


        public static DataMatrixTextValidator CreateOfDataMatrix (IBaseDataMatrixParameter param)
        {
            return new OfDataMatrixParameter (param);
        }

        public ValidationResult ValidateText (string text)
        {
            var collection = Regex.Matches (text, regex);
            if (collection.Count != 0) {
                var builder = ValidationResult.CreateBuilder ();
                builder.PutError (
                    ValidationCategory.Text,
                    ValidationError.DataMatrixTextInvalidCharacter,
                    "Invalid Characters.");
                return builder.Create ();
            }
            if (text.Length > 80) {
                var builder = ValidationResult.CreateBuilder ();
                builder.PutError (
                    ValidationCategory.Text,
                    ValidationError.DataMatrixTextLengthOutOfRange,
                    "Character counts must be less than 80.");
                return builder.Create ();
            }
            return ValidationResult.Empty;
        }


        class OfDataMatrixParameter : DataMatrixTextValidator
        {
            IBaseDataMatrixParameter p;

            public OfDataMatrixParameter (IBaseDataMatrixParameter p)
            {
                this.p = p;
            }

            protected override string Text {
                get {
                    return p.Text;
            }}
        }
    }
}

