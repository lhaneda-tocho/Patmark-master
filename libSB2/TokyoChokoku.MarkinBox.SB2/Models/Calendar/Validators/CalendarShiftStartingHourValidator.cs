using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarShiftStartingHourValidator
    {
        public CalendarShiftStartingHourValidator ()
        {
        }

        public ValidationResult Validate (byte value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

