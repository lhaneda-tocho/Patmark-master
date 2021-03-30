using System;
using System.Threading.Tasks;
using Unicast.ProgressKit;

namespace TokyoChokoku.Patmark.Droid
{
    public static class ProgressAPIAndroid
    {
        static Lazy<ProgressTaskWorker> MainWorkerLazy { get; }

        static Lazy<ProgressAPI> Handle { get; }

        static ProgressAPIAndroid()
        {
            MainWorkerLazy = new Lazy<ProgressTaskWorker>(() =>
                ProgressTaskWorkerOnSyncContext.CreateCurrentContext()
            );
            Handle = new Lazy<ProgressAPI>(() => {
                var main = MainWorkerLazy.Value;
                return new ProgressAPI(main);
            });
        }

        // ====================

        public static void InitOnCurrentThread()
        {
            _ = Handle.Value;
        }

        /// <summary>
        /// ProgressAPI インスタンスを取得します。
        /// </summary>
        /// <returns>取得したインスタンス</returns>
        /// <exception cref="InvalidOperationException">初期化されていない場合。</exception>
        public static ProgressAPI GetProgressAPI()
        {
            if (Handle.IsValueCreated)
                return Handle.Value;
            throw new InvalidOperationException("not initialized");
        }

        // ====================

        public static ProgressEventListenerToken Register(ProgressViewController view)
        {
            return GetProgressAPI().Register(view);
        }


        // ====================

        /// <summary>
        /// Compute スレッドプール上でタスクを起動します。
        /// </summary>
        public static async Task<R> LaunchComputeTaskAsync<R>(Func<ProgressTaskHandler, Task<R>> block)
        {
            return await GetProgressAPI().LaunchTaskAsync(ProgressTaskWorkerOnThreadPool.Instance, block);
        }


        /// <summary>
        /// Compute スレッドプール上でタスクを起動します。
        /// </summary>
        public static async Task LaunchComputeTaskAsync(Func<ProgressTaskHandler, Task> block)
        {
            await GetProgressAPI().LaunchTaskAsync(ProgressTaskWorkerOnThreadPool.Instance, block);
        }

        /// <summary>
        /// UIスレッド上でタスクを起動します。
        /// </summary>
        public static async Task<R> LaunchUITaskAsync<R>(Func<ProgressTaskHandler, Task<R>> block)
        {
            return await GetProgressAPI().LaunchTaskAsync(null, block);
        }

        /// <summary>
        /// UIスレッド上でタスクを起動します。
        /// </summary>
        public static async Task LaunchUITaskAsync(Func<ProgressTaskHandler, Task> block)
        {
            await GetProgressAPI().LaunchTaskAsync(null, block);
        }

        // ====================
    }
}
