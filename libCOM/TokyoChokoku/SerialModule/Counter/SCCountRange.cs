using System;
namespace TokyoChokoku.SerialModule.Counter
{
    /// <summary>
    /// カウンターの範囲
    /// </summary>
    public struct SCCountRange : IEquatable<SCCountRange>
    {
        /// <summary>
        /// The max value.
        /// </summary>
        public UInt32 MaxValue;
        /// <summary>
        /// The minimum value.
        /// </summary>
        public UInt32 MinValue;

        /// <summary>
        /// Init the specified max and min.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="max">Max.</param>
        /// <param name="min">Minimum.</param>
        public static SCCountRange Init(UInt32 min, UInt32 max) {
            var ins = new SCCountRange();
            ins.MaxValue = max;
            ins.MinValue = min;
            return ins;
        }

        public override string ToString()
        {
            return string.Format("[{0} ~ {1}]", MinValue, MaxValue);
        }

        // ↓　自動生成 ↓　

        public override bool Equals(object obj)
        {
            return obj is SCCountRange range && Equals(range);
        }

        public bool Equals(SCCountRange other)
        {
            return MaxValue == other.MaxValue &&
                   MinValue == other.MinValue;
        }

        public override int GetHashCode()
        {
            int hashCode = -630686110;
            hashCode = hashCode * -1521134295 + MaxValue.GetHashCode();
            hashCode = hashCode * -1521134295 + MinValue.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SCCountRange left, SCCountRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SCCountRange left, SCCountRange right)
        {
            return !(left == right);
        }

        // ↑　自動生成  ↑
    }
}
