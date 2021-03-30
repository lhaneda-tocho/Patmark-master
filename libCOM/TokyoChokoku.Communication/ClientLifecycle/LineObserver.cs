using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    using LineEventListener = Func<LineState, Task>;

    /// <summary>
    /// このクラスの持つ状態
    /// </summary>
    public enum LineState : int
    {
        Offline, Online
    }



    /// <summary>
    /// LineStateの変更を通知するイベントセンダ
    /// リスナの処理が終了しているか確認する機能がある.
    /// </summary>
    public class LineEventSender
    {
        object TheLock = new object();

        readonly List<LineEventListener> listenerList = new List<LineEventListener>();

        volatile Int32 ProcessingTaskCount = 0;

        /// <summary>
        /// リスナの処理が全て終了していれば true を返します.
        /// </summary>
        /// <value><c>true</c> if is processing now; otherwise, <c>false</c>.</value>
        public bool IsProcessingNow {
            get {
                return ProcessingTaskCount > 0; 
            }
        }

        /// <summary>
        /// イベントリスナのリストです.
        /// </summary>
        /// <value>The listener list.</value>
        List<LineEventListener> CopyListenerList {
            get {
                lock(TheLock)
                {
                    return new List<LineEventListener>(listenerList);
                }
            }
        }

        /// <summary>
        /// リスナを追加する
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="listener">Listener.</param>
        public void Add(LineEventListener listener)
        {
            lock (TheLock)
            {
                listenerList.Add(listener);
            }
        }

        /// <summary>
        /// リスナを取り除く.
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="listener">Listener.</param>
        public void Remove(LineEventListener listener)
        {
            lock (TheLock)
            {
                listenerList.Remove(listener);
            }
        }

        /// <summary>
        /// イベントの通知を行います. 各イベントはスレッドプール上で実行されます.
        /// 処理中のベントがある場合は例外となります. 呼び出し前に処理中のイベントがないことを確認してください.
        /// このメソッドはスレッドセーフではありません.
        /// </summary>
        /// <exception cref="InvalidOperationException">処理中のベントが残っている場合.</exception>
        public void Fire(LineState next) {
            // 処理が終わっていないのに呼び出そうとした場合はエラー.
            if(IsProcessingNow) {
                throw new InvalidOperationException("Busy");
            }
            // イベントの通知
            var ll = CopyListenerList;
            // 処理中のイベントの数を記録
            ProcessingTaskCount = ll.Count;
            Console.WriteLine("OnTick!! " + ll.Count);
            foreach(var lis in ll) {
                Task.Run(()=> SafeFire(lis, next));
            }
        }

        async Task SafeFire(Func<LineState, Task> task, LineState next)
        {
            try
            {
                // 処理が終わるまで待機
                await task(next);
            }
            catch(Exception ex)
            {
                // ログ出力
                Console.Error.WriteLine("Error in LineEventDispatch");
                Console.Error.WriteLine(ex);
            }
            finally
            {
                // 処理が終了したことを通知
                Interlocked.Decrement(ref ProcessingTaskCount);
            }
        }
    }

    public abstract class LineObserver
    {
        /// <summary>
        /// 現在の状態
        /// </summary>
        /// <value>The state of the current.</value>
        public LineState CurrentState => (LineState)walker.State;
        protected readonly AtomicIntState walker = new AtomicIntState((int) LineState.Offline);

        /// <summary>
        /// EventSender
        /// </summary>
        protected LineEventSender eventSender = new LineEventSender();

        /// <summary>
        /// イベントリスナを登録します.
        /// イベントはC#が確保するスレッドプール上で実行されます.
        /// 登録されたイベントが実行されてから終了するまでの間に通知が来ることはありません.
        /// </summary>
        public event LineEventListener OnStateChanged {
            add {
                eventSender.Add(value);
            }
            remove {
                eventSender.Remove(value);
            }
        }

        public bool IsBusy {
            get {
                return eventSender.IsProcessingNow;
            }
        }


        /// <summary>
        /// 監視を開始します．
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 監視を停止します.
        /// </summary>
        public abstract void Stop();

        public bool IsOnline()
        {
            return CurrentState == LineState.Online;
        }

        public bool IsOffline()
        {
            return CurrentState == LineState.Offline;
        }

        /// <summary>
        /// PMとの接続ができなくなった場合
        /// このメソッドに入った時点で状態遷移は完了しています.
        /// (Atomicに処理するよう実装すること)
        /// </summary>
        protected void OnLostConnection()
        {
            var tuple = walker.GetAndTryWalk((int)LineState.Offline, (int)LineState.Online);
            if (tuple.Item1)
            {
                eventSender.Fire(LineState.Offline);
            }
        }

        /// <summary>
        /// PMと接続できるようになった場合
        /// このメソッドに入った時点で状態遷移は完了しています.
        /// (Atomicに処理するよう実装すること)
        /// </summary>
        protected void OnFoundConnection()
        {
            var tuple = walker.GetAndTryWalk((int)LineState.Online, (int)LineState.Offline);
            if (tuple.Item1)
            {
                eventSender.Fire(LineState.Online);
            }
        }
    }


    class LineObserverWithPolling: LineObserver
    {
        /// <summary>
        /// The checker.
        /// </summary>
        ICommunicationChecker Checker;

        /// <summary>
        /// 監視用のタイマー
        /// </summary>
        EventDispatchTimer DispatchTimer { get; } = new EventDispatchTimer(interval: 1000);

        /// <summary>
        /// The max trial count.
        /// この回数だけ切断の再確認を行う. 3の場合、 ３回連続で切断状態を検知したら offline に状態遷移する.
        /// </summary>
        const int MaxTrialCount = 3;
        int TrialCount = 0; 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LineObserverWithPolling(ICommunicationChecker checker)
        {
            Checker = checker;
            DispatchTimer.OnTick += () => OnTick();
        }

        /// <summary>
        /// 検査と分岐
        /// (イベントディスパッチスレッド上で呼ぶこと. マルチスレッドで呼ばないこと)
        /// </summary>
        void OnTick()
        {
            // OnLostConnection, OnFoundConnectionのアトミック性を保つため,
            // イベント処理が終わるまでイベントの通知を行わないようにする
            if (eventSender.IsProcessingNow)
                return;

            // 実際に接続状態を確認する処理
            // 
            if (Checker.IsConnectable())
            {
                // 接続可能
                TrialCount = 0; // reset
                OnFoundConnection();
            }
            else
            {
                // 切断
                var cnt = TrialCount++;
                if (cnt >= MaxTrialCount)
                {
                    TrialCount = 0; // reset
                    OnLostConnection();
                }
            }
        }


        /// <summary>
        /// 監視を開始します．
        /// </summary>
        public override void Start()
        {
            DispatchTimer.Start();
        }

        /// <summary>
        /// 監視を停止します.
        /// </summary>
        public override void Stop()
        {
            DispatchTimer.Stop();
        }

    }
}
