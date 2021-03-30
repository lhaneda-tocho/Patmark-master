using System;

namespace TokyoChokoku.Communication
{
	public static class CommandCode
	{
		public const byte Syn = (byte)0x16;
		public const byte Stx = (byte)0x02;
		public const byte Adr = (byte)0xFF;
		public const byte Etx = (byte)0x03;
	}
}

