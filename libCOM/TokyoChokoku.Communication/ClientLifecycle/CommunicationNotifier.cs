using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; 

namespace TokyoChokoku.Communication
{
    [Obsolete("ConnectionStateObserver, ConnectionStateEventSenderを利用してください.", true)]
    public class CommunicationNotifier
    {
        public delegate void OnConnectedDelegate();
        public delegate void OnDisconnectedDelegate();
        public delegate void OnConnectionStatusChangedDelegate(ConnectionState state, ConnectionState preState);

        public event OnConnectedDelegate OnConnected;
        public event OnDisconnectedDelegate OnDisconnected;
        public event OnConnectionStatusChangedDelegate OnConnectionStatusChanged;

        ConnectionState state = ConnectionState.NotConnected;
        public ConnectionState State { get { return state; } }

        int Interval;
        CancelCallback cancelCallback = null;
        object theLock = new object();

        public CommunicationNotifier(int interval)
        {
            Interval = interval;
        }

        public bool Start()
        {
            CancelCallback cancelable;
            // Startの準備. (すでにスタートしている場合は false となる)
            if (!CheckAndReady(out cancelable))
                return false;
            Feedback(cancelable);
            return true;
        }

        /**
         * 開始
         */ 
        public Task<bool> LazyStart(int interval, int repeat = 3)
        {
            return Task.Run(async ()=>{
                CancelCallback cancelable;
                for (int i = 0; i < repeat; i++) {
                    if (CheckAndReady(out cancelable)) {
                        // 成功した場合
                        Feedback(cancelable);
                        return true;
                    }
                    // 失敗した場合
                    await Task.Delay(interval);
                }
                return false;
            });

        }

        public Task<bool> LazyStart() {
            return LazyStart((int)(Interval*1.2));
        }

        public void SendMessageToListener(String message) {
            
        }

        void Feedback(CancelCallback cancelable)
        {
            Task.Run(async () =>
            {
                var client = CommunicationClient.Instance;

                // polling
                var preState = state;
                state = client.GetCurrentState();
                Console.WriteLine("Feedback Tick");
                if (preState.SwitchedOfflineToOnline(state) && OnConnected != null)
                {
                    Console.WriteLine(Thread.CurrentThread.ToString() + ": Connected.");
                    OnConnected();
                }
                if (preState.SwitchedOnlineToOffline(state) && OnDisconnected != null)
                {
                    Console.WriteLine(Thread.CurrentThread.ToString() + ": Disconnected.");
                    OnDisconnected();
                }
                if (preState.ChangedState(state) && OnConnectionStatusChanged != null)
                {
                    Console.WriteLine(Thread.CurrentThread.ToString() + ": State Changed.");
                    OnConnectionStatusChanged(state, preState);
                }
                if (cancelable.isCanceled)
                {
                    CancelAccepted();
                    return;
                }
                await Task.Delay(Interval);
                if (cancelable.isCanceled)
                {
                    CancelAccepted();
                    return;
                }
                Feedback(cancelable);
            });
        }

        public void Stop()
        {
            lock (theLock)
            {
                if(cancelCallback != null)
                    cancelCallback.cancel();
            }
        }

        void CancelAccepted()
        {
            lock(theLock)
            {
                cancelCallback = null;
            }            
        }

        bool CheckAndReady(out CancelCallback callback) {
            // 実行可能かどうか確認
            bool started;
            lock (theLock)
            {
                // check started
                started = cancelCallback != null;
                // ready to start
                if (!started)
                {
                    cancelCallback = new CancelCallback();
                    callback = cancelCallback;
                } else {
                    callback = null;
                }
            }
            if (started)
            {
                Console.WriteLine("Already started yet.");
                return false;
            }
            return true;
        }


        private class CancelCallback {
            public bool isCanceled { get; private set; } = false;
            public void cancel() {
                isCanceled = true;
            }
        }
    }
}


