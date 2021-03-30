using System;

namespace TokyoChokoku.Communication
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
		
        public static /* not zero */ uint DataSize(this CommandDataType dataType) 
        {

            switch (dataType)
            {
                case CommandDataType.C:
                    return 1;
                case CommandDataType.R:
                case CommandDataType.D:
                case CommandDataType.T:
                    return 2;
                case CommandDataType.F:
                case CommandDataType.L:
                    return 4;
                //case CommandDataType.C:
                //  return 1;
                default:
                    throw new ArgumentOutOfRangeException("CommandDataTypeExt.DataSize - ケース設定に漏れがあります。");
            }
        }

        public static float ReceivedDataByteSize(this CommandDataType dataType)
        {

            switch (dataType)
            {
                case CommandDataType.C:
                case CommandDataType.R:
                case CommandDataType.D:
                case CommandDataType.T:
                    return 2;
                case CommandDataType.F:
                case CommandDataType.L:
                    return 4;
                //case CommandDataType.C:
                //  return 1;
                default:
                    throw new ArgumentOutOfRangeException("CommandDataTypeExt.DataSize - ケース設定に漏れがあります。");
            }
        }

	}

}

