using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// <c>ProgressTask</c> の状態の保持と操作を担当するクラスです。
    /// </summary>
    public class ProgressTaskHandler
    {
        /// <summary>
        /// 保持しているタスクオブジェクトです。
        /// </summary>
        public IProgressTask ProgressTask { get; }

        /// <summary>
        /// CancellationToken です。
        /// </summary>
        public CancellationToken CancellationToken => ProgressTask.CancellationToken;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="task">操作するタスクオブジェクト (NOT NULL)</param>
        internal ProgressTaskHandler(IProgressTask task)
        {
            ProgressTask = task ?? throw new ArgumentNullException(nameof(task));
        }
    }
}
