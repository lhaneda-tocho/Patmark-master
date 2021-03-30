using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TokyoChokoku.WorkerThread
{


    /// <summary>
    /// 単一スレッドのタスク受付クラスです。
    /// </summary>
    public class SingleThreadPoolWorker : SingleThreadWorker
    {
        /// <summary>
        /// ロガーです。
        /// </summary>
        static NLog.Logger Logger { get; }
            = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// スレッド名用の採番フィールドです。コンストラクタのみから参照されます。
        /// </summary>
        static int sequencer = 0;

        /// <summary>
        /// タスク受付キューです。
        /// </summary>
        BlockingCollection<Action> queue
            = new BlockingCollection<Action>();

        /// <summary>
        /// 起動したスレッドです。
        /// </summary>
        internal Thread WorkerThread { get; }

        /// <summary>
        /// スレッド名です。
        /// </summary>
        public override string Name => WorkerThread.Name;

        /// <summary>
        /// ワーカースレッドが動いている間は true となります。StopRequest が呼ばれると、一定時間後に、Disposeメソッドが呼び終わるまえに false となります。
        /// </summary>
        public bool IsAlive {
            get {
                return WorkerThread.IsAlive;
            }
        }

        /// <summary>
        /// スレッドを起動し、タスク受付を始めます。
        /// スレッドはバックグラウンドスレッドとして起動されます。
        /// </summary>
        /// <param name="name">
        /// スレッド名です。主にデバッグ時に利用するもので、未キャッチの例外時にエラーログで表示されます。
        /// null、もしくは空白の場合は "SingleThreadWorker-0", "SingleThreadWorker-1", "SingleThreadWorker-2" ... 
        /// という名前が代わりにつけられます。
        /// </param>
        public SingleThreadPoolWorker(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = string.Format("SingleThreadPoolWorker-{0}", Interlocked.Increment(ref sequencer));
            WorkerThread = new Thread(Proceed);
            WorkerThread.Name = name;
            WorkerThread.IsBackground = true;
            Execute<object>(() => {
                // 空のタスクを入れておく。
                return null;
            });
            WorkerThread.Start();
        }

        void Proceed()
        {
            SetSynchronizationContext(this);
            try
            {
                while (true)
                {
                    var action = queue.Take();
                    action();
                }
            }
            catch (InvalidOperationException)
            {
                // Complete
            }
        }

        /// <summary>
        /// 停止を要求します。停止するのを待たずにこのメソッドを脱出します。
        /// </summary>
        public void StopRequest()
        {
            queue.CompleteAdding();
        }

        public override Task<R> Execute<R>(Func<R> func)
        {
            try
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
                        Logger.Error(e, String.Format("the exception handled by {0}", Name));
                    }
                });
                return task.Task;
            } catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Worker thread {WorkerThread.Name} stopped.");
            }
        }

        public override void Dispose()
        {
            StopRequest();
            if (!object.ReferenceEquals(this.WorkerThread, Thread.CurrentThread))
                WorkerThread.Join();
        }
    }
}
