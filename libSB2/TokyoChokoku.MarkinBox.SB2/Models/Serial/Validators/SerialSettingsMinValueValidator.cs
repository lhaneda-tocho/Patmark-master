using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsMinValueValidator
    {
        public SerialSettingsMinValueValidator ()
        {
        }

        public ValidationResult Validate (int value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

