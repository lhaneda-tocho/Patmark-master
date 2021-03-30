using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class RawResponseContainer : IRawResponse
	{
        public bool IsOk { get; private set; }
        public List<byte> Data { get; private set; }

        public RawResponseContainer (bool isOk, List<byte> data)
		{
            this.IsOk = isOk;
            this.Data = data;
		}
	}
}

