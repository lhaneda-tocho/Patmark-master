using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsMaxValueValidator
    {
        public SerialSettingsMaxValueValidator ()
        {
        }

        public ValidationResult Validate (int value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

