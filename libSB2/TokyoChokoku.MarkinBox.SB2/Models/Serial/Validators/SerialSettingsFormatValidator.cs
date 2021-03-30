using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsFormatValidator
    {
        public SerialSettingsFormatValidator ()
        {
        }

        public ValidationResult Validate (short value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

