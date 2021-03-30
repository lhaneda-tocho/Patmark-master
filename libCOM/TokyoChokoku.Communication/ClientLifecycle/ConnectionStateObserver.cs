using System;
using System.Collections.Generic;
namespace TokyoChokoku.Communication
{
    using ListenerList = List<ConnectionStateListener>;
    using ExclusionErrorCallback = Action<ExclusionError>;
    using ConnectionCallback     = Action<ConnectionState, ConnectionState>;

    public class ConnectionStateObserver
    {
        object                                 TheListenerLock = new object();
        readonly ConnectionStateEventSender    TheSender; // inject
        readonly ListenerList                  ListenerList = new List<ConnectionStateListener>();

        readonly ExclusionErrorCallback OnOpenError;
        readonly ExclusionErrorCallback OnReleaseError;
        readonly ConnectionCallback     OnChanged;

        public ConnectionState CurrentState { get; private set; } = ConnectionState.NotConnected;     


        public ConnectionStateObserver(ConnectionStateEventSender sender)
        {
            TheSender = sender;
            OnChanged      = (next, prev) => FireOnChanged     (next, prev);
            OnReleaseError = (error)      => FireOnFailRelease (error);
            OnOpenError    = (error)      => FireOnFailOpen    (error);

            sender.OnStateChanged  += OnChanged;
            sender.OnFailOpening   += OnOpenError;
            sender.OnFailReleasing += OnReleaseError;
        }

        public void Dispose()
        {
            var sender = TheSender;
            sender.OnStateChanged  -= OnChanged;
            sender.OnFailOpening   -= OnOpenError;
            sender.OnFailReleasing -= OnReleaseError;
        }

        public void AddListener(ConnectionStateListener listener)
        {
            lock(TheListenerLock) {
                ListenerList.Add(listener);
            }
            TheSender.PostEvent(()=>
            {
                // Senderのタスクを処理するスレッド上で実行する
                listener.DidSumbit(CurrentState);
            });
        }

        public void RemoveListener(ConnectionStateListener listener)
        {
            lock(TheListenerLock) {
                ListenerList.Remove(listener);
            }
        }

        public void Purge() {
            lock(TheListenerLock) {
                ListenerList.Clear();
            }
        }

        void FireOnChanged(ConnectionState next, ConnectionState prev)
        {
            ListenerList list;
            CurrentState = next;
            lock (TheListenerLock)
            {
                list = new ListenerList(ListenerList);
            }
            foreach(var l in list) {
                l.OnStateChanged(next, prev);
            }
        }

        void FireOnFailRelease(ExclusionError error)
        {
            ListenerList list;
            lock (TheListenerLock)
            {
                list = new ListenerList(ListenerList);
            }
            foreach (var l in list)
            {
                l.OnFailReleasing(error);
            }
        }

        void FireOnFailOpen(ExclusionError error)
        {
            ListenerList list;
            lock (TheListenerLock)
            {
                list = new ListenerList(ListenerList);
            }
            foreach (var l in list)
            {
                l.OnFailOpening(error);
            }
        }
    }
}
