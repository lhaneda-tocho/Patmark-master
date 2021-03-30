using System;
using System.Threading.Tasks;
using Monad;
namespace TokyoChokoku.Communication
{
    /// <summary>
    /// MarkinBOXとの接続状況を表します．
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 準備完了．直ちに通信することができます．
        /// </summary>
        Ready,
        /// <summary>
        /// 排他処理していない状態
        /// </summary>
        NotExcluding,
        /// <summary>
        /// Controllerと接続していない状態．
        /// </summary>
        NotConnected
    }



    /// <summary>
    /// 通信可能であるか，
    /// </summary>
    public enum ExcludingState
    {
        /// <summary>
        /// 排他処理を行い，自分自身以外が通信できないようにしている状態．
        /// </summary>
        ExcludingOther,
        /// <summary>
        /// 排他されていて通信できない or まだ排他していない状態．
        /// </summary>
        NotExcluding
    }


    public static class ConnectionStateExt
    {
        public static bool IsOnline (this ConnectionState it)
        {
            return it != ConnectionState.NotConnected; 
        }

        public static bool IsOffline (this ConnectionState it)
        {
            return it == ConnectionState.NotConnected;
        }

        public static bool Ready (this ConnectionState it)
        {
            return it == ConnectionState.Ready;
        }

        public static bool NotExcluding (this ConnectionState it)
        {
            return it == ConnectionState.NotExcluding;
        }

        public static bool SwitchedOnlineToOffline (this ConnectionState it, ConnectionState next)
        {
            return it.IsOnline () && next.IsOffline ();
        }

        public static bool SwitchedOfflineToOnline (this ConnectionState it, ConnectionState next)
        {
            return it.IsOffline () && next.IsOnline ();
        }

        public static bool ChangedState (this ConnectionState it, ConnectionState next)
        {
            return it != next;
        }


        /// <summary>
        /// 通信可能な状態に限り，通信関係のロジックを実行します．
        /// </summary>
        /// <returns>The current state.</returns>
        public static async Task ProcessCommunication (this ConnectionState it, Func<Task> communication)
        {
            if (it.Ready ()) {
                await communication ();
            }
        }

        /// <summary>
        /// 通信可能な状態に限り，通信関係のロジックを実行します．
        /// </summary>
        /// <returns>The current state.</returns>
        public static async Task <Option<T>> ProcessCommunication <T> (this ConnectionState it, Func<Task<T>> communication)
        {
            if (it.Ready ()) {
                var ans = await communication();
                return Option.Return(() => ans);
            }
            return Option.Nothing<T>();
        }
    }
}
