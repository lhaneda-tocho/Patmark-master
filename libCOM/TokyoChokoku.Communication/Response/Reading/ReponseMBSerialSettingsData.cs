using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox;
using BN = TokyoChokoku.Structure.Binary;

namespace TokyoChokoku.Communication
{
    public class ResponseMBSerialSettingData : AbstractResponse
    {
        public ResponseMBSerialSettingData(ReadResponse raw) : base(raw)
        {
        }

        public List<MBSerialData> Value
        {
            get
            {
                var binarizer = new BN.MBSerialSettingDataBinarizer(Data);
                return binarizer.ConstructObject();
            }
        }
    }
}
