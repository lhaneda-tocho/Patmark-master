using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarYearValidator
    {
        public CalendarYearValidator ()
        {
        }

        public ValidationResult Validate (char value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

