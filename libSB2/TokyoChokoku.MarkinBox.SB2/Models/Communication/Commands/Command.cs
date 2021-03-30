using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class Command : ICommand
    {
        private Byte[] RawCommand;
        public bool NeedsResponse { get; private set; } = true;
        public int Timeout { get; private set; }
        public int NumOfRetry { get; private set; }

        public Command(Byte[] rawCommand, bool needsResponse, int timeout, int numOfRetry)
        {
            this.RawCommand = rawCommand;
			this.NeedsResponse = needsResponse;
			this.Timeout = timeout;
            this.NumOfRetry = numOfRetry;
        }

        public Byte[] ToBytes()
        {
            return this.RawCommand;
        }
    }
}

