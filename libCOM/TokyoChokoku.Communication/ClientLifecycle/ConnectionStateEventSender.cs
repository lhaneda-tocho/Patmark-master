using System;
using System.Threading;
using System.Threading.Tasks;
namespace TokyoChokoku.Communication
{
    public class ConnectionStateEventSender
    {
        /// <summary>
        /// イベントを登録するSynchronizationContextを指定します.
        /// 指定しなかった場合はスレッドプール上で実行されます.
        /// </summary>
        public SynchronizationContext TheSynchronizationContext { get; set; } = null;
        public ConnectionState CurrentState => (ConnectionState)walker.State;
        AtomicIntState walker = new AtomicIntState((int)ConnectionState.NotConnected);

        /// <summary>
        /// 状態が変化した時に呼び出されます.
        /// </summary>
        public event Action<ConnectionState, ConnectionState> OnStateChanged  = null;
        /// <summary>
        /// 排他処理に失敗した時に呼び出されます.
        /// </summary>
        public event Action<ExclusionError>                   OnFailOpening   = null;
        /// <summary>
        /// 排他解除に失敗した時に呼び出されます.
        /// </summary>
        public event Action<ExclusionError>                   OnFailReleasing = null;

        /// <summary>
        /// Offlineになった時に呼び出す.
        /// </summary>
        /// <returns>The offline.</returns>
        public void OnOffline()
        {
            var before = walker.GetAndWalk((int)ConnectionState.NotConnected);
            PostEvent(()=>OnStateChanged?.Invoke(ConnectionState.NotConnected, (ConnectionState)before));
        }

        /// <summary>
        /// Onlineになったばかりの時に呼び出す
        /// </summary>
        /// <returns>The online.</returns>
        public void OnOnline()
        {
            var tuple = walker.GetAndTryWalk((int)ConnectionState.NotExcluding, (int)ConnectionState.NotConnected);
            if(tuple.Item1)
                PostEvent(()=>OnStateChanged?.Invoke(ConnectionState.NotExcluding, (ConnectionState)tuple.Item2));
        }

        /// <summary>
        /// 排他が解除された時に呼び出す
        /// </summary>
        /// <returns>The release.</returns>
        public void OnRelease()
        {
            var tuple = walker.GetAndTryWalk((int)ConnectionState.NotExcluding, (int)ConnectionState.Ready);
            if (tuple.Item1)
                PostEvent(()=>OnStateChanged?.Invoke(ConnectionState.NotExcluding, (ConnectionState)tuple.Item2));
        }

        /// <summary>
        /// 排他を開始した時に呼び出す
        /// </summary>
        /// <returns>The open.</returns>
        public void OnOpen()
        {
            var tuple = walker.GetAndTryWalk((int)ConnectionState.Ready, (int)ConnectionState.NotExcluding);
            if (tuple.Item1)
                PostEvent(()=>OnStateChanged?.Invoke(ConnectionState.Ready, (ConnectionState)tuple.Item2));

        }

        /// <summary>
        /// 排他処理に失敗した時に呼び出します.
        /// </summary>
        /// <param name="kind">Kind.</param>
        public void FireFailExclusionEvent(PipeState kind, ExclusionError error)
        {
            if(kind == PipeState.Opening) {
                PostEvent(()=>OnFailOpening?.Invoke(error));
            } else if(kind == PipeState.Releasing) {
                PostEvent(() => OnFailReleasing?.Invoke(error));
            }
        }

        public void PostEvent(Action task)
        {
            var sc = TheSynchronizationContext;

            if (sc == null)
                // スレッドプールで実行
                Task.Run(task);
            else
                // TheSynchronizationContext に登録
                sc.Post((_)=>task(), null);
        }
    }
}
