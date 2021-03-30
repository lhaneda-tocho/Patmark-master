using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.Text;

namespace TokyoChokoku.Communication
{
    public class ResponseAscii : AbstractResponse
    {
        public ResponseAscii (ReadResponse raw) : base(raw)
        {
        }

        public string Value{
            get{
                var contents = Data.GetAllBytes();
                return TextEncodings.Byte1.GetString(contents.ToArray());
            }
        }
    }
}


