using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.Communication.CommandGenerateKit
{
    /// <summary>
    /// Command element.
    /// Jsonと変換可能．
    /// </summary>
	public class CommandElement
    {
        public string   Title               { get; set; }
        public string[] Arguments           { get; set; }
        public string   Address             { get; set; }
        public string   Data                { get; set; } // Data, DataLineとの選択
        public string[] DataLines           { get; set; } // Data, DataLineとの選択
        public string   ReturnValueClass    { get; set; } = "Response";
        public bool     NeedsResponse       { get; set; }
        public int      Timeout             { get; set; }
        public int      NumOfRetry          { get; set; }
        public int      Delay               { get; set; }
        public bool     WaitToFinishWriting { get; set; }
        public string[] Comment             { get; set; } = new string[]{""};

        public CommandElement(
            string title,
            string[] arguments,
            string address,
            string data,
            string returnValueClass = "Response",
            bool needsResponse = true,
            int timeout = 500,
            int numOfRetry = 3,
            int delay = 10,
            bool waitToFinishWriting = false
        )
        {
            Title               = title;
            Arguments           = arguments;
            Address             = address;
            Data                = data;
            ReturnValueClass    = returnValueClass;
            NeedsResponse       = needsResponse;
            Timeout             = timeout;
            NumOfRetry          = numOfRetry;
            Delay               = delay;
            WaitToFinishWriting = waitToFinishWriting;
        }

		public CommandElement()
		{
            NeedsResponse = true;
            Timeout = 500;
            NumOfRetry = 3;
            Delay = 10;
            WaitToFinishWriting = false;
        }

        public void SetDefaultNeeded()
        {
            if (Arguments == null)
                Arguments = new string[0];
            if (ReturnValueClass == null)
                ReturnValueClass = "Response";
            if (Comment == null)
                Comment = new string[]{""};
        }

        public void FixAndValidate()
		{
			Require("not found Title"    , Title != null);
			Require("not found Arguments", Arguments != null);
			Require("not found Address", Address != null);
            ValidateData();
			Require("not found Return Value Class", ReturnValueClass != null);
            foreach(var i in Arguments) {
                Require("not allowed null in argument array.", i != null);
            }
        }

        void ValidateData()
        {
            var single = Data != null;
			var multi  = DataLines != null;
			Console.WriteLine(single);
            Console.WriteLine(multi);


			var found = (single || multi);
            var nonconflict = !((Data != null) && (DataLines != null));

            Require("not found Data or DataLine", found);
            Require("conflict properties of Data and DataLine", nonconflict);
        }

        public string GenData(int space) {
            //Console.WriteLine("ねこ");
            ValidateData();
            string indent;
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < space; ++i)
                    sb.Append(' ');
                indent = sb.ToString();
            }
            if (Data != null)
			{
                return Data;
            }
            else
            if (DataLines != null)
			{
                bool first = true;
				var sb = new System.Text.StringBuilder();
                foreach (var line in DataLines)
                {
                    if (first)
                        sb.Append(line);
                    else
                        sb.AppendLine().Append(indent).Append(line);
                    first = false;
                }
                return sb.ToString();
            }

            throw new InvalidOperationException("detect concurrent modification.");
        }

        void Require(string message, bool condition) {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        public override string ToString()
        {
            return string.Format("[CommandElement: Title={0}, Arguments={1}, Address={2}, Data={3}, ReturnValueClass={4}, NeedsResponse={5}, Timeout={6}, NumOfRetry={7}, Delay={8}, WaitToFinishWriting={9}]", Title, Arguments, Address, Data, ReturnValueClass, NeedsResponse, Timeout, NumOfRetry, Delay, WaitToFinishWriting);
        }

        public bool ContentEquals(CommandElement r)
		{
            if (Title != r.Title)
                return false;
            if (Equals(Arguments, r.Arguments))
                return false;
            if (Address != r.Address)
                return false;
            if (Data != r.Data)
                return false;
            if (ReturnValueClass != r.ReturnValueClass)
                return false;
            if (NeedsResponse != r.NeedsResponse)
                return false;
            if (Timeout != r.Timeout)
                return false;
            if (NumOfRetry != r.NumOfRetry)
                return false;
            if (Delay != r.Delay)
                return false;
            if (WaitToFinishWriting != r.WaitToFinishWriting)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
                return false;
            var e = obj as CommandElement;
            return ContentEquals(e);
        }

        public override int GetHashCode()
        {
            int hash = 7;
            NextHash(ref hash, Title);
            foreach (var i in Arguments)
                NextHash(ref hash, i);
            NextHash(ref hash, Address);
            NextHash(ref hash, Data);
            NextHash(ref hash, ReturnValueClass);
            NextHash(ref hash, NeedsResponse);
            NextHash(ref hash, Timeout);
            NextHash(ref hash, NumOfRetry);
            NextHash(ref hash, Delay);
            NextHash(ref hash, WaitToFinishWriting);
            return hash;
        }

        void NextHash(ref int value, object any) {
            value += 13 + value * any.GetHashCode();
        }
    }
}

