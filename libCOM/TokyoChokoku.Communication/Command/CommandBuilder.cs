using System;

using System.Collections.Generic;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Communication
{
	public abstract class CommandBuilder
	{
        byte FrameNo { get; set; } = 0x00;

        protected CommandBuilder()
		{
			FrameNo = FrameNumberManager.Instance.GetNumber ();
		}

		public Byte[] ToBytes(){
			List<Byte> result = new List<Byte>();
			// Header
			result.AddRange (GetHeader ());
			// Body
			var body = GetBody ();
			result.AddRange (BigEndian.GetBytes((short)body.Count));
			result.AddRange (body);
			// Footer
			result.Add (CommandCode.Etx);
			result.Add (GetCheckSum(body));
			return result.ToArray();
		}

		private List<Byte> GetHeader(){
			List<Byte> result = new List<Byte>();
			result.Add( CommandCode.Syn );
			result.Add( CommandCode.Syn );
			result.Add( CommandCode.Syn );
			result.Add( CommandCode.Stx );
			result.Add( CommandCode.Adr );
			result.Add( FrameNo );
			return result;
		}

		protected virtual List<Byte> GetBody (){
			return new List<Byte> ();
		}

        private byte GetCheckSum(List<Byte> body)
        {
            List<Byte> items = new List<Byte>();

            items.Add(CommandCode.Stx);
            items.Add(CommandCode.Adr);
            items.Add(FrameNo);
            items.AddRange(BigEndian.GetBytes((short)body.Count));
            items.AddRange(body);
            items.Add(CommandCode.Etx);

            byte result = 0x00;
            foreach (byte item in items ){
				result += item;
			}
			return result;
        }
	}
}

