using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseRaw : Response
    {
        public ResponseRaw (IRawResponse raw) : base(raw)
        {
        }

        public List<byte> Value{
            get{
                return Raw.Data;
            }
        }
    }
}

