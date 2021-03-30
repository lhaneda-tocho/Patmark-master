using System;
namespace TokyoChokoku.MarkinBox
{
    public static class MBSerialResetRule
    {
        // 0:最大　1:年　2:月　3:日
        public const UInt16 ReachingMaximum  = 0;
        public const UInt16 IntoSpecYear     = 1;
        public const UInt16 IntoSpecMonth    = 2;
        public const UInt16 OntoSpecDay      = 3;

        /// <summary>
        /// Verify the specified reset rule value.
        /// </summary>
        /// <returns>true if the rule is valid. otherwize, false.</returns>
        /// <param name="resetRule">Reset rule.</param>
        public static bool Verify(UInt16 resetRule) {
            switch(resetRule)
            {
                case ReachingMaximum:
                case IntoSpecYear:
                case IntoSpecMonth:
                case OntoSpecDay:
                    return true;
                default:
                    return false;
            }
        }
    }
}
