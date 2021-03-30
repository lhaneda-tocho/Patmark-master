using System;
using System.Linq;
using System.Collections.Generic;


namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ResponseRemoteFileNames : Response
    {
        public ResponseRemoteFileNames (IRawResponse raw) : base(raw)
        {
        }

        public List<string> Value{
            get
            {
                var names = new List<string>();
                for (var i = 0; i < Sizes.NumOfRemoteFile; i++)
                {
                    var responseFileName = Raw.Data.Skip (i * Sizes.BytesOfRemoteFileName * 2).Take (Sizes.BytesOfRemoteFileName * 2).ToArray ();
                    var rawFileName = BigEndianBitConverter.ResponseCharArrayToByteArray (responseFileName);
                    var fileName = BigEndianBitConverter.DecodeText (rawFileName, TextEncode.Byte2);
                    names.Add (fileName);
                }
                return names;
            }
        }
    }
}

