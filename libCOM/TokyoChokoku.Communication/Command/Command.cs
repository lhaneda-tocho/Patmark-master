using System;
namespace TokyoChokoku.Communication
{
    public class Command : ICommand
    {
        private Byte[] RawCommand;
        public bool NeedsResponse { get; private set; } = true;
        public int Timeout { get; private set; }
        public int NumOfRetry { get; private set; }
        public int DelayAfter { get; private set; }
        public int ExpectedDataSize { get; private set; }

        public bool EnableBeforeExcluding { get; }

        public Command(Byte[] rawCommand, bool needsResponse, int timeout, int numOfRetry, int delayAfter = 0, int expectedDataSize = 0, bool enableBeforeExcluding = false)
        {
            this.RawCommand = rawCommand;
            this.Timeout = timeout;
            this.NumOfRetry = numOfRetry;
            this.DelayAfter = delayAfter;
            this.ExpectedDataSize = expectedDataSize;
            this.EnableBeforeExcluding = enableBeforeExcluding;
        }

        public Byte[] ToBytes()
        {
            return this.RawCommand;
        }


    }
}

