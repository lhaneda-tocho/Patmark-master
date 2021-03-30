using System;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// ProgressTaskLauncher の返り値なし版です。
    /// </summary>
    internal class ProgressTaskLauncher : ProgressTaskLauncher<object>
    {
        static Func<ProgressTaskHandler, Task<object>> Adapt(Func<ProgressTaskHandler, Task> block)
        {
            return async it =>
            {
                await block(it);
                return null;
            };
        }

        public ProgressTaskLauncher(ProgressTaskWorker worker, Func<ProgressTaskHandler, Task> block) : base(worker, Adapt(block))
        {
            // NONE
        }
    }

    /// <summary>
    /// 非同期タスクの起動処理を表します。
    /// </summary>
    internal class ProgressTaskLauncher<R>
    {
        /// <summary>
        /// このタスクを起動するスレッドです。
        /// (NOT NULL)
        /// </summary> 
        ProgressTaskWorker Worker { get; }

        /// <summary>
        /// 実行する処理です。
        /// (NOT NULL)
        /// </summary>
        Func<ProgressTaskHandler, Task<R>> Block { get; }

        private System.Diagnostics.StackTrace StackTrace;

        public ProgressTaskLauncher(ProgressTaskWorker worker, Func<ProgressTaskHandler, Task<R>> block)
        {
            Worker = worker ?? throw new ArgumentNullException(nameof(worker));
            Block = block ?? throw new ArgumentNullException(nameof(block));
            StackTrace = new System.Diagnostics.StackTrace(true);
        }


        /// <summary>
        /// タスクを起動します。
        /// </summary>
        /// <returns></returns>
        public async Task<R> Launch(ProgressTaskHandler handler)
        {
            try
            {
                return await Worker.Launch(handler, Block);
            } catch (ProgressTaskWorker.NullTaskException ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Unhandled Exception in ProgressTaskLauncher: {ex}{Environment.NewLine}==== previous stack trace when the task submitted ===={Environment.NewLine}{StackTrace}"
                );
                throw new TaskCanceledException("Reason: Unhandled Exception in ProgressTaskLauncher", ex);
            }
        }
    }
}
