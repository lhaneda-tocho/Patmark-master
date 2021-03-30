using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarDayValidator
    {
        public CalendarDayValidator ()
        {
        }

        public ValidationResult Validate (char value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

