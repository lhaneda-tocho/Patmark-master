using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarShiftCodeValidator
    {
        public CalendarShiftCodeValidator ()
        {
        }

        public ValidationResult Validate (char value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

