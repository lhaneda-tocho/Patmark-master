using System;
using System.Threading;
namespace TokyoChokoku.Communication
{
    public class AtomicIntState
    {
        public int State => state;
        volatile int state;

        public AtomicIntState(int start)
        {
            state = start;
        }

        /// <summary>
        /// 状態遷移を試みる.
        /// </summary>
        /// <returns>(状態遷移が成功したかどうか, 前の状態)のペア</returns>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        public Tuple<bool, int> GetAndTryWalk(int to, int from)
        {
            var prev = Interlocked.CompareExchange(ref state, to, from);
            return Tuple.Create(prev == from, prev);
        }

        /// <summary>
        /// 状態遷移を試みる. 失敗したらStateWalkExceptionをスローします.
        /// </summary>
        /// <returns>before stae</returns>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <exception cref="StateWalkException">状態遷移に失敗した場合</exception>
        public int ShouldGetAndWalk(int to, int from)
        {
            var tuple = GetAndTryWalk(to, from);
            var success = tuple.Item1;
            var prev    = tuple.Item2;
            if (success)
                return prev;
            else
                throw new StateWalkException();
        }

        /// <summary>
        /// 状態遷移を行う.
        /// </summary>
        /// <returns>前の状態</returns>
        /// <param name="to">To.</param>
        public int GetAndWalk(int to)
        {
            return Interlocked.Exchange(ref state, to);
        }

        /// <summary>
        /// comparatorで条件が一致している場合に状態遷移を試みる.
        /// </summary>
        /// <returns>(状態遷移が成功したかどうか, 前の状態)のペア</returns>
        /// <param name="to">To.</param>
        /// <param name="comparator">Comparator. (current)→bool</param>
        public Tuple<bool, int> GetAndCompareWalk(int to, Func<int, bool> comparator)
        {
            int prev = state;
            while (comparator(prev))
            {
                int prev2 = Interlocked.CompareExchange(ref state, to, prev);
                if (prev == prev2)
                    return Tuple.Create(true, prev2);
                prev = prev2;
            }
            return Tuple.Create(false, prev);
        }
    }

    public class StateWalkException: Exception {
        public StateWalkException()
        {
        }

        public StateWalkException(string message) : base(message)
        {
        }

        public StateWalkException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
