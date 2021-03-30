using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokyoChokoku.TaskUtil
{
    /// <summary>
    /// タスクのユーティリティクラスです。
    /// </summary>
    public static class TaskUtil
    {
        /// <summary>
        /// タスクが完了するまで待機します。タスクが破棄されている場合は直ちに終了します。
        /// </summary>
        /// <param name="task"></param>
        /// <param name="exceptionWhenCancelled">trueの場合、タスクがキャンセルされていた場合に例外をスローします。(デフォルト値 false)</param>
        /// <returns>引数に指定したタスク。</returns>
        /// <exception cref="TaskCanceledException">タスクがキャンセルされた場合</exception>
        /// <exception cref="Exception\">タスク処理中に例外が発生した場合。</exception>
        public static Task WaitCompletion(
            this Task task,
            bool exceptionWhenCancelled = false
        )
        {
            try
            {
                task.Wait();
                //return onComplete(task);
            }
            catch (ObjectDisposedException)
            {
                // ignore
                // onComplete(task);
            }
            catch (AggregateException ex)
            {
                // 解体
                if (ex.InnerExceptions.Count > 1)
                    throw;
                var inner = ex.InnerException;
                if (!(inner is TaskCanceledException) || exceptionWhenCancelled)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return task;
        }
    }
}
