using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.MetaCommunication
{
	public class CommandElement
    {
        public string Title { get; protected set; }
        public string[] Arguments { get; protected set; }
        public string Address { get; protected set; }
        public string Data { get; protected set; }
        public string ReturnValueClass { get; protected set; }
        public bool NeedsResponse { get; protected set; }
        public int Timeout { get; protected set; }
        public int NumOfRetry { get; protected set; }
        public int Delay { get; protected set; }
        public bool WaitToFinishWriting { get; private set; }

        public CommandElement(
            string title,
            string[] arguments,
            string address,
            string data,
            string returnValueClass = "ResponseRaw",
            bool needsResponse = true,
            int timeout = 5000,
            int numOfRetry = 3,
            int delay = 0,
            bool waitToFinishWriting = false
        )
        {
            this.Title = title;
            this.Arguments = arguments;
            this.Address = address;
            this.Data = data;
            this.ReturnValueClass = returnValueClass;
            this.NeedsResponse = needsResponse;
            this.Timeout = timeout;
            this.NumOfRetry = numOfRetry;
            this.Delay = delay;
            this.WaitToFinishWriting = waitToFinishWriting;
        }
    }
}

