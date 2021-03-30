using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class Response
    {
        protected IRawResponse Raw;

        public Response (IRawResponse raw)
        {
            this.Raw = raw;
        }

        public bool IsOk
        {
            get{
                return Raw.IsOk;
            }
        }
    }
}

