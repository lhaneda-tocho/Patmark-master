using System;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Counter
{
    public enum SCResetRule: UInt16
    {
        /// <summary>
        /// The reaching maximum.
        /// </summary>
        ReachingMaximum = MBSerialResetRule.ReachingMaximum,
        /// <summary>
        /// The into spec year.
        /// </summary>
        IntoSpecYear    = MBSerialResetRule.IntoSpecYear,
        /// <summary>
        /// The into spec month.
        /// </summary>
        IntoSpecMonth   = MBSerialResetRule.IntoSpecMonth,
        /// <summary>
        /// The onto spec day.
        /// </summary>
        OntoSpecDay     = MBSerialResetRule.OntoSpecDay
    }

    public static class SCResetRuleExt
    {
        /// <summary>
        /// Verify the specified format value.
        /// Usage: SCFormatExt.Verify(format = value);
        /// </summary>
        /// <returns>true if the format is valid. otherwize, false.</returns>
        /// <param name="format">Format.</param>
        public static bool IsSCResetRule(this UInt16 format)
        {
            return MBSerialResetRule.Verify(format);
        }

        /// <summary>
        /// Froms the format value.
        /// </summary>
        /// <returns>The format value.</returns>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentOutOfRangeException">value is not format value.(Verify(value) is false)</exception>
        public static SCResetRule ToSCResetRule(this UInt16 value)
        {
            switch (value)
            {
                case MBSerialResetRule.ReachingMaximum:
                    return SCResetRule.ReachingMaximum;
                case MBSerialResetRule.IntoSpecYear:
                    return SCResetRule.IntoSpecYear;
                case MBSerialResetRule.IntoSpecMonth:
                    return SCResetRule.IntoSpecMonth;
                case MBSerialResetRule.OntoSpecDay:
                    return SCResetRule.OntoSpecDay;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Convert to MBForm
        /// </summary>
        /// <returns>The MBForm.</returns>
        /// <param name="format">Format.</param>
        public static UInt16 ToMBForm(this SCResetRule format)
        {
            return (UInt16)format;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="format">Format.</param>
        public static string GetName(this SCResetRule format)
        {
            var type = typeof(SCResetRule);
            return Enum.GetName(type, format);
        }


        /// <summary>
        /// Pattern match the specified format
        /// and call a corresponding delegate from fillZero, rightJustify and leftJustify.
        /// </summary>
        /// <returns>The match.</returns>
        /// <param name="resetRule">Reset rule.</param>
        /// <param name="reachingMaximum">Reaching maximum.</param>
        /// <param name="intoSpecYear">Into spec year.</param>
        /// <param name="intoSpecMonth">Into spec month.</param>
        /// <param name="ontoSpecDay">Onto spec day.</param>
        /// <typeparam name="R">The 1st type parameter.</typeparam>
        public static R Match<R>(this SCResetRule resetRule,
                                 Func<SCResetRule, R> reachingMaximum,
                                 Func<SCResetRule, R> intoSpecYear,
                                 Func<SCResetRule, R> intoSpecMonth,
                                 Func<SCResetRule, R> ontoSpecDay)
        {
            switch (resetRule)
            {
                case SCResetRule.ReachingMaximum:
                    return reachingMaximum(resetRule);
                case SCResetRule.IntoSpecYear:
                    return intoSpecYear(resetRule);
                case SCResetRule.IntoSpecMonth:
                    return intoSpecMonth(resetRule);
                case SCResetRule.OntoSpecDay:
                    return ontoSpecDay(resetRule);
                                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
