using System;
namespace TokyoChokoku.Patmark.Common
{
    public abstract class ComparableValueType<T> : ValueType<T>, IComparable<ComparableValueType<T>>, IComparable<T> where T : ComparableValueType<T>
    {
        /// <summary>
        /// 大小比較を行います。
        /// </summary>
        /// <param name="other">大小比較の右辺値です. null を入れた場合は 常に正の値を返します.</param>
        /// <returns></returns>
        public abstract int CompareTo(T other);


        /// <summary>
        /// 大小比較を行います。
        /// </summary>
        /// <param name="other">大小比較の右辺値です. null を入れた場合は 常に正の値を返します.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">other が 互換性のない型であった場合 (このクラスを継承するサブクラスの実装ミスによって引き起こされます)</exception>
        public int CompareTo(ComparableValueType<T> other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            var obj = other as T;
            Checks.Required(!ReferenceEquals(obj, null), "CompareTo: Incompatible Type");
            return CompareTo(obj);
        }

        public static int Compare(ComparableValueType<T> a, ComparableValueType<T> b)
        {
            if (ReferenceEquals(a, b))
                return 0;
            if (ReferenceEquals(a, null))
                return -1;
            return a.CompareTo(b);
        }

        public static bool operator > (ComparableValueType<T> a, ComparableValueType<T> b)
        {
            return Compare(a, b) > 0;
        }

        public static bool operator >=(ComparableValueType<T> a, ComparableValueType<T> b)
        {
            return Compare(a, b) >= 0;
        }

        public static bool operator < (ComparableValueType<T> a, ComparableValueType<T> b)
        {
            return Compare(a, b) < 0;
        }

        public static bool operator <=(ComparableValueType<T> a, ComparableValueType<T> b)
        {
            return Compare(a, b) <= 0;
        }
    }
}
