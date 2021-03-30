using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class ResponseShort : AbstractResponse
    {
        public ResponseShort (ReadResponse raw) : base(raw)
        {
        }

        public short Value
        {
            get
            {
                return Data.GetWord().SInt;
            }
        }
    }
}

