using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TokyoChokoku.Patmark.Common
{
    /// <summary>
    /// リストによる値型を表します。このオブジェクトは不変です。
    /// </summary>
    public sealed class ListValueType<T> : ValueType<ListValueType<T>>, IList<T>
    {
        public sealed class Builder
        {
            readonly ImmutableList<T>.Builder b = ImmutableList<T>.Empty.ToBuilder();

            public Builder Add(T value)
            {
                b.Add(value);
                return this;
            }

            public ListValueType<T> Build()
            {
                return new ListValueType<T>(b.ToImmutableList());
            }
        }

        public static Builder CreateBuilder() => new Builder();

        // ========

        private readonly IList<T> w;

        public ListValueType(ICollection<T> collection)
        {
            if (collection is ListValueType<T>)
                w = ((ListValueType<T>)collection).w;
            else if (collection is ImmutableList<T>)
                w = (ImmutableList<T>)collection;
            else
                w = ImmutableList.CreateRange(collection);
        }

        public override bool Equals(ListValueType<T> other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(w, other.w))
                return true;
            var count = Count;
            if (count != other.Count)
                return false;
            for (int i = 0; i < count; ++i)
            {
                var a = this[i];
                var b = other[i];
                if (!a.Equals(b))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            var count = Count;
            var hashCode = -2144758991;
            hashCode = hashCode * -1521134295 + count.GetHashCode();
            foreach(var e in this)
                hashCode = hashCode * -1521134295 + e.GetHashCode();
            return hashCode;
        }

        public T this[int index]
        {
            get => w[index];
            set => w[index] = value;
        }

        public int Count => w.Count;

        public bool IsReadOnly => w.IsReadOnly;

        public void Add(T item)
        {
            w.Add(item);
        }

        public void Clear()
        {
            w.Clear();
        }

        public bool Contains(T item)
        {
            return w.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            w.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return w.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return w.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            w.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return w.Remove(item);
        }

        public void RemoveAt(int index)
        {
            w.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return w.GetEnumerator();
        }
    }
}
