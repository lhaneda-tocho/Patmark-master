using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseMBData : Response
    {
        public ResponseMBData (IRawResponse raw) : base(raw)
        {
        }

        public MBData Value
        {
            get{
                var binarizer = new MBDataBinarizer (Raw.Data.ToArray ());
                return binarizer.ToMBData ();
            }
        }
    }
}

