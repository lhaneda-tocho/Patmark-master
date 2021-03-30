using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{

    /// <summary>
    /// 非同期処理の起動を行い、プログレスバーの表示を指令する機能へアクセスできるようにします。
    /// </summary>
    public class ProgressAPI: IDisposable
    {

        // ====================

        /// <summary>
        /// UIスレッドで動くワーカです
        /// </summary>
        ProgressTaskWorker     Main { get; }

        /// <summary>
        /// タスクコントローラです。
        /// </summary>
        ProgressTaskController TaskController { get; }

        /// <summary>
        /// タスク監視のキャンセルに使用するオブジェクトです
        /// </summary>
        CancellationTokenSource TaskObserverCancellationTokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// タスク監視のタスクです。
        /// </summary>
        Task ObserverTask { get; }

        /// <summary>
        /// ワーカを指定して ProgressAPI を初期化します。
        /// この初期化時に、TaskController のタスク監視処理も起動します。
        /// </summary>
        /// <param name="mainWorker">UIスレッドでタスクを処理するワーカを指定します。</param>
        public ProgressAPI(
            ProgressTaskWorker mainWorker
        )
        {
            Main = mainWorker ?? throw new ArgumentNullException(nameof(mainWorker));
            TaskController = new ProgressTaskController();
            ObserverTask = TaskController.StartObserve(TaskObserverCancellationTokenSource.Token);

#if ENABLE_THREAD_CHECK
            Console.WriteLine(@$"==== Initialized Progress API ====
    Main = {Main}
    Main.CurrentThreadUsing = {Main.CurrentThreadUsing()} // <-- 2020/05/01  これを2回呼ぶと Xamarin がクラッシュする。
");
#else
            Console.WriteLine(@$"==== Initialized Progress API ====
    Main = {Main}
    Main.CurrentThreadUsing =  <<DISABLED!!>>
");
#endif
        }

        /// <summary>
        /// タスクの監視を終了します。
        /// これ以降に <c>Dispose</c> 以外の ProgressAPI のメソッドを呼び出した場合の動作は未定義となります。
        ///
        /// このメソッドは UI スレッドから呼び出すことができます。
        /// </summary>
        public void Dispose()
        {
#if ENABLE_THREAD_CHECK
            if (Main.CurrentThreadUsing())
#endif
                TaskObserverCancellationTokenSource.Cancel();
#if ENABLE_THREAD_CHECK
            throw new InvalidOperationException("Invocation on non ui thread.");
#endif
        }

        /// <summary>
        /// タスクの監視を終了します。
        /// これ以降に ProgressAPI のメソッドを呼び出した場合の動作は未定義となります。
        ///
        /// 破棄処理はメインスレッドで行われます。
        /// </summary>
        public async Task DisposeAsync()
        {
            await Main.Launch<object>(null, _ => {
                Dispose();
                return null;
            });
            try
            {
                await ObserverTask;
            } catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled Exception in Observation: {ex}");
            }
        }

        // ====================

        /// <summary>
        /// ProgressViewController を登録し、イベントを購読できるようにします。
        ///
        /// このメソッドは UI スレッドから呼び出すことができます。
        /// </summary>
        /// <returns>ProgressEventListenerToken. これを Dispose することで、 イベントの購読が解除されます。</returns>
        /// <exception cref="InvalidOperationException">UIスレッド以外からの呼び出しを検知した場合</exception>
        public ProgressEventListenerToken Register(ProgressViewController view)
        {
#if ENABLE_THREAD_CHECK
            if (Main.CurrentThreadUsing())
#endif
                return TaskController.AddViewController(Main, view);
#if ENABLE_THREAD_CHECK
            throw new InvalidOperationException("Invocation on non ui thread.");
#endif
        }


        // ====================

        /// <summary>
        /// タスクを起動します。
        ///
        /// タスクの状態遷移と通知はUIスレッド上で行われます。
        /// タスクの実行は, 引数に指定したワーカ上で行います。
        ///
        /// <c>task</c> で例外がスローされた場合は、回復処理を行った後、例外を再スローします。
        ///
        /// このメソッドはスレッドセーフです。登録処理はメインスレッド上で,  非同期処理は 引数に指定された worker で行われます。
        /// </summary>
        /// <param name="worker">
        ///     <c>task</c>を実行するスレッドを指定します。
        ///     <c>null</c> を指定した場合は、UIスレッド上で起動されます。
        /// </param>
        /// <param name="block">
        ///     起動する処理です。
        /// </param>
        /// <returns>起動したタスク</returns>
        public Task<R> LaunchTaskAsync<R>(ProgressTaskWorker worker, Func<ProgressTaskHandler, Task<R>> block)
        {
            // worker が null の場合は メインスレッド上で起動する。
            worker = worker ?? Main;
            // Launcher 作成 
            var launcher = new ProgressTaskLauncher<R>(worker, block);
            // コントローラへメッセージ送信
            return Main.Launch(launcher, SubmitToControllerAsyncOnMain);
        }


        /// <summary>
        /// タスクを起動します。
        ///
        /// タスクの状態遷移と通知はUIスレッド上で行われます。
        /// タスクの実行は, 引数に指定したワーカ上で行います。
        ///
        /// <c>task</c> で例外がスローされた場合は、回復処理を行った後、例外を再スローします。
        /// 
        /// このメソッドはスレッドセーフです。登録処理はメインスレッド上で,  非同期処理は 引数に指定された worker で行われます。
        /// </summary>
        /// <param name="worker">
        ///     <c>task</c>を実行するスレッドを指定します。
        ///     <c>null</c> を指定した場合は、UIスレッド上で起動されます。
        /// </param>
        /// <param name="block">
        ///     起動する処理です。
        /// </param>
        /// <returns>起動したタスク</returns>
        public Task LaunchTaskAsync(ProgressTaskWorker worker, Func<ProgressTaskHandler, Task> block)
        {
            // worker が null の場合は メインスレッド上で起動する。
            worker = worker ?? Main;
            // Launcher 作成 
            var launcher = new ProgressTaskLauncher(worker, block);
            // コントローラへメッセージ送信
            return Main.Launch(launcher, SubmitToControllerAsyncOnMain);
        }


        /// <summary>
        /// コントローラへタスクの管理を委託します。
        /// このメソッドはメインスレッドで呼び出します。
        /// </summary>
        Task<R> SubmitToControllerAsyncOnMain<R>(ProgressTaskLauncher<R> launcher)
        {
            return TaskController.SubmitTask(launcher);
        }




    }
}
