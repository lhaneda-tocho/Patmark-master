using System;
using TokyoChokoku.MachineModel;

namespace TokyoChokoku.Patmark.MachineModel
{

    /// <summary>
    /// Patmarkのバージョンを表します. Patmarkのバージョンは以下の順で4つの数で表します.
    /// Major, Middle, Minor, Revision
    /// </summary>
    public class PatmarkVersion: MachineVersionSpec, IComparable<PatmarkVersion>
    {
        /// <summary>
        /// バージョンを指定してインスタンス化します. 2つ目以降は省略可能でそうした場合は 0 という扱いになります.
        /// </summary>
        /// <returns>The version.</returns>
        public static PatmarkVersion Of(int major, int middle=0, int minor=0, int revision=0)
        {
            return new PatmarkVersion(major, middle, minor, revision);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v0">major</param>
        /// <param name="v1">middle</param>
        /// <param name="v2">minor</param>
        /// <param name="v3">revision</param>
        protected PatmarkVersion(int v0, int v1, int v2, int v3): base("Patmark", v0, v1, v2, v3)
        {
        }

        /// <summary>
        /// 人が見やすい形式に変換します。
        /// </summary>
        /// <returns>人が見やすい形式に変換します。</returns>
        public override string ToString()
        {
            return string.Format("Ver. {0}.{1}.{2} Rev.{3}", this[0], this[1], this[2], this[3]);
        }

        /// <summary>
        /// バージョンが新しいか古いかの比較を行います. 正の値の時は新しいバージョン, 負の値の時は古いバージョンと表します.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="other">Other.</param>
        public int CompareTo(PatmarkVersion other)
        {
            var cnt = Math.Min(Count, other.Count); // 小さい方に合わせる
            for (int i = 0; i < cnt; ++i)
            {
                var a = this[i];
                var b = other[i];
                if (a < b) return +1; // 正順
                if (a > b) return -1; // 逆順
                // 同じ場合は次の比較を行う
            }
            // ここまで同じ値なら長い方を新しいバージョンとする
            if (Count < other.Count) return +1; // 正順
            if (Count > other.Count) return -1; // 逆順
            return 0;
        }

        /// <summary>
        /// Equals operation
        /// </summary>
        /// <param name="obj">right term</param>
        /// <returns>condition</returns>
        /// <see cref="MachineVersionSpec.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Hash code generator.
        /// </summary>
        /// <returns>the hashcode of this instance.</returns>
        /// <see cref="MachineVersionSpec.GetHashCode()"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Operator of &lt;
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator <(PatmarkVersion a, PatmarkVersion b)
        {
            return a.CompareTo(b) >  0;
        }

        /// <summary>
        /// Operator of &lt;=
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator <=(PatmarkVersion a, PatmarkVersion b)
        {
            return a.CompareTo(b) >= 0;
        }

        /// <summary>
        /// Operator of &gt;
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator >(PatmarkVersion a, PatmarkVersion b)
        {
            return a.CompareTo(b) <  0;
        }

        /// <summary>
        /// Operator of &gt;=
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator >=(PatmarkVersion a, PatmarkVersion b)
        {
            return a.CompareTo(b) <= 0;
        }


        /// <summary>
        /// Operator of ==
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator ==(PatmarkVersion a, PatmarkVersion b)
        {
            return a.Equals(b);
        }


        /// <summary>
        /// Operator of !=
        /// </summary>
        /// <param name="a">left term</param>
        /// <param name="b">right term</param>
        /// <returns>condition</returns>
        public static bool operator !=(PatmarkVersion a, PatmarkVersion b)
        {
            return !a.Equals(b);
        }
    }
}
