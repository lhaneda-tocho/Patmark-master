using System;
using System.Threading;
using System.Threading.Tasks;
namespace Unicast.ProgressKit
{
    /// <summary>
    /// スレッドに設定されている SynchronizationContext を使用して実装された ProgressTaskWorker です。
    /// </summary>
    public class ProgressTaskWorkerOnSyncContext: ProgressTaskWorker
    {
        /// <summary>
        /// 現在のスレッドに設定されている SynchronizationContext を使用してこのオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">現在のスレッドに設定された SynchronizationContext が存在しない場合</exception>
        public static ProgressTaskWorkerOnSyncContext CreateCurrentContext()
        {
            var context = SynchronizationContext.Current ?? throw new InvalidOperationException("no synchronization context");
            return new ProgressTaskWorkerOnSyncContext(context);
        }

        // ===========================

        /// <summary>
        /// SynchronizationContext です。
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; }

        /// <summary>
        /// SynchronizationContext を指定して初期化します。
        /// </summary>
        /// <param name="context"></param>
        public ProgressTaskWorkerOnSyncContext(SynchronizationContext context)
        {
            SynchronizationContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ===========================

        /// <summary>
        /// 現在のスレッドに、このワーカと同じ <c>SynchronizationContext</c> が設定されているか確認します。
        /// Task.Run で起動されるスレッドのように、 SynchronizationContext を持たないスレッドでは常に false が返されます。
        /// </summary>
        /// <returns>このワーカと同じ <c>SynchronizationContext</c> が設定されていれば true, そうでなければ false.</returns>
        public bool CurrentThreadUsing()
        {
            return true;
            //System.Diagnostics.Debug.WriteLine("Before SynchronizationContext.Current");
            //var current = SynchronizationContext.Current;
            //System.Diagnostics.Debug.WriteLine("After SynchronizationContext.Current");
            //return ReferenceEquals(current, SynchronizationContext);
        }

        /// <inheritdoc/>
        public Task<R> Launch<T, R>(T message, Func<T, Task<R>> block)
        {
            var tcs = new TaskCompletionSource<R>();
            SynchronizationContext.Post((_) => {
                _ = Dispatch(tcs, message, block);
            }, null);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task Launch<T>(T message, Func<T, Task> block)
        {
            var tcs = new TaskCompletionSource<object>();
            SynchronizationContext.Post((_) => {
                _ = Dispatch(tcs, message, block);
            }, null);
            return tcs.Task;
        }


        // async Task と指定しても破棄されてしまうため、代わりに、 TaskCompletionSource で結果を返すようにします。
        async Task Dispatch<T, R>(TaskCompletionSource<R> tcs, T message, Func<T, Task<R>> block)
        {
            try
            {
                var ans = await block(message);
                tcs.SetResult(ans);
            }
            catch(Exception ex)
            {
                tcs.SetException(ex);
            }
        }

        // async Task と指定しても破棄されてしまうため、代わりに、 TaskCompletionSource で結果を返すようにします。
        async Task Dispatch<T>(TaskCompletionSource<object> tcs, T message, Func<T, Task> block)
        {
            try
            {
                await block(message);
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }
    }
}
