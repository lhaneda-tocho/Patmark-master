using System;
using System.Threading;
using System.Threading.Tasks;

namespace TokyoChokoku.Communication
{
    public class CommunicationClient
    {
        /// <summary>
        /// MarkinBOXと接続せずに通信を開始しようとした場合に発生します．
        /// </summary>
        public class NotConnectedException : Exception { }


        volatile static CommunicationClient _Instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static CommunicationClient Instance { get { return _Instance; } }


        public   CommandExecutor                    CommandExecutor => commandExecutorHolder.Value;
        readonly Lazy<CommandExecutor>              commandExecutorHolder;

        /// <summary>
        /// Gets the command task manager.
        /// </summary>
        /// <value>The command task manager.</value>
        public   CommandTaskManager                 CommandTaskManager => commandTaskManagerHolder.Value;
        readonly Lazy<CommandTaskManagerWithClient> commandTaskManagerHolder;

        /// <summary>
        /// Gets the line observer.
        /// </summary>
        /// <value>The line observer.</value>
        public   LineObserver                       LineObserver => lineObserverHolder.Value;
        readonly Lazy<LineObserver>                 lineObserverHolder;

        /// <summary>
        /// Gets the pipe state manager.
        /// </summary>
        /// <value>The pipe state manager.</value>
        public   PipeStateManager                   PipeStateManager => pipeStateManagerHolder.Value;
        readonly Lazy<PipeStateManager>             pipeStateManagerHolder;

        /// <summary>
        /// Gets the connection state event sender.
        /// </summary>
        /// <value>The connection state event sender.</value>
        public   ConnectionStateEventSender         ConnectionStateEventSender => connectionStateEventSenderHolder.Value;
        readonly Lazy<ConnectionStateEventSender>   connectionStateEventSenderHolder;

        public   LineState LineState => LineObserver.CurrentState;
        public   PipeState PipeState => PipeStateManager.CurrentState;

        /// <summary>
        /// 排他処理の状態
        /// </summary>
        /// <value>The state of the current exclusing.</value>
        ExcludingState          CurrentExclusingState { get; set; } = ExcludingState.NotExcluding;
        ICommunicatableSupplier ClientFactory         { get; set; }
        ICommunicationChecker   Checker               { get; set; }


        /// <summary>
        /// Gets or sets the coder assets.
        /// </summary>
        /// <value>The coder assets.</value>
        public EndianFormatter Formatter { get; } = new MarkinBoxEndianFormatter();


        /// <summary>
        /// 通信できる時 true を返します．
        /// </summary>
        /// <value><c>true</c> if ready; otherwise, <c>false</c>.</value>
        public bool Ready
        {
            get
            {
                return PipeStateManager.CurrentState == PipeState.Open;
            }
        }


        CommunicationClient(ICommunicatableSupplier factory, ICommunicationChecker checker, EndianFormatter formatter)
        {
            Checker = checker;
            ClientFactory = factory;
			commandTaskManagerHolder = new Lazy<CommandTaskManagerWithClient>(() =>
			{
				return new CommandTaskManagerWithClient(this);
			});
			commandExecutorHolder = new Lazy<CommandExecutor>(() =>
			{
				return new CommandExecutor(this, formatter, CommandTaskManager);
            });
            connectionStateEventSenderHolder = new Lazy<ConnectionStateEventSender>(() =>
            {
                return PipeStateManager.TheConnectionStateEventSender;
            });
            lineObserverHolder = new Lazy<LineObserver>(() => 
            {
                return new LineObserverWithPolling(Checker);
            });
            pipeStateManagerHolder = new Lazy<PipeStateManager>(() =>
            {
                return new PipeStatemanagerWithCommandExecutor(LineObserver, CommandExecutor);
            });
            Formatter = formatter;
		}

        /// <summary>
        /// init (or re-init) CommunicationClient for MarkinBox
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="checker">Checker.</param>
        public static void InitMarkinBoxClient(ICommunicatableSupplier factory, ICommunicationChecker checker)
        {
            _Instance = new CommunicationClient(factory, checker, new MarkinBoxEndianFormatter());
        }

        /// <summary>
        /// init (or re-init) CommunicationClient for MarkinBox
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="checker">Checker.</param>
        public static void InitPatmarkClient(ICommunicatableSupplier factory, ICommunicationChecker checker)
        {
            _Instance = new CommunicationClient(factory, checker, new PatmarkEndianFormatter());
        }


        /// <summary>
        /// Creates the command executor.
        /// </summary>
        /// <returns>The command executor.</returns>
        [Obsolete("CommunicationClient.CommandExecutorプロパティを利用してください. このメソッドは互換性のために残しています.")]
        public CommandExecutor CreateCommandExecutor()
        {
            return commandExecutorHolder.Value;
        }

        /// <summary>
        /// Creates the communication object.
        /// </summary>
        /// <returns>The communicatable.</returns>
		public ICommunicatable CreateCommunicatable()
        {
			return this.ClientFactory.Create ();
		}

        /// <summary>
        /// 接続状況の監視を開始します.
        /// </summary>
        public void StartObserveLineState() {
            var ph = PipeStateManager; // インスタンス化
            LineObserver.Start();
        }

        /// <summary>
        /// 接続状況の監視を終了します.
        /// </summary>
        public void StopObserveLineState() {
            var ph = PipeStateManager;
            LineObserver.Stop();
        }

        /// <summary>
        /// check client is online.
        /// </summary>
        /// <returns><c>true</c>, if online was ised, <c>false</c> otherwise.</returns>
        public bool IsOnline(){
            return PipeStateManager.Enable;
        }


        /// <summary>
        /// 排他処理の状態を返します．
        /// </summary>
        /// <returns>The excluding state.</returns>
        public ExcludingState GetExcludingState ()
        {
            return PipeState == PipeState.Open ? 
                                   ExcludingState.ExcludingOther : 
                                   ExcludingState.NotExcluding;
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
            return await PipeStateManager.OnStartCommunication();
        }

        /// <summary>
        /// 排他を解除します．
        /// このタスクは スレッド安全ではありません．
        /// コントローラと接続していない場合，このタスクは何もせずに処理を終了します．
        /// </summary>
        /// <returns>The excluding other.</returns>
        public async Task<bool> FreeController ()
        {
            return await PipeStateManager.OnTerminateCommunication();
        }

        /// <summary>
        /// 排他状態を読み出します．
        /// </summary>
        /// <returns>The excluding.</returns>
        async Task<bool> GetExcluding ()
        {
            try
            {
                var value = await (PipeStateManager as PipeStatemanagerWithCommandExecutor).RetrieveExcludingValue();
                return 0 != value;
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 排他状態を設定します．
        /// </summary>
        /// <returns>The excluding.</returns>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        [Obsolete("TryExcludingOther, FreeControllerを利用したください.")]
        async Task<bool> SetExcluding (bool enabled)
        {
            if (enabled)
                return await TryExcludingOther();
            else
                return await FreeController();
        }

        /// <summary>
        /// 接続状況を1つの列挙型で表現し，返します
        /// </summary>
        /// <returns>The current state.</returns>
        [Obsolete()]
        public ConnectionState GetCurrentState ()
        {
            var mng = PipeStateManager;
            var enable = mng.Enable;
            var state  = mng.CurrentState;
            if(enable) {
                return state == PipeState.Open ? ConnectionState.Ready : ConnectionState.NotExcluding;
            } else {
                return ConnectionState.NotConnected;
            }
        }

        /// <summary>
        /// こちらは表示用です。
        /// </summary>
        /// <returns>The rom version.</returns>
        public async Task<RomVersion?> RetrieveRomVersion()
        {
            if (Ready)
            {
                var exe = CommandExecutor;
                var rev = (UInt16)(await exe.ReadRevision()).Value;
                var ver = (UInt16)(await exe.ReadVersion()).Value;
                return (RomVersion?)RomVersion.Deformat(ver, rev);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// こちらは機種判定用です。
        /// </summary>
        /// <returns>The rom version raw.</returns>
        public async Task<RomVersion?> RetrieveRomVersionForMachineModel()
        {
            if (Ready)
            {
                var exe = CommandExecutor;
                var rev = (UInt16)(await exe.ReadRevision()).Value;
                var ver = (UInt16)(await exe.ReadVersion()).Value;
                return (RomVersion?)RomVersion.DeformatForMachineModel(ver, rev);
            }
            else
            {
                return null;
            }
        }
    }
}

