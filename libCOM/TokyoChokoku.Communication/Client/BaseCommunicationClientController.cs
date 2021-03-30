using System;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.Communication;

namespace TokyoChokoku.Communication
{
    public enum CommunicationRound
    {
        NotReady,
        Connect,
        Ready,
        Clean
    }

    public abstract class BaseCommunicationClientController: ConnectionStateListener
    {
        protected readonly CommunicationClient TheClient;
        readonly TerminationHook     TheHook = new TerminationHook();



        /// <summary>
        /// Gets the current round.
        /// </summary>
        /// <value>The current round.</value>
        public  CommunicationRound CurrentRound => (CommunicationRound)walker.State;
        private AtomicIntState walker = new AtomicIntState((int)CommunicationRound.NotReady);


        /// <summary>
        /// コンストラクタ
        /// <see cref="T:TokyoChokoku.Communication.Client.BaseCommunicationClientController"/> class.
        /// </summary>
        /// <param name="theClient">The client.</param>
        public BaseCommunicationClientController(CommunicationClient theClient)
        {
            TheClient = theClient ?? CommunicationClient.Instance;
        }


        /// <summary>
        /// 渡されたタスクをバックグラウンドで処理します.
        /// UIのロックの処理もここで行います.
        /// </summary>
        /// <returns>The background.</returns>
        /// <param name="task">Task.</param>
        protected abstract Task RunBackground(Func<Task> task);

        /// <summary>
        /// 通信準備を行う. (排他処理は終了している)
        /// このメソッドで状態遷移する必要はありません.
        /// </summary>
        /// <returns>The start.</returns>
        protected abstract Task DoReady();

        /// <summary>
        /// 通信のキャンセル処理を行う
        /// このメソッドで状態遷移する必要はありません.
        /// </summary>
        /// <returns>The clean.</returns>
        protected abstract Task DoClean();

        /// <summary>
        /// 通信終了時に呼ばれる
        /// </summary>
        /// <returns>The complete.</returns>
        protected abstract Task DidTerminate();


        public virtual void DidSumbit(ConnectionState current)
        {
            // 何もしない
        }

        public virtual void OnStateChanged(ConnectionState next, ConnectionState prev)
        {
            switch(next)
            {
                case ConnectionState.Ready:
                    break;
                case ConnectionState.NotExcluding:
                    // オフライン状態から オンライン状態になる時に限定して
                    // 排他処理を試みる
                    if (prev.SwitchedOfflineToOnline(next))
                    {
                        StartCommunication();
                    }
                    break;
                case ConnectionState.NotConnected:
                    LostConnection();
                    break;
            }
        }

        public virtual void OnFailOpening(ExclusionError error)
        {
            // 何もしない
        }

        public virtual void OnFailReleasing(ExclusionError error)
        {
            // 何もしない
        }

        /// <summary>
        /// 排他状態を開始し，通信の準備を行う. 
        /// 処理開始直前と，完了時にコールバックを呼び出す.コールバック内の処理が終了するまでは 
        /// 通信開始， 通信終了, 接続ロストに割り込まれない. (ただし, 常に通信可能である保証はない)
        /// </summary>
        /// <returns>The communication.</returns>
        public Task StartCommunication()
        {
            return RunBackground(async () =>
            {
                try
                {
                    // 事前条件
                    var success = walker.ShouldGetAndWalk(to: (int)CommunicationRound.Connect, from: (int)CommunicationRound.NotReady);
                }
                catch (StateWalkException)
                {
                    // 状態遷移に失敗したら諦める
                    return;
                }
                // 次の状態遷移までは割り込まれない
                // フックの取得 (TaskCompletionSourceが返る)
                var tcs = TheHook.Register();

                bool fail = true;
                try
                {
                    // 接続を試みる
                    // 失敗したらコールバックが呼ばれるので気にしなくて良い
                    if (await TheClient.TryExcludingOther())
                    {
                        // 排他に成功したら通信の準備
                        fail = false;
                        try {
                            // init
                            await TheClient.CommandExecutor.LoadFileNameAndMapOnSdCardIfNeeded();
                        } finally {
                            // 成功しようが失敗しようが コールバック終了まで待機
                            await DoReady();
                        }
                    }
                }
                finally
                {
                    // 最後に状態遷移
                    if (fail)
                        walker.GetAndWalk(to: (int)CommunicationRound.NotReady);
                    else
                        walker.GetAndWalk(to: (int)CommunicationRound.Ready   );
                    // フックを解除
                    TheHook.Complete(tcs);
                }
            });
        }

        /// <summary>
        /// 通信に関わる処理を全て棄却し，排他状態を解除する. 処理開始直前と，完了時にコールバックを呼び出す. 
        /// コールバック内の処理が終了するまでは 通信開始， 通信終了, 接続ロストに割り込まれない. 
        /// (ただし, 常に通信可能である保証はない)
        /// </summary>
        /// <returns>The communication.</returns>
        public Task TerminateCommunication()
        {
            return RunBackground(async () =>
            {
                try
                {
                    // 事前条件
                    var success = walker.ShouldGetAndWalk(to: (int)CommunicationRound.Clean, from: (int)CommunicationRound.Ready);
                }
                catch (StateWalkException)
                {
                    // 状態遷移に失敗したら諦める
                    return;
                }

                // 次の状態遷移までは割り込まれない
                // フックの取得 (TaskCompletionSourceが返る)
                var tcs = TheHook.Register();

                try {
                    try
                    {
                        // キャンセル処理
                        await DoClean();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error to DoClean");
                        Console.Error.WriteLine(ex);
                    }
                    try
                    {
                        await TheClient.FreeController();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error on FreeController");
                        Console.Error.WriteLine(ex);
                    }

                    try
                    {
                        await DidTerminate();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error on DidTerminate");
                        Console.Error.WriteLine(ex);
                    }
                }
                finally
                {
                    // 状態遷移
                    walker.GetAndWalk(to: (int)CommunicationRound.NotReady);
                    // フックを解除
                    TheHook.Complete(tcs);
                }
            });
        }

        /// <summary>
        /// PMとの接続ができなくなり，排他状態が解除された時に発生する. 通信に関わる処理を全て棄却する.
        /// </summary>
        /// <returns>The connection.</returns>
        public Task LostConnection()
        {
            return RunBackground(async () =>
            {
                while (true)
                {
                    // 事前条件
                    var tuple = walker.GetAndTryWalk(to: (int)CommunicationRound.Clean, from: (int)CommunicationRound.Ready);
                    var prev  = (CommunicationRound)tuple.Item2;

                    // 事前条件判定
                    switch (prev)
                    {
                        // 前の状態が Readyだった -> 状態遷移成功
                        case CommunicationRound.Ready:
                            {
                                // 次の状態遷移までは割り込まれない
                                // フックの取得 (TaskCompletionSourceが返る)
                                var tcs = TheHook.Register();
                                try
                                {
                                    await DoLostConnection();
                                }
                                finally
                                {
                                    // 状態遷移
                                    walker.GetAndWalk(to: (int)CommunicationRound.NotReady);
                                    // フックを解除
                                    TheHook.Complete(tcs);
                                }
                                return;
                            }

                        case CommunicationRound.Clean:
                        case CommunicationRound.NotReady:
                            return;

                        case CommunicationRound.Connect:
                            // 終了まで待機 (suspend)
                            await TheHook.Hook();
                            // 繰り返し (joined)
                            break;
                    }
                }
            });
        }

        async Task DoLostConnection()
        {
            try{
                await DoClean();
            }
                catch (Exception ex)
            {
                Console.Error.WriteLine("Error to DoClean");
                Console.Error.WriteLine(ex);
            }

            try
            {
                await DidTerminate();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error on DidTerminate");
                Console.Error.WriteLine(ex);
            }
        }
    }
}
