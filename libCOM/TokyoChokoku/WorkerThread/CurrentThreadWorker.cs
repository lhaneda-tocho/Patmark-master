using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TokyoChokoku.WorkerThread
{
    public class CurrentThreadWorker : SingleThreadWorker
    {
        /// <summary>
        /// ロガーです。
        /// </summary>
        static NLog.Logger Logger { get; }
            = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// タスク受付キューです。
        /// </summary>
        BlockingCollection<Action> queue
            = new BlockingCollection<Action>();

        public override string Name => Runner.Name;

        public Thread Runner;

        public CurrentThreadWorker(Thread runner)
        {
            Runner = runner ?? throw new ArgumentNullException("null thread");
        }

        public T Run<T>(Func<Task<T>> main)
        {
            var previous = SynchronizationContext.Current;
            SetSynchronizationContext(this);
            async Task<T> RunAsync()
            {
                return await main();
            }
            var maintask = RunAsync();
            try
            {
                while (true)
                {
                    if (maintask.IsCanceled || maintask.IsFaulted || maintask.IsCompleted)
                    {
                        queue.CompleteAdding();
                        break;
                    }
                    var action = queue.Take();
                    action();
                }
            }
            catch (InvalidOperationException)
            {
                // Complete
            } finally
            {
                SetSynchronizationContext(previous);
            }
            if (maintask.IsFaulted)
            {
                var exceptions = maintask.Exception as AggregateException;
                if(exceptions == null)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(maintask.Exception).Throw();
                if (exceptions.InnerExceptions.Count == 1)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exceptions.InnerExceptions[0]).Throw();
                if (exceptions.InnerExceptions.Count > 1)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exceptions).Throw();
                throw new InvalidProgramException("internal error", maintask.Exception);
            }
            if (maintask.IsCanceled)
            {
                try
                {
                    return maintask.Result;
                } catch (AggregateException ex)
                {
                    var exceptions = ex as AggregateException;
                    if (exceptions == null)
                        System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(maintask.Exception).Throw();
                    if (exceptions.InnerExceptions.Count == 1)
                        System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exceptions.InnerExceptions[0]).Throw();
                    if (exceptions.InnerExceptions.Count > 1)
                        System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exceptions).Throw();
                    throw new InvalidProgramException("internal error", maintask.Exception);
                }
            }
            if (maintask.IsCompleted)
                return maintask.Result;
            throw new TaskCanceledException();
        }

        /// <summary>
        /// このWorkerは閉じることができません。
        /// </summary>
        public override void Dispose()
        {
            throw new InvalidOperationException();
        }

        public override Task<R> Execute<R>(Func<R> func)
        {
            var task = new TaskCompletionSource<R>();

            queue.Add(() =>
            {
                try
                {
                    R ans = func();
                    task.SetResult(ans);
                }
                catch (Exception e)
                {
                    task.SetException(e);
                    Logger.Error(e, string.Format("the exception handled by {0}", Name));
                }
            });
            return task.Task;
        }
    }
}
