using System;
using System.Threading.Tasks;


namespace TokyoChokoku.Communication
{
    public enum PipeState: int
    {
        Release,
        Opening,
        Open,
        Releasing
    }

    public enum ExclusionError: int
    {
        BrokenPipe,
        ExcludedYet
    }

    public abstract class PipeStateManager
    {
        readonly object                      TheLock = new object();
        public    ConnectionStateEventSender TheConnectionStateEventSender = new ConnectionStateEventSender();
        protected LineObserver               TheLineObserver; // inject
        TerminationHook                      TheHook = new TerminationHook();

        // Online -> true / Offlin -> false となる.
        // 通信してよいかどうかを表す変数です.
        public             bool   Enable => enable;
        protected volatile bool   enable = false;


        /// <summary>
        /// 現在の状態
        /// </summary>
        /// <value>The state of the current.</value>
        public PipeState CurrentState => (PipeState)walker.State;
        protected AtomicIntState walker = new AtomicIntState((int)PipeState.Release);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executor">Executor.</param>
        /// <param name="lineObserver">Line observer.</param>
        protected PipeStateManager(LineObserver lineObserver)
        {
            // inject
            TheLineObserver = lineObserver ?? throw new NullReferenceException();
            // submit
            TheLineObserver.OnStateChanged += OnLineStateChanged;
        }

        /// <summary>
        /// 通信開始時に呼ばれる. 成功する保証はなく, 失敗したらfalseが返される.
        /// </summary>
        public async Task<bool> OnStartCommunication()
        {
            TaskCompletionSource<object> tcs;

            // Offlineの場合は処理を始めないようにする.
            // 注: Enableで条件分岐しないと, ConnectionStateEventSenderのイベント発行に失敗します.
            if (!enable)
                return false;

            try
            {
                lock (TheLock)
                {
                    // 状態遷移を試みる (失敗したら例外を起こすメソッド)
                    walker.ShouldGetAndWalk(to: (int)PipeState.Opening, from: (int)PipeState.Release);
                    // 成功したらCompletionHookを登録する
                    tcs = TheHook.Register();
                }
            } 
            catch (StateWalkException ex)
            {
                // 状態遷移に失敗した場合 (マルチスレッドでこのメソッドが呼ばれると発生する)
                Console.Error.WriteLine("Blocked to WalkState in PipeStateManager");
                Console.Error.WriteLine(ex);
                return false;
            }

            // 成功 -> 状態遷移を管理する義務が発生
            bool manage = true;
            try{
                var success = await CallStart();
                if (!success)
                    return false;
                // 先にコールバックを呼ぶ
                OnSuccess(PipeState.Open);
                // 終了通知
                TheHook.Complete(tcs);
                // 成功したら状態遷移. ここで状態管理の義務が消失.
                walker.GetAndWalk(to: (int)PipeState.Open);
                manage = false;
                return true;
            }
            catch(Exception ex)
            {
                // Exclusion内でエラーが発生した時
                Console.Error.WriteLine("Error Exclusion in PipeStateManager");
                Console.Error.WriteLine(ex);
                OnFailExclusion(PipeState.Opening, ExclusionError.BrokenPipe);
            }
            finally
            {
                // 状態遷移が必要な場合
                if (manage)
                {
                    // 終了通知
                    TheHook.Complete(tcs);
                    walker.GetAndWalk(to: (int)PipeState.Release);
                }
            }
            return false;
        }


        /// <summary>
        /// 通信終了時に呼ばれる. 成功する保証はなく, 失敗したらfalseが返される.
        /// </summary>
        public async Task<bool> OnTerminateCommunication()
        {
            TaskCompletionSource<object> tcs;

            // Offlineの場合は処理を始めないようにする.
            // 注: Enableで条件分岐しないと, ConnectionStateEventSenderのイベント発行に失敗します.
            if (!enable)
                return false;

            try {
                lock (TheLock)
                {
                    // 状態遷移を試みる (失敗したら例外を起こすメソッド)
                    walker.ShouldGetAndWalk(to: (int)PipeState.Releasing, from: (int)PipeState.Open);
                    // 成功したらCompletionHookを登録する
                    tcs = TheHook.Register();
                }
            }
            catch (StateWalkException ex)
            {
                // 状態遷移に失敗した場合 (マルチスレッドでこのメソッドが呼ばれると発生する)
                Console.Error.WriteLine("Blocked to WalkState in PipeStateManager");
                Console.Error.WriteLine(ex);
                return false;
            }

            try{
                // 排他解除を試みる
                await CallTerminate();
                return true;
            }
            catch (Exception ex)
            {
                // Exclusion内でエラーが発生した時
                Console.Error.WriteLine("Error Release Exclusion in PipeStateManager");
                Console.Error.WriteLine(ex);
                OnFailExclusion(PipeState.Releasing, ExclusionError.BrokenPipe);
                return false;
            }
            finally
            {
                // 成功しようが失敗しようがコールバックを呼ぶ (切断の処理に失敗しただけで, 排他解除としては成功扱いとするため)
                OnSuccess(PipeState.Release);
                // 終了通知
                TheHook.Complete(tcs);
                walker.GetAndWalk(to: (int)PipeState.Release);
            }
        }

        /// <summary>
        /// 排他処理に成功したら呼び出されます. (ここで例外を起こさないこと)
        /// </summary>
        /// <param name="next">Next.</param>
        void OnSuccess(PipeState next)
        {
            try
            {
                if (next == PipeState.Open)
                {
                    TheConnectionStateEventSender.OnOpen();
                }
                else if (next == PipeState.Release)
                {
                    TheConnectionStateEventSender.OnRelease();
                }
            } catch(Exception ex)
            {
                Console.Error.WriteLine("Error on OnSuccess " + Enum.GetName(typeof(PipeState), next));
                Console.Error.WriteLine(ex);
            }
        }

        /// <summary>
        /// 接続状況が変化した時に呼ばれる. 
        /// </summary>
        /// <param name="next">Next.</param>
        async Task OnLineStateChanged(LineState next)
        {
            if (next == LineState.Offline) 
                await OnLostConnection();
            else
                OnFindConnection();
        }

        /// <summary>
        /// Onlineになった時に呼び出される
        /// このタスクが終了するまで接続状態は更新されません.
        /// </summary>
        void OnFindConnection()
        {
            // 接続を有効にする
            enable = true;
            TheConnectionStateEventSender.OnOnline();
        }

        /// <summary>
        /// Offlineになった時に呼ばれる.
        /// このタスクが終了するまで接続状態は更新されません.
        /// </summary>
        /// <returns>The lost connection.</returns>
        async Task OnLostConnection()
        {
            // このタスクが終わるまでは Offline 状態であることが保証されているとする.
            // この条件下の場合, OnLostCommunicationは 
            // OnStartCommunication, OnTerminateCommunication よりも後に呼び出されることが保証される.
            // さらに，OnLostCommunication が実行中は 
            // OnStartCommunication, OnTerminateCommunication が実行できないことが保証される.


            // 接続を禁止する
            enable = false;
            // 先に状態遷移を通知する
            TheConnectionStateEventSender.OnOffline();

            while (true)
            {
                // Open -> Release への状態遷移を試みる
                Tuple<bool, int> result;
                lock (TheLock)
                {
                    result = walker.GetAndTryWalk(to: (int)PipeState.Release, from: (int)PipeState.Open);
                }
                var before = (PipeState)result.Item2;

                // 前の状態に応じて分岐
                switch (before)
                {
                    case PipeState.Open:
                        // 状態遷移に成功 -> 終了 (前提条件A: OfflineでOpenだった)
                        return;

                    case PipeState.Release:
                        // 状態遷移が不要だった -> 終了 (前提条件B: OfflineでReleaseだった)
                        return;

                    case PipeState.Releasing:
                        // 終了処理中だった -> 終了まで待機する (前提条件C: OfflineでReleasingだった)
                        // (終了処理で確実にReleaseに遷移するのでこれを利用する)
                        await TheHook.Hook();
                        return;

                    case PipeState.Opening:
                        // 開始処理中だった -> 処理が終わるまで待機し，繰り返し. (前提条件D: OfflineでOpeningだった)
                        await TheHook.Hook();
                        break;
                }
            }
        }
       

        protected void OnFailExclusion(PipeState kind, ExclusionError error)
        {
            try
            {
                TheConnectionStateEventSender.FireFailExclusionEvent(kind, error);
            } catch(Exception ex) {
                Console.Error.WriteLine("Error in OnFailExclusion Callback");
                Console.Error.WriteLine(ex);
            }
        }

        /// <summary>
        /// 通信を開始ために排他開始のコマンドを送信します.
        /// </summary>
        /// <returns>排他に成功した場合は true, 失敗した時は false.</returns>
        /// <exception cref="System.IO.IOException">通信エラーが発生した時.</exception>
        /// <exception cref="InvalidOperationException">通信できる状態でない時.</exception>
        protected abstract Task<bool> CallStart();


        /// <summary>
        /// 通信を終了するために排他解除のコマンドを送信します.
        /// </summary>
        /// <exception cref="System.IO.IOException">通信エラーが発生した時.</exception>
        /// <exception cref="InvalidOperationException">通信できる状態でない時.</exception>
        protected abstract Task CallTerminate();
    }


    public class PipeStatemanagerWithCommandExecutor : PipeStateManager
    {
        CommandExecutor TheExecutor; // inject
        
        public PipeStatemanagerWithCommandExecutor(LineObserver lineObserver, CommandExecutor executor) : base(lineObserver)
        {
            // inject
            TheExecutor = executor ?? throw new NullReferenceException();
        }


        /// <summary>
        /// コントローラから排他状態を取得します.
        /// </summary>
        /// <returns>The excluding value.</returns>
        /// <exception cref="System.IO.IOException">通信エラーが発生した時.</exception>
        /// <exception cref="InvalidOperationException">通信開始できる状態でない時. (Enableがfalse)</exception>
        public async Task<int> RetrieveExcludingValue()
        {
            if (!enable)
                throw new InvalidOperationException("Disabled");
            try
            {
                var response = await TheExecutor.ReadExclusion(enableBeforeExcluding: true);
                if (response.IsOk)
                {
                    int ans = response.Value;
                    return ans;
                } else {
                    throw new System.IO.IOException("Response error: ReadExclusion");
                }
            }
            catch (System.IO.IOException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new System.IO.IOException("Fail to retrieve excluding value", ex);
            }
        }


        protected override async Task<bool> CallStart()
        {
            // 排他状態の取得を試みる (失敗したら IOException or InvalidOperationException)
            int ev = await RetrieveExcludingValue();
            if (ev != 0)
            {
                // 他の端末が排他している場合は失敗
                OnFailExclusion(PipeState.Opening, ExclusionError.ExcludedYet);
                return false;
            }
            // 排他を試みる (失敗したら例外)
            try
            {
                var res = await TheExecutor.SetExclusion(true, enableBeforeExcluding: true);
                if (res.IsOk)
                    return true;
                else
                    throw new System.IO.IOException("Response error: SetExclusion");
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch(Exception ex) {
                // 通信エラーは IOExceptionに変換
                throw new System.IO.IOException("Error on Excluding", ex);
            }
        }


        protected override async Task CallTerminate()
        {
            try
            {
                var res = await TheExecutor.SetExclusion(false);
                if (!res.IsOk)
                    throw new System.IO.IOException("Response error: SetExclusion");
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // 通信エラーは IOExceptionに変換
                throw new System.IO.IOException("Error on Unexcluding", ex);
            }
        }
    }
}
