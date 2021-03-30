using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
	public class WritingResponseExtracter
	{
        public static bool IsOk(byte[] data){
            // 2 bytes : Ack
            // 3 bytes : Nack
            return (data != null && data.Length == 2);
        }

        public static List<byte> Extract(byte[] data){
            return new List<byte> (data);
		}
	}
}

