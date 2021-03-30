using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class AbstractResponse
	{
        
        public readonly MemoryAddress   Address;
		public readonly CommandDataType DataType;

        public bool       IsOk { get; }
        public Programmer Data { get; }

        protected AbstractResponse (ReadResponse raw)
        {
            
            Address  = raw.Address;
			DataType = raw.DataType;
            IsOk     = raw.IsOk;
            Data     = raw.Data;
		}
    }
}

