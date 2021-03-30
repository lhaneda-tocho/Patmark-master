using System;
namespace TokyoChokoku.MarkinBox
{
    /// <summary>
    /// Value of Serial Format
    /// </summary>
    public static class MBSerialFormat
    {
        /// <summary>
        /// 0埋め
        /// </summary>
        public const UInt16 FillZero     = 0;
        /// <summary>
        /// The right justify.
        /// </summary>
        public const UInt16 RightJustify = 1;
        /// <summary>
        /// The left justify.
        /// </summary>
        public const UInt16 LeftJustify  = 2;

        /// <summary>
        /// Verify the specified format value.
        /// Usage: SCFormatExt.Verify(format = value);
        /// </summary>
        /// <returns>true if the format is valid. otherwize, false.</returns>
        /// <param name="format">Format.</param>
        public static bool Verify (UInt16 format)
        {
            switch(format) {
                case FillZero:
                case RightJustify:
                case LeftJustify:
                    return true;
                default:
                    return false;
            }
        }
    }
}
