using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsRepeatingCountValidator
    {
        public SerialSettingsRepeatingCountValidator ()
        {
        }

        public ValidationResult Validate (byte value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

