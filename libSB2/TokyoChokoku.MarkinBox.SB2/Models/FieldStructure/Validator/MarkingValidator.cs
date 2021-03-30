using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public abstract partial class MarkingValidator
    {
        protected abstract short Speed { get; }
        protected abstract short Power { get; }
        protected abstract bool  Pause { get; }

        public ValidationResult ValidateSpeed (short speed)
        {
            return ValidationResult.Empty;
        }

        public ValidationResult ValidatePower (short power)
        {
            return ValidationResult.Empty;
        }

        public ValidationResult ValidateReverse (bool reverse)
        {
            return ValidationResult.Empty;
        }

        public ValidationResult ValidatePause (bool pause)
        {
            return ValidationResult.Empty;
        }



        public short AdjustSpeed (short speed)
        {
            return speed;
        }

        public short AdjustPower (short power)
        {
            return power;
        }

        public bool AdjustPause (bool pause)
        {
            return pause;
        }
    }
}

