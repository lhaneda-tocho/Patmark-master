using System;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators {
    public partial class QrCodeValidator {

        public static ValidationResult Validate (IBaseQrCodeParameter param)
        {
            var textvalidator = QrCodeTextValidator.CreateOfQrCode (param);
            return textvalidator.ValidateText ();
        }
   }
}
