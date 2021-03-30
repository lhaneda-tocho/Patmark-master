using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace TokyoChokoku.FieldTextStreamer
{
    public class FieldTextStream
    {
        struct Matched<T>
        {
            public int     MatchIndex  { get; }
            public bool    Success     { get; }
            public string  Head        { get; }
            public Func<T> ConsumeBody { get; }
            public string  Tail        { get; }

            public Matched(int matchIndex, bool success, string head, Func<T> consumeBody, string tail)
            {
                if (matchIndex < 0)
                    throw new ArgumentOutOfRangeException($"{nameof(matchIndex)} should be 0 or positive.");
                MatchIndex = matchIndex;
                Success = success;
                if(success)
                {
                    Head = head ?? throw new ArgumentNullException(nameof(head));
                    ConsumeBody = consumeBody ?? throw new ArgumentNullException(nameof(consumeBody));
                } else
                {
                    if (head != null)
                        throw new ArgumentException($"{nameof(head)} shuld be null");
                    if (consumeBody != null)
                        throw new ArgumentException($"{nameof(consumeBody)} shuld be null");
                    Head = null;
                    ConsumeBody = null;
                }
                Tail = tail ?? throw new ArgumentNullException(nameof(tail));
            }
         }

        /// <summary>
        /// フィールドテキストです．
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; }


        /// <summary>
        /// フィールドテキストを指定して初期化します．
        /// </summary>
        /// <param name="text">Text.</param>
        public FieldTextStream(string text)
        {
            Source = text ?? throw new ArgumentNullException(nameof(text));
        }

        static Matched<T> MatchAndMap<T>(
            string source,
            Func<SerialPart, T> onSerial
        )
        {
            Matched<T> ans = new Matched<T>(
                int.MaxValue, // 配列の index は この値になれないので， null の代わりに利用できる．
                false,
                null,
                null,
                source
            );
            // マッチ
            {
                var ms = RegSerial.Match(source);
                if (ms.Success && ms.Index < ans.MatchIndex)
                {
                    ans = new Matched<T>(
                        ms.Index,
                        true,
                        ms.HeadText,
                        () => onSerial(ms.SerialPart),
                        ms.TailText
                    );
                }
            }
            return ans;
        }



        /// <summary>
        /// テキストのパース結果に関数を適用して任意の列挙を返します．
        ///
        /// NOTE: シノニムチェックまでは行いません。例えば、 SerialPart のID値が異常値でも onSerial は呼ばれます。
        /// </summary>
        /// <returns>変換結果</returns>
        /// <typeparam name="T">関数適用結果の返り値型.</typeparam>
        public IEnumerable<T> Select<T>(
            Func<TextPart  , T> onText,
            Func<SerialPart, T> onSerial
        )
        {
            // 変数準備
            string rem = Source;

            IList<Func<T>> consumerList = new List<Func<T>>();

            // マッチ
            Matched<T> m = MatchAndMap(rem, onSerial);

            // 繰り返し
            while (m.Success)
            {
                // リストに追加
                var head = m.Head;
                if (head.Length > 0)
                    consumerList.Add(() => onText(new TextPart(head)));
                consumerList.Add(m.ConsumeBody);

                // 次の評価
                rem = m.Tail;
                m = MatchAndMap(rem, onSerial);
            }
            consumerList.Add(() => onText(new TextPart(rem)));
            return from consumer in consumerList
                   select consumer();
        }
    }
}
