using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class ResponseByte : AbstractResponse
    {
        public ResponseByte (ReadResponse raw) : base(raw)
        {
        }

        public byte Value
        {
            get
            {
                return Data.GetByte();
            }
        }
    }
}

