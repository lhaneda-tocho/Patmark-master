using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarShiftEndingMinuteValidator
    {
        public CalendarShiftEndingMinuteValidator ()
        {
        }

        public ValidationResult Validate (byte value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

