using System;
namespace TokyoChokoku.CalendarModule
{
    /// <summary>
    /// シフトの適用範囲
    /// Replacement range.
    /// </summary>
    public struct TimeRange
    {
        /// <summary>
        /// Start Inclusive
        /// </summary>
        public Point From;

        /// <summary>
        /// End Exclusive
        /// </summary>
        public Point To;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <returns>The init.</returns>
        public static TimeRange Init()
        {
            var range = new TimeRange();
            range.From = Point.Init();
            range.To = Point.Init();
            return range;
        }

        /// <summary>
        /// Start, End のデータ型
        /// </summary>
        public struct Point {
            public byte Hour;
            public byte Min;

            /// <summary>
            /// Initialize the specified day and min.
            /// </summary>
            /// <returns>The init.</returns>
            /// <param name="day">Day.</param>
            /// <param name="min">Minimum.</param>
            public static Point Init(byte hour=0, byte min=0)
            {
                var p = new Point();
                p.Hour = hour;
                p.Min  = min;
                return p;
            }

            public override string ToString()
            {
                return Hour + ":" + Min;
            }
        }

        public bool ContainsAt(DateTime date)
        {
            var hour = (byte) date.Hour;
            var min  = (byte)date.Minute;
            return ContainsAt(hour, min);
        }

        /// <summary>
        /// Containses at Time of hour and min.
        /// </summary>
        /// <returns>The <see cref="T:System.Boolean"/>.</returns>
        /// <param name="hour">Hour.</param>
        /// <param name="min">Minimum.</param>
        public bool ContainsAt(byte hour, byte min) 
        {
            int nowH = hour;
            int nowM = min;
            int sH = From.Hour;
            int sM = From.Min;
            int eH = To.Hour;
            int eM = To.Min;

            // 23:59 ~ 0:01 対策
            if (sH > eH)
                eH += 24;

            bool isEqualsOrGreaterThanStart = (sH < nowH) || (sH   == nowH && sM   <= nowM);
            bool isLessThanEnd              = (nowH < eH) || (nowH == eH   && nowM <  eM  );

            return isEqualsOrGreaterThanStart && isLessThanEnd;
        }

        public override string ToString()
        {
            return "From=" + From + ", To=" + To;
        }
    }
}
