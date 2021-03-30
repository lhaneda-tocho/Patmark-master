using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections;

namespace TokyoChokoku.CalendarModule.Replacement
{
    /// <summary>
    /// シフトのリストです．シフト番号 1 ~ 5 を指定してシフトの設定を行うことができます.
    /// 
    /// Thread safety : Unsafe | Thread-compatible or Anti-Thread(注1)
    /// 
    /// (注1): CRShiftList() に指定されたリストはコンストラクタ内でコピーされません．
    ///        他のスレッドからこのリストを直接変更されうる場合は Aiti-Thread であるかもしれません．
    /// </summary>
    public class CRShiftList : IEnumerable<CRShift>
    {
        public class Immutable : CRShiftList
        {
            public Immutable() : base(CRShift.DefaultImmutableList) { }
            public Immutable(IEnumerable<CRShift> data) : base(data.Take(RequiredSize).ToImmutableList()) { }
        }
        public class Mutable : CRShiftList
        {
            public Mutable() : base(CRShift.DefaultMutableList) { }
            public Mutable(IEnumerable<CRShift> data) : base(data.Take(RequiredSize).ToList()) { }
        }

        public const int RequiredSize = MarkinBox.MBCalendar.NumOfShift;

        IList<CRShift> List;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => List.Count();

        /// <summary>
        /// Gets the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRShiftList"/> at the specified index.
        /// index range : [1, 5]
        /// </summary>
        /// <param name="index">Index.</param>
        public CRShift this[int index]
        {
            get => List[index - 1]; // シフト番号は 1から始まる
            set => List[index - 1] = value;
        }

        /// <summary>
        /// The alternative of this[int]
        /// throws NullReferenceException if the specified value is null;
        /// 
        /// </summary>
        /// <param name="date">Date.</param>
        /// <exception cref="ArgumentOutOfRangeException">if not contains the element corresponding with the specified date.</exception>
        public CRShift this[DateTime date]
        {
            get {
                foreach (var shift in this) {
                    if (shift.Range.ContainsAt(date))
                        return shift;
                }
                throw new ArgumentOutOfRangeException();
            }
            set {
                var c = Count;
                for (int i = 0; i < c; i++) {
                    var v = List[i];
                    if (v.Range.ContainsAt(date))
                    {
                        List[i] = value;
                        return;
                    }
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// check containing the element corresponding with the specified date.
        /// </summary>
        /// <returns>The <see cref="T:System.Boolean"/>.</returns>
        /// <param name="date">Date.</param>
        public bool ContainsAt(DateTime date) {
            return null != FirstAtTime(date);
        }

        /// <summary>
        /// Finds the element corresponding with the specified date.
        /// return null if no element is found.
        /// </summary>
        /// <param name="date">date</param>
        public CRShift? FirstAtTime(DateTime date)
        {
            foreach (var shift in this)
            {
                if (shift.Range.ContainsAt(date))
                    return shift;
            }
            return null;
        }

        /// <summary>
        /// Finds the element corresponding with the specified date.
        /// return defaultValue if no element is found.
        /// </summary>
        /// <param name="date">date</param>
        public CRShift FirstAtTime(DateTime date, CRShift defaultValue)
        {
            return FirstAtTime(date) ?? defaultValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.CalendarModel.Replacement.CRShiftList"/> class.
        /// </summary>
        /// <param name="list">List.</param>
        public CRShiftList(IList<CRShift> list)
        {
            if (list.Count != RequiredSize)
                throw new InvalidOperationException();
            List = list;
        }

        /// <summary>
        /// The specified shift with index is Enable or not.
        /// </summary>
        /// <returns><c>true</c>, if enable was ised, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        public bool IsEnable(int index) {
            return this[index].Enable;
        }

        /// <summary>
        /// The inversion of IsEnable(index)
        /// </summary>
        /// <returns><c>true</c>, if disable was ised, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        public bool IsDisable(int index) {
            return !IsEnable(index);
        }

        /// <summary>
        /// 有効な要素数を指定して shift の有効化・無効化を行います.
        /// この呼び出しにより
        ///     1 ~ availableCount の範囲が Enable に.
        ///     それ以降が全て Disable になります.
        /// </summary>
        public void ApplyAvailableCount(int availableCount) {
            var c = List.Count;
            for (int i = 1; i <= c; i++) {
                if (i < availableCount)
                    Enable(i);
                else
                    Disable(i);
            }
        }

        /// <summary>
        /// Enables the specified shift with index.
        /// </summary>
        /// <returns>The disable.</returns>
        /// <param name="index">Index.</param>
        public void Enable(int index)
        {
            var v = List[index];
            v.Enable = true;
            List[index] = v;
        }

        /// <summary>
        /// Disables the specified shift with index.
        /// </summary>
        /// <returns>The disable.</returns>
        /// <param name="index">Index.</param>
        public void Disable(int index) {
            var v = this[index];
            v.Enable = false;
            this[index] = v;
        }

        /// <summary>
        /// copy to a new mutable list.
        /// the new list do not have disabled shifts.
        /// </summary>
        /// <returns>The copy.</returns>
        public IList<CRShift> CopyEnables() {
            var enu = from e in this
                      where e.Enable
                      select e;
            return enu.ToList();
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
        public Mutable   MutableCopy() { return new Mutable(this); }

        #region IEnumerable
        public IEnumerator<CRShift> GetEnumerator()
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
