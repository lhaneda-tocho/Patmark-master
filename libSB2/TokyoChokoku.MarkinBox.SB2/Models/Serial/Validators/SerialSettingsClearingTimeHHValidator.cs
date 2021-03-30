using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsClearingTimeHHValidator
    {
        public SerialSettingsClearingTimeHHValidator ()
        {
        }

        public ValidationResult Validate (short value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

