using System;

namespace TokyoChokoku.Patmark.Common
{

    /// <summary>
    /// 継承することで値オブジェクトを実装できる抽象クラスです。
    /// T には、継承するサブクラスの型が入ります。
    /// サブクラスで Equals(T), GetHashCode() を実装することで、値オブジェクトが完成します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueType<T>: IEquatable<T>, IEquatable<ValueType<T>> where T: ValueType<T>
    {

        /// <summary>
        /// Equals の実装です。 == メソッドなどにも利用されます。
        /// 注: null が来た場合は false を返すようにしてください。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(T other);

        /// <summary>
        /// ハッシュコードを計算します。
        /// </summary>
        /// <returns></returns>
        public override abstract int GetHashCode();

        public bool Equals(ValueType<T> other)
        {
            return Equals(other as T);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public static bool operator ==(ValueType<T> a, ValueType<T> b)
        {
            if (ReferenceEquals(a, null))
                return false;
            if (ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(ValueType<T> a, ValueType<T> b)
        {
            return !(a == b);
        }
    }
}