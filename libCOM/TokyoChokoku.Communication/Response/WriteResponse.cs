using System;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class WriteResponse
	{
        public bool            IsOk     { get; }
        public List<byte>      Data     { get; }

        public WriteResponse (bool isOk, List<byte> data)
		{
            IsOk     = isOk;
            Data     = data;
		}
	}
}

