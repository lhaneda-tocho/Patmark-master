using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    /// <summary>
    /// 監視用のタイマー
    /// </summary>
    public class EventDispatchTimer
    {
        // lock
        object Thelock = new object();

        // onTickコールバック
        public event Action OnTick = null;

        // Timer
        readonly Timer Timer;

        // Call Interval
        readonly int Interval;

        // is loop enable
        volatile bool isEnable = false;
        // is loop stopped.
        volatile bool stop = true;

        public EventDispatchTimer(int interval)
        {
            Interval = interval;
            Timer = new Timer((_) => Update(), null, Timeout.Infinite, Timeout.Infinite);
        }

        void Update()
        {
            // スタートとストップの処理
            lock (Thelock)
            {
                if (!isEnable)
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    stop = true;
                    return;
                }
                else
                {
                    stop = false;
                }
            }
            // onTickコールバック実行
            try
            {
                OnTick?.Invoke();
            }
            catch (Exception ex)
            {
                var e = Console.Error;
                e.WriteLine("Error from onTick in " + this);
                e.WriteLine(ex);
            }
        }

        public void Start()
        {
            lock (Thelock)
            {
                isEnable = true;
                Timer.Change(0, Interval);
            }
        }

        public void Stop()
        {
            lock (Thelock)
            {
                isEnable = false;
            }
        }
    }
}