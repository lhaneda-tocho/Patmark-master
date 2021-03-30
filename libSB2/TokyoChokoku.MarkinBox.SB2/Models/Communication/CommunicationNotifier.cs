using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class CommunicationNotifier
    {
        public delegate void OnConnectedDelegate();
        public delegate void OnDisconnectedDelegate();
        public delegate void OnConnectionStatusChangedDelegate(ConnectionState state, ConnectionState preState);

        public OnConnectedDelegate OnConnected;
        public OnDisconnectedDelegate OnDisconnected;
        public OnConnectionStatusChangedDelegate OnConnectionStatusChanged;

        ConnectionState state = ConnectionState.NotConnected;


        int Interval;
        bool Alival;

        public CommunicationNotifier (int interval)
        {
            Interval = interval;
        }

        public void Start(){
            Alival = true;
            Task.Run(async () => {
                var client = CommunicationClientManager.Instance;

                try
                {
                    // polling
                    var preState = state;
                    state = client.GetCurrentState();

                    if (preState.SwitchedOfflineToOnline(state) && OnConnected != null)
                    {
                        OnConnected();
                    }
                    if (preState.SwitchedOnlineToOffline(state) && OnDisconnected != null)
                    {
                        OnDisconnected();
                    }
                    if (preState.ChangedState(state) && OnConnectionStatusChanged != null)
                    {
                        OnConnectionStatusChanged(state, preState);
                    }
                    await Task.Delay(Interval);
                } catch (Exception ex)
                {
                    Log.Error($"Unhandled Exception: {ex}");
                }


                if (Alival)
                {
                    Start();
                }
            });
        }

        public void Stop()
        {
            Alival = false;
        }




    }
}

