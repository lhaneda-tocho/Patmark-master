using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections;

namespace TokyoChokoku.CalendarModule.Replacement
{

    /// <summary>
    /// 日付置換のリストです. 日付番号 1~31から 該当する置換設定を読み込むことができます.
    /// 
    /// Thread safety : Unsafe | Thread-compatible or Anti-Thread(注1)
    /// 
    /// (注1): CRDayList() に指定されたリストはコンストラクタ内でコピーされません．
    ///        他のスレッドからこのリストを直接変更されうる場合は Aiti-Thread であるかもしれません．
    /// </summary>
    public class CRDayList : IEnumerable<CRDay>
    {
        public class Immutable : CRDayList
        {
            public Immutable() : base(CRDay.DefaultImmutableList) { }
            public Immutable(IEnumerable<CRDay> data) : base(data.Take(RequiredSize).ToImmutableList()) { }
        }
        public class Mutable : CRDayList
        {
            public Mutable() : base(CRDay.DefaultMutableList) { }
            public Mutable(IEnumerable<CRDay> data) : base(data.Take(RequiredSize).ToList()) { }
        }

        public const int RequiredSize = MarkinBox.MBCalendar.CharsOfDayReplacements;

        IList<CRDay> List;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => List.Count();

        /// <summary>
        /// Gets the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRDayList"/> at the specified index.
        /// index range : [1, 31]
        /// </summary>
        /// <param name="index">Index.</param>
        public CRDay this[int index] {
            get => List[index - 1]; // 日付は 1から始まる
            set => List[index - 1] = value;
        }

        /// <summary>
        /// The alternative of this[int]
        /// </summary>
        /// <param name="date">Date.</param>
        public CRDay this[DateTime date]
        {
            get => this[date.Day];
            set => this[date.Day] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRDayList"/> class.
        /// </summary>
        /// <param name="list">List.</param>
        public CRDayList(IList<CRDay> list)
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
        public IEnumerator<CRDay> GetEnumerator()
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
