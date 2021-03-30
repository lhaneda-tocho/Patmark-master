using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox;
using BN = TokyoChokoku.Structure.Binary;

namespace TokyoChokoku.Communication
{
    public class ResponseMBSerialCounterData : AbstractResponse
    {
        public ResponseMBSerialCounterData(ReadResponse raw) : base(raw)
        {
        }

        public List<MBSerialCounterData> Value
        {
            get
            {
                var binarizer = new BN.MBSerialCounterDataBinarizer(Data);
                return binarizer.ConstructObject();
            }
        }
    }
}
