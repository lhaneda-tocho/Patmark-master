using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections;

namespace TokyoChokoku.CalendarModule.Replacement
{
    /// <summary>
    /// 年置換のリストです. 年番号 0~9から 該当する置換設定を読み込むことができます.
    /// 
    /// Thread safety : Unsafe | Thread-compatible or Anti-Thread(注1)
    /// 
    /// (注1): CRYearList() に指定されたリストはコンストラクタ内でコピーされません．
    ///        他のスレッドからこのリストを直接変更されうる場合は Aiti-Thread であるかもしれません．
    /// </summary>
    public class CRYearList : IEnumerable<CRYear>
    {
        public class Immutable : CRYearList
        {
            public Immutable() : base(CRYear.DefaultImmutableList) { }
            public Immutable(IEnumerable<CRYear> data) : base(data.Take(RequiredSize).ToImmutableList()) { }
        }
        public class Mutable : CRYearList
        {
            public Mutable() : base(CRYear.DefaultMutableList) { }
            public Mutable(IEnumerable<CRYear> data) : base(data.Take(RequiredSize).ToList()) { }
        }

        public const int RequiredSize = MarkinBox.MBCalendar.CharsOfYearReplacements;

        IList<CRYear> List;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => List.Count();

        /// <summary>
        /// Gets the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRYearList"/> at the specified index.
        /// index range : [0, 9]
        /// </summary>
        /// <param name="index">Index.</param>
        public CRYear this[int index]
        {
            get => List[index]; // 日付は 1から始まる
            set => List[index] = value;
        }

        /// <summary>
        /// The alternative of this[int]
        /// </summary>
        /// <param name="date">Date.</param>
        public CRYear this[DateTime date]
        {
            get => this[date.Year % 10];
            set => this[date.Year % 10] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRYearList"/> class.
        /// </summary>
        /// <param name="list">List.</param>
        public CRYearList(IList<CRYear> list)
        {
            if (list.Count != RequiredSize)
                throw new InvalidOperationException();
            List = list;
        }

        /// <summary>
        /// copy list to Immutable list.
        /// </summary>
        /// <returns>The copy.</returns>
        public Immutable ImmutableCopy() { return new Immutable(this); }

        /// <summary>
        /// copy list to Mutables list;
        /// </summary>
        /// <returns>The copy.</returns>
        public Mutable MutableCopy() { return new Mutable(this); }

        #region IEnumerable
        public IEnumerator<CRYear> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
        #endregion


    }
}
