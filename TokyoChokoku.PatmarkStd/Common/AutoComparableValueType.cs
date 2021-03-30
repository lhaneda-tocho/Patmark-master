using System;
namespace TokyoChokoku.Patmark.Common
{
    public abstract class AutoComparableValueType<T> : ComparableValueType<T> where T : AutoComparableValueType<T>
    {
        readonly Lazy<ListValueType<object>> elementsLazy;
        readonly Lazy<int> hashLazy;

        /// <summary>
        /// 
        /// </summary>
        protected ListValueType<object> Elements => elementsLazy.Value;

        protected AutoComparableValueType()
        {
            elementsLazy = new Lazy<ListValueType<object>>(() => GetValueList(), true);
            hashLazy = new Lazy<int>(() => CalcHashCode(), true);
        }

        public sealed override bool Equals(T other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Elements.Equals(other.Elements);
        }

        public sealed override int GetHashCode()
        {
            return hashLazy.Value;
        }

        private int CalcHashCode()
        {
            return -1521134295 + Elements.GetHashCode();
        }

        /// <summary>
        /// 等値比較対象の値を登録したリストを返します。
        /// </summary>
        /// <returns></returns>
        protected abstract ListValueType<object> GetValueList();
    }
}
