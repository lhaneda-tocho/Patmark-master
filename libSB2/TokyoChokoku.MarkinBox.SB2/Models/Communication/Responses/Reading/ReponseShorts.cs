using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseShorts : Response
    {
        public ResponseShorts (IRawResponse raw) : base(raw)
        {
        }

        public List<short> Value{
            get{
                return BigEndianBitConverter.ToShorts (Raw.Data);
            }
        }
    }
}

