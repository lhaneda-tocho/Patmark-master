using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsClearingConditionValidator
    {
        public SerialSettingsClearingConditionValidator ()
        {
        }

        public ValidationResult Validate (short value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

