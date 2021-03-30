using System;
using TokyoChokoku.Communication;
namespace nunit.TokyoChokoku.Communication
{
    public class MockedLineObserver: LineObserver
    {
        public override void Start()
        {
            // ignore
        }

        public override void Stop()
        {
            // ignore
        }

        public void NotifyOnline()
        {
            OnFoundConnection();
        }

        public void NotifyOffline()
        {
            OnLostConnection();
        }

        public void WaitFree()
        {
            while (IsBusy)
                System.Threading.Thread.Sleep(1);
        }
    }
}
