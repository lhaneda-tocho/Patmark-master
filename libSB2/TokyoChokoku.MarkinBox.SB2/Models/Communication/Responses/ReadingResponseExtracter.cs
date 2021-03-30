using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public static class ReadingResponseExtracter
	{
        public static bool IsOk(byte[] data){
            return (data != null && data.Length >= 10);
        }

		public static List<byte> Extract(byte[] data){
            if (IsOk(data)) {
				// Syn,Syn,Syn,Stx,Adr,Frm, データ部バイト数(2), データ部(n), Etx, Lrc 
                var numofValueBytes = BigEndianBitConverter.ToShort(new BigEndianBytes(data.Skip(6).Take(2)));
				if (numofValueBytes > 0) {
                    return data.Skip (8).Take (numofValueBytes).ToList();
				}
			}
            return new List<byte>();
		}
	}
}

