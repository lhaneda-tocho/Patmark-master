namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public static class EmptyValidator
    {
        public static ValidationResult Validate <Type> (Type value)
        {
            return ValidationResult.Empty;
        }

        public static Type Adjust<Type> (Type value)
        {
            return value;
        }
    }
}

