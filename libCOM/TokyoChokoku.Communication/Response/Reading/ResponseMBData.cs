using System;
using TokyoChokoku.MarkinBox.Sketchbook;
using BN = TokyoChokoku.Structure.Binary;

namespace TokyoChokoku.Communication
{
    public class ResponseMBData : AbstractResponse
    {
        public ResponseMBData (ReadResponse raw) : base(raw)
        {
        }

        public MBData Value
        {
			get
			{
				var binarizer = new BN.MBDataBinarizer (Data);
				//var binarizer = new MBDataBinarizer (Contents.ToArray (), Coder);
				return binarizer.ToMBData ();
            }
        }
    }
}

