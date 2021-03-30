/// @file RuleEnumeration.cs
/// @brief クラス定義ファイル
using System;
using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// The abstract class of rule enums.
    /// </summary>
    /// <typeparam name="T">The type of a subclass.</typeparam>
    public abstract class RuleEnumeration<T>: AutoComparableValueType<T> where T: RuleEnumeration<T>
    {
        /// <summary>
        /// The name of this instance.
        ///
        /// It is the name of an enum constant corresponding to this instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The id of this instance.
        ///
        /// It is not Index, not Ordinal and maybe Discontinuous.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// It is the ordinal index corresponding to this instance.
        ///
        /// It will be greater than equals 0 and less than the numbers of all values.
        /// In implementations where size object is defined, The size object index is the numbers of all values.
        ///
        /// Also, it is used by the CompareTo method.
        /// </summary>
        public abstract int Index { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">the specified id</param>
        /// <param name="name">the specified name</param>
        protected RuleEnumeration(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc/>
        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(Id)
                .Build();
        }

        /// <inheritdoc/>
        public override int CompareTo(T other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            if (Index > other.Index)
                return 1;
            else if (Index == other.Index)
                return 0;
            return -1;
        }

        /// <summary>
        /// This method returns the name corresponding to this instance.
        ///
        /// This is behaves like <c>Name</c>.
        /// </summary>
        /// <returns>The name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}