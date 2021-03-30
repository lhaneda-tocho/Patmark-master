using System;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// <c>Task.Run</c>  を使用して実装されたワーカです。
    /// </summary>
    public class ProgressTaskWorkerOnThreadPool: ProgressTaskWorker
    {
        /// <summary>
        /// このインスタンスを取得します。
        /// </summary>
        public static ProgressTaskWorkerOnThreadPool Instance { get; } = new ProgressTaskWorkerOnThreadPool();


        // ====

        /// <summary>
        /// コンストラクタ禁止
        /// </summary>
        private ProgressTaskWorkerOnThreadPool() { }

        /// <summary>
        /// 常に false です。
        /// </summary>
        public bool CurrentThreadUsing()
        {
            return false;
        }


        /// <inheritdoc/>
        public async Task<R> Launch<T, R>(T message, Func<T, Task<R>> block)
        {
            _ = block ?? throw new ArgumentNullException(nameof(block));
            return await Task.Run(()=> 
                ProgressTaskWorker.StartTaskAsync<T, R>(message, block)
            );
        }

        /// <inheritdoc/>
        public async Task Launch<T>(T message, Func<T, Task> block)
        {
            _ = block ?? throw new ArgumentNullException(nameof(block));
            await Task.Run<object>(() => 
                ProgressTaskWorker.StartTaskAsync<T>(message, block)
            );
        }
    }
}
