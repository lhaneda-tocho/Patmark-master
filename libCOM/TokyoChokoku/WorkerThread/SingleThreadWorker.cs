using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TokyoChokoku.TaskUtil;

namespace TokyoChokoku.WorkerThread
{
    /// <summary>
    /// 単一スレッド上で タスクを処理するクラスであり、タスクを受け付けるための Start メソッドと、Actionを受け付ける Execute メソッドを実装します。
    /// </summary>
    public abstract class SingleThreadWorker : SynchronizationContext, IDisposable
    {
        public static SingleThreadPoolWorker CreateNew(string name=null)
        {
            return new SingleThreadPoolWorker(name);
        }

        private static CurrentThreadWorker CreateCurrentThread(Thread runner = null)
        {
            return new CurrentThreadWorker(runner ?? Thread.CurrentThread);
        }

        public static T RunBlocking<T>(Func<Task<T>> task)
        {
            return new CurrentThreadWorker(Thread.CurrentThread).Run(task);
        }

        public static void RunBlocking(Func<Task> task)
        {
            new CurrentThreadWorker(Thread.CurrentThread).Run<object>(async () => { await task(); return null; });
        }

        public static SingleThreadWorker CurrentOrNull()
        {
            return SynchronizationContext.Current as SingleThreadWorker;
        }


        /// <summary>
        /// このワーカーの名前です。
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 引数に指定したタスクを、このワーカースレッドで実行します。
        /// (Task の flatmap 関数)
        /// </summary>
        /// <param name="task">呼び出したいタスクオブジェクト</param>
        /// <returns>新しいタスクオブジェクト</returns>
        public Task Start(Func<Task> task)
        {
            var tcs = new TaskCompletionSource<object>();
            Execute(async () => {
                try
                {
                    await task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// 引数に指定したタスクを、このワーカースレッドで実行します。
        /// (Task の flatmap 関数)
        /// </summary>
        /// <typeparam name="R">タスクの返り値</typeparam>
        /// <param name="task">呼び出したいタスクオブジェクト</param>
        /// <returns>新しいタスクオブジェクト</returns>
        public Task<R> Start<R>(Func<Task<R>> task)
        {
            var tcs = new TaskCompletionSource<R>();
            Execute(async () => {
                try
                {
                    var result = await task();
                    tcs.SetResult(result);
                } catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// 指定されたアクションメソッドをこのクラスに対応するスレッド上で実行します。
        /// </summary>
        /// <param name="func">呼び出したいアクション</param>
        /// <returns>タスクオブジェクト。アクションの実行結果を表します。</returns>
        /// <exception cref="InvalidOperationException">ワーカーがDisposeされており、リクエストを受け付けない場合。</exception>
        public Task Execute(Action func)
        {
            return Execute<object>(() => { func(); return null; });
        }
        
        public override void Post(SendOrPostCallback d, object state)
        {
            Execute(() => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Execute(() => d(state)).WaitCompletion();
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }

        /// <summary>
        /// 指定された関数メソッドをこのクラスに対応するスレッド上で実行します。
        /// </summary>
        /// <param name="func">呼び出したい関数</param>
        /// <typeparam name="R">タスクの返り値</typeparam>
        /// <returns>関数オブジェクト。関数の実行結果と返り値を表します。</returns>
        /// <exception cref="InvalidOperationException">ワーカーがDisposeされており、リクエストを受け付けない場合。</exception>
        public abstract Task<R> Execute<R>(Func<R> func);

        /// <summary>
        /// 可能ならワーカースレッドを終了します。終了する必要がない、終了できない場合は、例外がスローされます。
        /// 
        /// このメソッドをこのワーカースレッド内で呼び出した場合は、終了をスケジュールし、メソッドを脱出します。
        /// それ以外のスレッドの場合は、終了するまでブロックされます。
        /// </summary>
        /// <exception cref="InvalidOperationException">終了できないワーカースレッドの場合。</exception>
        public abstract void Dispose();
    }
}
