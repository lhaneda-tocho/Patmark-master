using System;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Counter
{
    public struct SCTime : IEquatable<SCTime>
    {
        public byte Hour;
        public byte Minute;

        /// <summary>
        /// Init with the specified hour and minute.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="hour">Hour.</param>
        /// <param name="minute">Minute.</param>
        public static SCTime Init(byte hour, byte minute) {
            var ins = new SCTime();
            ins.Hour   =   hour; 
            ins.Minute = minute;
            return ins;
        }

        /// <summary>
        /// Init from the specified MBSerialTime
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="stime">Stime.</param>
        public static SCTime From(MBSerialTime stime) {
            return Init(stime.Hour, stime.Minute);
        }

        /// <summary>
        /// Convert to MBForm.
        /// </summary>
        /// <returns>The MBF orm.</returns>
        public MBSerialTime ToMBForm() {
            return MBSerialTime.Init(Hour, Minute);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Hour, Minute);
        }

        // ↓ 自動生成 ↓

        public override bool Equals(object obj)
        {
            return obj is SCTime time && Equals(time);
        }

        public bool Equals(SCTime other)
        {
            return Hour == other.Hour &&
                   Minute == other.Minute;
        }

        public override int GetHashCode()
        {
            int hashCode = 510674192;
            hashCode = hashCode * -1521134295 + Hour.GetHashCode();
            hashCode = hashCode * -1521134295 + Minute.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SCTime left, SCTime right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SCTime left, SCTime right)
        {
            return !(left == right);
        }

        // ↑ 自動生成 ↑
    }
}
