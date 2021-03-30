using System;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public interface ClosedValidator
    {
        /// <summary>
        /// 内部で所持する <c>IMutableParameter</c> の妥当性チェックを行います．
        /// </summary>
        ValidationResult Validate ();

    }
}

