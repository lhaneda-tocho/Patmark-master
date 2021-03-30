using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseChars : Response
    {
        public ResponseChars (IRawResponse raw) : base(raw)
        {
        }

        public List<char> Value{
            get{
                return BigEndianBitConverter.ToChars (Raw.Data);
            }
        }
    }
}

