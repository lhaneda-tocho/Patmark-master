using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public class ResponseShorts : AbstractResponse
    {
        public ResponseShorts (ReadResponse raw) : base(raw)
        {
        }

        public List<short> Value{
            get{
                return Data.GetAllWords().Select(
                    (v) => v.SInt
                ).ToList();
            }
        }
    }
}

