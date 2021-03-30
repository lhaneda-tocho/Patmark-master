using System;
using System.Threading;
using System.Threading.Tasks;
namespace TokyoChokoku.Communication
{
    class TerminationHook
    {
        volatile TaskCompletionSource<object> source;

        public TaskCompletionSource<object> Register()
        {
            var s = new TaskCompletionSource<object>();
            Interlocked.Exchange(ref source, s);
            return s;
        }

        public void Complete(TaskCompletionSource<object> tcs)
        {
            Interlocked.CompareExchange(ref source, null, tcs);
            tcs.SetResult(null);
        }

        /// <summary>
        /// タスクが終了するまで待機する.
        /// Registerしていない, もしくは Unregister後の場合は何もせずに終了する.
        /// </summary>
        /// <returns>The hook.</returns>
        public async Task Hook()
        {
            var task = source?.Task;
            if (task == null)
            {
                await Task.Yield();
                return;
            }
            else
                await task;
        }
    }
}
