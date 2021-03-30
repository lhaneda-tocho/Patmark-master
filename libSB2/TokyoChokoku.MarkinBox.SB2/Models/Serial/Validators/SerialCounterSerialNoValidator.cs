using System;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialCounterSerialNoValidator
    {
        public SerialCounterSerialNoValidator ()
        {
        }

        public ValidationResult Validate (short value)
        {
            return ValidationResult.CreateBuilder ().Create();
        }
    }
}

