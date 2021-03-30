using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// 非同期処理を表すオブジェクトのインタフェースです。
    /// </summary>
    public interface IProgressTask
    {
        /// <summary>
        /// このタスクされた時に、 キャンセル要求状態になる <c>CancellationToken</c> を返します。
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// 起動中のタスクを返します。
        /// </summary>
        /// <exception cref="InvalidOperationException">タスク起動前にこのプロパティにアクセスした場合</exception>
        Task Task { get; }

        /// <summary>
        /// タスクが終了した際に true となります。
        /// タスク起動前にアクセスした場合は false となります。
        /// </summary>
        public bool IsCompleted { get; }

        /// <summary>
        /// このタスクをキャンセルします。
        /// </summary>
        public void Cancel();

        /// <summary>
        /// このタスクが終了するまで非同期で待機します。
        ///
        /// 待機の仕方:
        /// <c>await progressTask.JoinAsync()</c>
        /// 
        /// </summary>
        /// <param name="exceptionHandler">例外ハンドラです。このタスクで発生した例外を引数に渡して実行します。 初期値は null です。</param>
        /// <returns>タスクが終了するまで待機するタスク</returns>
        /// <exception cref="Exception"> <c>exceptionHandler</c> 内でスローされた例外 </exception>
        Task JoinAsync(Action<Exception> exceptionHandler = null);
    }

    /// <summary>
    /// 非同期処理を表すオブジェクトです。
    ///
    /// <c>IProgressTask</c> を実装し、さらにタスクの起動〜終了までの状態を監視する責務があります。
    /// </summary>
    /// <typeparam name="T">タスクの返り値型</typeparam>
    public class ProgressTask<T>: IProgressTask
    {
        // ========

        CancellationTokenSource    Cts  { get; }
        Lazy<Task<T>>              TaskLazy { get; }

        /// <inheritdoc/>
        public CancellationToken CancellationToken => Cts.Token;

        /// <inheritdoc/>
        public Task Task
        {
            get
            {
                if (TaskLazy.IsValueCreated)
                    return TaskLazy.Value;
                throw new InvalidOperationException("Not Started");
            }
        }

        /// <inheritdoc/>
        public bool IsCompleted
        {
            get
            {
                if (TaskLazy.IsValueCreated)
                    return Task.IsCompleted;
                return false;
            }
        }

        /// <summary>
        /// タスク起動メソッドを指定して初期化します。
        /// </summary>
        /// <param name="launcher">StartIfNeeded の初回呼び出し時に実行される非同期タスクです。</param>
        internal ProgressTask(
            Func<ProgressTaskHandler, Task<T>> launcher
        )
        {
            Cts = new CancellationTokenSource();
            TaskLazy = new Lazy<Task<T>>(async () =>
            {
                var handler = new ProgressTaskHandler(this);
                return await launcher(handler);
            });
        }

        /// <summary>
        /// 現在のスレッドの  SynchronizationContext を使用してタスクを起動します。
        /// 起動済みの場合は、過去に起動したタスクを返します、
        /// </summary>
        /// <return>起動したタスク</return>
        internal Task<T> StartIfNeeded()
        {
           return TaskLazy.Value;
        }


        /// <inheritdoc/>
        public void Cancel()
        {
            Cts.Cancel();
        }

        /// <inheritdoc/>
        public async Task JoinAsync(Action<Exception> exceptionHandler = null)
        {
            try
            {
                await Task;
            } catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }
        }
    }
}
