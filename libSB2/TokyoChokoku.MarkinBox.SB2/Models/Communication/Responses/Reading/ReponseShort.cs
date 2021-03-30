using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseShort : Response
    {
        public ResponseShort (IRawResponse raw) : base(raw)
        {
        }

        public short Value{
            get{
                return BigEndianBitConverter.ToShort (new BigEndianBytes (Raw.Data));
            }
        }
    }
}

