using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsClearingTimeMMValidator
    {
        public SerialSettingsClearingTimeMMValidator ()
        {
        }

        public ValidationResult Validate (short value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

