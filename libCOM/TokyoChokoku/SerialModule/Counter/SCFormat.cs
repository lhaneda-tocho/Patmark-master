using System;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Counter
{
    /// <summary>
    /// フォーマット
    /// </summary>
    public enum SCFormat: UInt16
    {
        /// <summary>
        /// The fill zero.
        /// </summary>
        FillZero     = MBSerialFormat.FillZero,
        /// <summary>
        /// The right justify.
        /// </summary>
        RightJustify = MBSerialFormat.RightJustify,
        /// <summary>
        /// The left justify.
        /// </summary>
        LeftJustify  = MBSerialFormat.LeftJustify
    }

    public static class SCFormatExt {
        /// <summary>
        /// Verify the specified format value.
        /// Usage: SCFormatExt.Verify(format = value);
        /// </summary>
        /// <returns>true if the format is valid. otherwize, false.</returns>
        /// <param name="format">Format.</param>
        public static bool IsSCFormat(this UInt16 format)
        {
            return MBSerialFormat.Verify(format);
        }

        /// <summary>
        /// Froms the format value.
        /// </summary>
        /// <returns>The format value.</returns>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentOutOfRangeException">value is not format value.(Verify(value) is false)</exception>
        public static SCFormat ToSCFormat(this UInt16 value)
        {
            switch(value)
            {
                case MBSerialFormat.FillZero:
                    return SCFormat.FillZero;
                case MBSerialFormat.RightJustify:
                    return SCFormat.RightJustify;
                case MBSerialFormat.LeftJustify:
                    return SCFormat.LeftJustify;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Convert to MBForm
        /// </summary>
        /// <returns>The MBF orm.</returns>
        /// <param name="format">Format.</param>
        public static UInt16 ToMBForm(this SCFormat format)
        {
            return (UInt16)format;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="format">Format.</param>
        public static string GetName(this SCFormat format) 
        {
            var type = typeof(SCFormat);
            return Enum.GetName(type, format);
        }

        /// <summary>
        /// Pattern match the specified format
        /// and call a corresponding delegate from fillZero, rightJustify and leftJustify.
        /// </summary>
        /// <returns>The match.</returns>
        /// <param name="format">Format.</param>
        /// <param name="fillZero">Fill zero.</param>
        /// <param name="rightJustify">Right justify.</param>
        /// <param name="leftJustify">Left justify.</param>
        /// <typeparam name="R">The 1st type parameter.</typeparam>
        public static R Match<R>(this SCFormat format,
                                    Func<SCFormat, R> fillZero,
                                    Func<SCFormat, R> rightJustify,
                                    Func<SCFormat, R> leftJustify)
        {
            switch(format)
            {
                case SCFormat.FillZero:
                    return fillZero(format);
                case SCFormat.RightJustify:
                    return rightJustify(format);
                case SCFormat.LeftJustify:
                    return leftJustify(format);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
