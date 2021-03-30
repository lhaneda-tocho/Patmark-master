using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public abstract partial class MarkupTextValidator : TextValidator
    {
        protected abstract string Text { get; }

        public ValidationResult ValidateText (string text)
        {
            if (text.Length > 50) {
                var builder = ValidationResult.CreateBuilder ();
                builder.PutError (ValidationCategory.Text, ValidationError.TextLengthOutOfRange, " Character counts must be less than 50.");
                return builder.Create ();
            }
            return ValidationResult.Empty;
        }


        public string AdjustText (string text)
        {
            return text;
        }
    }
}

