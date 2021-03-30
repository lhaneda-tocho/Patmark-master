using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarShiftEndingHourValidator
    {
        public CalendarShiftEndingHourValidator ()
        {
        }

        public ValidationResult Validate (byte value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

