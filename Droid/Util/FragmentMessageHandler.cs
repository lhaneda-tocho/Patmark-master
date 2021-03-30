using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TokyoChokoku.Communication;
using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;
using TokyoChokoku.Patmark.Presenter.Embossment;
using TokyoChokoku.Patmark.Droid.Presenter.FileMenu;

namespace TokyoChokoku.Patmark.Droid.Util
{
    public class FragmentAccessHandler
    {
        /// <summary>
        /// The message queue Buffer.
        /// </summary>
        readonly ConcurrentQueue<Action> buffer = new ConcurrentQueue<Action>();

        /// <summary>
        /// フラグメントが処理できる期間であれば true, そうでなければ false
        /// </summary>
        volatile bool safe = false;

        /// <summary>
        /// Resume Callback. (needs to call on ui thread)
        /// </summary>
        public void OnResume() {
            Action task;
            safe = true;
            while(buffer.TryDequeue(out task)) {
                task();
            }
        }

        /// <summary>
        /// pause callback  (needs to call on ui thread)
        /// </summary>
        public void OnPause() {
            safe = false;
        }

        void HandleTask(Action task)
        {
            if (safe)
            {
                task();
            }
            else
            {
                buffer.Enqueue(task);
            }
        }

        /// <summary>
        /// Post the specified action.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="task">Task.</param>
        public Task Post(Action task)
        {
            var tcs = new TaskCompletionSource<object>();
            Application.SynchronizationContext.Post((_) =>
            {
                HandleTask(() => {
                    try
                    {
                        task();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                        tcs.SetException(ex);
                    }
                });
            }, null);
            return tcs.Task;
        }



        /// <summary>
        /// Post the specified action.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="task">Task.</param>
        public Task<T> Post<T>(Func<T> task)
        {
            var tcs = new TaskCompletionSource<T>();
            Application.SynchronizationContext.Post((_) =>
            {
                HandleTask(() => {
                    try
                    {
                        T ins = task();
                        tcs.SetResult(ins);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                        tcs.SetException(ex);
                    }
                });
            }, null);
            return tcs.Task;
        }

    }
}
