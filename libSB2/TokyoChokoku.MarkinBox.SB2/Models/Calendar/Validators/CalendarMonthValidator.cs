using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarMonthValidator
    {
        public CalendarMonthValidator ()
        {
        }

        public ValidationResult Validate (char value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

