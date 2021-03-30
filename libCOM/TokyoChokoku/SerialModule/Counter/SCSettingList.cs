using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.MarkinBox;

namespace TokyoChokoku.SerialModule.Counter
{
    /// <summary>
    /// シリアル設定のリストです。
    /// リストのサイズは固定長で <c>MBSerial.NumOfSerial</c> の通りとなります。
    ///
    /// このクラスをインスタンス化する場合は、以下のコンストラクタを使用します。
    ///
    /// <c>.Mutable(IEnumerable)</c>
    /// <c>.Immutable(IEnumerable)</c>
    /// </summary>
    public class SCSettingList : IEnumerable<SCSetting>
    {
        /// <summary>
        /// 可変リストを返します。
        /// コンストラクタの stateList から返される要素の数は <c>MBSerial.NumOfSerial</c> である必要があります。
        /// 上記の条件を満たさない場合、フォールバックが実行されます。
        /// 
        /// * 4を下回る場合は、残りの要素を、初期値でインスタンス化したオブジェクトを代わりに配置します。
        /// * 4を超える場合は、超えた分の要素を切り落とします。
        /// </summary>
        public class Mutable : SCSettingList
        {
            public Mutable() : base(SCSetting.DefaultList) { }
            public Mutable(IEnumerable<SCSetting> stateList) : base(CopyWithFallback(stateList, asImmutably: false))
            {
            }
        }

        /// <summary>
        /// 不変リストを返します。
        /// コンストラクタの stateList から返される要素の数は <c>MBSerial.NumOfSerial</c> である必要があります。
        /// 上記の条件を満たさない場合、フォールバックが実行されます。
        /// 
        /// * 4を下回る場合は、残りの要素を、初期値でインスタンス化したオブジェクトを代わりに配置します。
        /// * 4を超える場合は、超えた分の要素を切り落とします。
        /// </summary>
        public class Immutable : SCSettingList
        {
            public Immutable() : base(SCSetting.DefaultList) { }
            public Immutable(IEnumerable<SCSetting> stateList) : base(CopyWithFallback(stateList, asImmutably: true))
            {
            }
        }

        // ================

        public static Mutable CreateFrom(IEnumerable<MBSerialData> scdataList)
        {
            var list = new Mutable(SCSetting.DefaultList);
            foreach (var (i, e) in Enumerable.Range(1, 4).Zip(scdataList, (i, x) => (i, x)))
            {
                var setting = SCSetting.From(e);
                list[i] = setting;
            }
            return list;
        }

        /// <summary>
        /// フォールバックありで、リストのコピーを実行します。必ず新しいリストに変換されて返されます。
        /// NOTE: この処理中に <c>original</c> が書き換えられた場合の動作は保証しません。
        /// NOTE: IEnumerable は有限リストである必要があります。無限リストの場合は無限ループに陥ります。
        /// </summary>
        /// <param name="original">条件を満たしていない可能性のあるリスト</param>
        /// <param name="asImmutably">true の時、リストを不変にします。フォールバックしない場合でも、不変リストへの変換処理を施して返します。</param>
        /// <returns>フォールバックした新しいリストです. asImmutably を true に設定した場合は、さらに不変リストに変換して返します。</returns>
        static IList<SCSetting> CopyWithFallback(IEnumerable<SCSetting> original, bool asImmutably)
        {
            // オリジナルのリストのコピー (IEnumerable のままだと処理できないので)
            var originalList = original.ToImmutableList();

            // コピーされる要素数. RequiredSize であるべきだが、 original がそれより小さい場合はそちらを採用する。
            var copiedElements = Math.Min(RequiredSize, originalList.Count);

            // フォールバック後のリスト
            var fallbacked = SCSetting.DefaultList.ToList();

            // アサーション
            if (fallbacked.Count != RequiredSize)
                throw new InvalidProgramException("AssertionError: SCSetting.DefaultList.Count and RequiredSize is not same value");

            // コピー処理
            foreach(var index in Enumerable.Range(0, copiedElements))
            {
                fallbacked[index] = originalList[index];
            }

            // 終わり
            if (asImmutably)
                return fallbacked.ToImmutableList();
            else
                return fallbacked;
        }

        // ================

        public const int RequiredSize = MBSerial.NumOfSerial;

        readonly IList<SCSetting> List;

        public int Count => List.Count;

        /// <summary>
        /// コンストラクタ. コピーではなく、参照を代入するだけです。
        /// </summary>
        SCSettingList(IList<SCSetting> stateList)
        {
            List = stateList;
        }

        /// <summary>
        /// シリアル番号を入力してシリアル設定を返します。
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public SCSetting this[int serialNo] // 1 ~ 4
        {
            get => List[serialNo - 1];
            set => List[serialNo - 1] = value;
        }

        /// <summary>
        /// シリアル番号のリストを返します。
        /// </summary>
        public IList<int> SerialNoList
        {
            get
            {
                return Enumerable.Range(1, Count).ToList();
            }
        }

        public Immutable ToImmutable() {
            return new Immutable(this);
        }

        public Mutable ToMutable() {
            return new Mutable(this);
        }


        /// <summary>
        /// Convert to MBForm
        /// </summary>
        /// <returns>The MBF orm.</returns>
        public IList<MBSerialData> ToMBForm() => (
            from e in this
            select e.ToMBForm()
        ).ToList();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[CountSettings]");
            foreach(var ss in this) {
                sb.AppendLine(ss.ToString());
            }
            return sb.ToString();
        }

        #region IEnumerable
        public IEnumerator<SCSetting> GetEnumerator()
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
