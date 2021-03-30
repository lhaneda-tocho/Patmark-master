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
    /// シリアル状態のリストです。
    /// リストのサイズは固定長で <c>MBSerial.NumOfSerial</c> の通りとなります。
    /// 
    /// このクラスをインスタンス化する場合は、以下のコンストラクタを使用します。
    ///
    /// <c>.Mutable(IEnumerable)</c>
    /// <c>.Immutable(IEnumerable)</c>
    /// </summary>
    public class SCCountStateList : IEnumerable<SCCountState>
    {
        /// <summary>
        /// 可変リストです。
        /// コンストラクタの stateList から返される要素の数は <c>MBSerial.NumOfSerial</c> である必要があります。
        /// 上記の条件を満たさない場合、フォールバックが実行されます。
        /// 
        /// * 4を下回る場合は、残りの要素を、初期値でインスタンス化したオブジェクトを代わりに配置します。
        /// * 4を超える場合は、超えた分の要素を切り落とします。
        /// </summary>
        public class Mutable: SCCountStateList {
            public Mutable(): base(SCCountState.DefaultList) {}
            public Mutable(IEnumerable<SCCountState> stateList) : base(CopyWithFallback(stateList, asImmutably: false))
            {
            }
        }

        /// <summary>
        /// 不変リストです。
        /// コンストラクタの stateList から返される要素の数は <c>MBSerial.NumOfSerial</c> である必要があります。
        /// 上記の条件を満たさない場合、フォールバックが実行されます。
        /// 
        /// * 4を下回る場合は、残りの要素を、初期値でインスタンス化したオブジェクトを代わりに配置します。
        /// * 4を超える場合は、超えた分の要素を切り落とします。
        /// </summary>
        public class Immutable: SCCountStateList {
            public Immutable(): base(SCCountState.DefaultList) {}
            public Immutable(IEnumerable<SCCountState> stateList) : base(CopyWithFallback(stateList, asImmutably: true))
            {
            }
        }

        // ================

        public static Mutable CreateFrom(IEnumerable<MBSerialCounterData> scdataList)
        {
            var list = new SCCountStateList.Mutable(SCCountState.DefaultList);
            foreach(var e in scdataList)
            {
                var (serialNo, state) = SCCountState.From(e);
                if(0 < serialNo && serialNo <= RequiredSize)
                {
                    // シリアル番号が適正である場合
                    // 重複している場合は最後のものを適用する
                    list[e.SerialNo] = state;
                } else
                {
                    // シリアル番号が適正でない場合は無視される。
                    System.Diagnostics.Debug.WriteLine($"[Warning] Ignored Serial ID: {serialNo}");
                }
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
        static IList<SCCountState> CopyWithFallback(IEnumerable<SCCountState> original, bool asImmutably)
        {
            // オリジナルのリストのコピー (IEnumerable のままだと処理できないので)
            var originalList = original.ToImmutableList();

            // コピーされる要素数. RequiredSize であるべきだが、 original がそれより小さい場合はそちらを採用する。
            var copiedElements = Math.Min(RequiredSize, originalList.Count);

            // フォールバック後のリスト
            var fallbacked = SCCountState.DefaultList.ToList();

            // アサーション
            if (fallbacked.Count != RequiredSize)
                throw new InvalidProgramException("AssertionError: SCCountState.DefaultList.Count and RequiredSize is not same value");

            // コピー処理
            foreach (var index in Enumerable.Range(0, copiedElements))
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

        readonly IList<SCCountState> List;

        public int Count => List.Count;

        SCCountStateList(IList<SCCountState> stateList)
        {
            List = stateList;
        }

        public SCCountState this[int serialNo] // 1 ~ 4
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

        //public void Set(SCCountState state)
        //{
        //    var no = state.SerialNo;
        //    this[no] = state;
        //}

        //public void Set(SCCountState state, int serialNo)
        //{
        //    var data = state.ToMutable();
        //    data.SerialNo = (ushort)serialNo;
        //    Set(data.ToImmutable());
        //}

        public Immutable ToImmutable()
        {
            return new Immutable(this);
        }

        public Mutable ToMutable()
        {
            return new Mutable(this);
        }


        /// <summary>
        /// Convert to MBForm
        /// </summary>
        /// <returns>MBData</returns>
        public IList<MBSerialCounterData> ToMBForm() {
            var indices = Enumerable.Range(1, Count);
            var enu = indices.Zip(this,
                (i, e) => e.ToMBForm((ushort)i)
            );
            return enu.ToList();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[CountState]");
            foreach (var ss in this)
            {
                sb.AppendLine(ss.ToString());
            }
            return sb.ToString();
        }

        #region IEnumerable
        public IEnumerator<SCCountState> GetEnumerator()
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
