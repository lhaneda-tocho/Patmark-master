using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections;

namespace TokyoChokoku.CalendarModule.Replacement
{

    /// <summary>
    /// 月置換のリストです. 月番号 1~12から 該当する置換設定を読み込むことができます.
    /// 
    /// Thread safety : Unsafe | Thread-compatible or Anti-Thread(注1)
    /// 
    /// (注1): CRMonthList() に指定されたリストはコンストラクタ内でコピーされません．
    ///        他のスレッドからこのリストを直接変更されうる場合は Aiti-Thread であるかもしれません．
    /// </summary>
    public class CRMonthList : IEnumerable<CRMonth>
    {
        public class Immutable : CRMonthList
        {
            public Immutable() : base(CRMonth.DefaultImmutableList) { }
            public Immutable(IEnumerable<CRMonth> data) : base(data.Take(RequiredSize).ToImmutableList()) { }
        }
        public class Mutable : CRMonthList
        {
            public Mutable() : base(CRMonth.DefaultMutableList) { }
            public Mutable(IEnumerable<CRMonth> data) : base(data.Take(RequiredSize).ToList()) { }
        }

        public const int RequiredSize = MarkinBox.MBCalendar.CharsOfMonthReplacements;

        IList<CRMonth> List;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => List.Count();

        /// <summary>
        /// Gets the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRMonthList"/> at the specified index.
        /// index range : [1, 12]
        /// </summary>
        /// <param name="index">Index.</param>
        public CRMonth this[int index]
        {
            get => List[index - 1]; // 日付は 1から始まる
            set => List[index - 1] = value;
        }

        /// <summary>
        /// The alternative of this[int]
        /// </summary>
        /// <param name="date">Date.</param>
        public CRMonth this[DateTime date]
        {
            get => this[date.Month];
            set => this[date.Month] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRMonthList"/> class.
        /// </summary>
        /// <param name="list">List.</param>
        public CRMonthList(IList<CRMonth> list)
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
        public IEnumerator<CRMonth> GetEnumerator()
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
