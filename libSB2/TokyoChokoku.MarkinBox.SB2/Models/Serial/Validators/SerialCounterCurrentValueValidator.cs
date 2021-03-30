using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialCounterCurrentValueValidator
    {
        public SerialCounterCurrentValueValidator ()
        {
        }

        public ValidationResult Validate (int value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

