using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public enum CommandDataType : byte
	{
		R = (byte)0x52, // R:word
		D = (byte)0x44, // D:word
		T = (byte)0x54, // T:word
		C = (byte)0x43, // C:byte
		L = (byte)0x4c, // L:long
		F = (byte)0x46  // F:single
	}

	public static class CommandDataTypeExt{
		
		public static float DataSize(this CommandDataType dataType){

			switch (dataType) {
			case CommandDataType.R:
			case CommandDataType.D:
			case CommandDataType.T:
			case CommandDataType.C:
				return 2;
			case CommandDataType.F:
			case CommandDataType.L:
				return 4;
			//case CommandDataType.C:
			//	return 1;
			default:
				throw new ArgumentOutOfRangeException("CommandDataTypeExt.DataSize - ケース設定に漏れがあります。");
			}
		}

	}

}

