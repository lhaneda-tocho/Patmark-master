using System;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class ReadResponse
	{
		public bool       IsOk { get; }
		public Programmer Data { get; }

        public MemoryAddress   Address  { get; }
        public CommandDataType DataType { get; }


        public ReadResponse (bool isOk, List<byte> data, MemoryAddress addreess, EndianFormatter formatter)
		{
            IsOk     = isOk;
            Address  = addreess;
			DataType = Address.DataType;
            Data = Programmer.ReadCommandData(formatter, data.ToArray(), Address.DataType);
		}
	}
}

