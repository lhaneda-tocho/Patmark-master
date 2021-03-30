using System;
using System.Threading;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public class CommunicationClientManager
	{
        /// <summary>
        /// MarkinBOXと接続せずに通信を開始しようとした場合に発生します．
        /// </summary>
        public class NotConnectedException : Exception { }

        static CommunicationClientManager _Instance;
		public static CommunicationClientManager Instance { get { return _Instance; } }



        CommunicationClientManager(ICommunicationClientFactory factory, ICommunicationChecker checker)
        {
            this.Checker = checker;
            this.ClientFactory = factory;

            //this.StartWatching();
        }

        public static void Init(ICommunicationClientFactory factory, ICommunicationChecker checker)
        {
            _Instance = new CommunicationClientManager(factory, checker);
        }

        /// <summary>
        /// 排他処理の状態
        /// </summary>
        /// <value>The state of the current exclusing.</value>
        ExcludingState CurrentExclusingState { get; set; } = ExcludingState.NotExcluding;

        ICommunicationClientFactory ClientFactory;
        ICommunicationChecker Checker;



		public ICommunicationClient CreateClient(){
			return this.ClientFactory.Create ();
		}

        public bool IsOnline(){
            return this.Checker.IsConnectable ();
        }

        /// <summary>
        /// 排他処理の状態を返します．
        /// </summary>
        /// <returns>The excluding state.</returns>
        public ExcludingState GetExcludingState ()
        {
            return CurrentExclusingState;
        }



        /// <summary>
        /// 他の端末の排他を試みます．
        /// このタスクは スレッド安全ではありません．
        /// コントローラと接続していない場合，このタスクは false を返して終了します．
        /// NOTE: 他の端末と同時に 排他処理を行うと この処理は失敗します．(失敗を検知することもできない)
        /// </summary>
        /// <returns>The excluding other.</returns>
        public async Task<bool> TryExcludingOther ()
        {
            if (!IsOnline ())
                return false;

            var excluding = await GetExcluding ();
            if (excluding) { // NOTE: この比較の後，コントローラの排他状態が変わっている可能性がある． 
                // 排他中...
                if (CurrentExclusingState == ExcludingState.ExcludingOther) { // NOTE: スレッド安全ではない 比較
                    // すでに他の端末を排他中の時
                    return true;
                } else {
                    // 他の端末に排他されている時
                    return false;
                }
            } else {
                // 排他されていない時
                await SetExcluding (true);
                CurrentExclusingState = ExcludingState.ExcludingOther;        // NOTE: スレッド安全ではない 書き換え
                return true;
            }
        }

        /// <summary>
        /// 排他を解除します．
        /// このタスクは スレッド安全ではありません．
        /// コントローラと接続していない場合，このタスクは何もせずに処理を終了します．
        /// </summary>
        /// <returns>The excluding other.</returns>
        public async Task FreeController ()
        {
            if (!IsOnline ())
                return;

            // コントローラ側の排他状態
            var excluding = await GetExcluding ();
            
            if (CurrentExclusingState == ExcludingState.ExcludingOther) { // NOTE: スレッド安全ではない 比較
                // 他の端末を排他中 -> 排他の解除
                CurrentExclusingState = ExcludingState.NotExcluding;      // NOTE: スレッド安全ではない 書き換え
                if (excluding)
                    await SetExcluding (false);
                // 他の端末からの書き換えとの競合を抑えるため，排他状態が解除されている時 (異常) は何もしない，
            }
        }

        /// <summary>
        /// 排他状態を読み出します．
        /// </summary>
        /// <returns>The excluding.</returns>
        async Task<bool> GetExcluding ()
        {
            if (!IsOnline()) {
                throw new NotConnectedException ();
            }
            var state = await CommandExecuter.ReadExclusion ();
            return state.Value != 0;
        }

        /// <summary>
        /// 排他状態を設定します．
        /// </summary>
        /// <returns>The excluding.</returns>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        async Task SetExcluding (bool enabled)
        {
            if (!IsOnline()) {
                throw new NotConnectedException ();
            }
            var state = await CommandExecuter.SetExclusion (enabled);
        }

        /// <summary>
        /// 接続状況を1つの列挙型で表現し，返します
        /// </summary>
        /// <returns>The current state.</returns>
        public ConnectionState GetCurrentState ()
        {
            var ins = Instance;
            if (!ins.IsOnline ())
                return ConnectionState.NotConnected;
            if (ins.GetExcludingState () == ExcludingState.NotExcluding)
                return ConnectionState.NotExcluding;
            return ConnectionState.Ready;
        }
    }
}

