using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarShiftStartingMinuteValidator
    {
        public CalendarShiftStartingMinuteValidator ()
        {
        }

        public ValidationResult Validate (byte value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

