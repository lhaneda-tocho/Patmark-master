using System;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators {
    public partial class BypassValidator {

        public static ValidationResult Validate (IBaseBypassParameter param)
        {
            return ValidationResult.Empty;
        }
   }
}
