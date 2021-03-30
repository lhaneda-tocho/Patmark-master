using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.Text;


namespace TokyoChokoku.Communication
{
    public class ResponseRemoteFileNames : AbstractResponse
    {
        public static string Decode(byte[] binary)
        {
            var fileName = TextEncodings.Byte1.GetStringFromNoStride(binary);
            var indexNum = fileName.IndexOf('\0');
            if (indexNum != -1)
            {
                fileName = fileName.Substring(0, indexNum);
            }
            return fileName;
        }

        // ==== ==== ==== ==== ==== ====

        public ResponseRemoteFileNames (ReadResponse raw) : base(raw)
        {
        }

        public List<string> Value{
            get
            {
                var names = new List<string>();
                for (var i = 0; i < Sizes.NumOfRemoteFile; i++)
				{
					var count    = Sizes.BytesOfRemoteFileName;
                    var index    = i * count;
                    var element  = Data.GetBytes(index, count, IndexType.Byte);
                    var fileName = Decode(element.ToArray());

                    names.Add (fileName);
                }
                return names;
            }
        }


    }
}

