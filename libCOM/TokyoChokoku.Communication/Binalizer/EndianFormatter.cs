using System;
using System.Linq;
using BitConverter;
namespace TokyoChokoku.Communication
{
    /// <summary>
    /// 打刻機側のデータをプログラマで扱える形式へ変換する
    /// </summary>
    public abstract class EndianFormatter
	{
        #region To Command Content
        Byte[] CommandFormatC(Byte[] programmerBytes)
        {
            return programmerBytes.Select(
                (v)=>PackC(v)
            ).Flatten().ToArray();
        }

        Byte[] CommandFormatD(Byte[] programmerBytes)
        {
            return programmerBytes.DiPack().Select(
                (v)=>FlipDRT(v)
            ).Flatten().ToArray();
		}

		Byte[] CommandFormatR(Byte[] programmerBytes)
		{
			return programmerBytes.DiPack().Select(
				(v) => FlipDRT(v)
			).Flatten().ToArray();
		}

		Byte[] CommandFormatT(Byte[] programmerBytes)
		{
			return programmerBytes.DiPack().Select(
				(v) => FlipDRT(v)
			).Flatten().ToArray();
		}

		Byte[] CommandFormatF(Byte[] programmerBytes)
		{
			return programmerBytes.TetraPack().Select(
				(v) => FlipFL(v)
			).Flatten().ToArray();
		}

		Byte[] CommandFormatL(Byte[] programmerBytes)
		{
			return programmerBytes.TetraPack().Select(
				(v) => FlipFL(v)
			).Flatten().ToArray();
		}

        public Byte[] CommandFormat(Byte[] programmerBytes, CommandDataType type)
        {
            switch(type)
			{
				case CommandDataType.C: return CommandFormatC(programmerBytes);
                    
				case CommandDataType.D: return CommandFormatD(programmerBytes);
				case CommandDataType.R: return CommandFormatR(programmerBytes);
				case CommandDataType.T: return CommandFormatT(programmerBytes);
                    
				case CommandDataType.F: return CommandFormatF(programmerBytes);
				case CommandDataType.L: return CommandFormatL(programmerBytes);

                default: throw new ArgumentOutOfRangeException();
            }
        }
		#endregion

		#region To Programmer Content
		Byte[] ProgrammerFormatC(Byte[] commandData)
        {
            return commandData.DiPack().Select(
                (v) => UnpackC(v)
            ).ToArray();
		}

		Byte[] ProgrammerFormatD(Byte[] commandData)
		{
			return commandData.DiPack().Select(
				(v) => FlipDRT(v)
			).Flatten().ToArray();
		}

        Byte[] ProgrammerFormatR(Byte[] commandData)
        {
            return commandData.DiPack().Select(
                (v) => FlipDRT(v)
            ).Flatten().ToArray();
        }

		Byte[] ProgrammerFormatT(Byte[] commandData)
		{
			return commandData.DiPack().Select(
				(v) => FlipDRT(v)
			).Flatten().ToArray();
		}

		Byte[] ProgrammerFormatF(Byte[] commandData)
		{
			return commandData.TetraPack().Select(
				(v) => FlipFL(v)
			).Flatten().ToArray();
		}

        Byte[] ProgrammerFormatL(Byte[] commandData)
        {
            return commandData.TetraPack().Select(
                (v) => FlipFL(v)
            ).Flatten().ToArray();
        }

		public Byte[] ProgrammerFormat(Byte[] commandData, CommandDataType type)
		{
			switch (type)
			{
				case CommandDataType.C: return ProgrammerFormatC(commandData);

				case CommandDataType.D: return ProgrammerFormatD(commandData);
				case CommandDataType.R: return ProgrammerFormatR(commandData);
				case CommandDataType.T: return ProgrammerFormatT(commandData);

				case CommandDataType.F: return ProgrammerFormatF(commandData);
				case CommandDataType.L: return ProgrammerFormatL(commandData);

				default: throw new ArgumentOutOfRangeException();
			}
		}

        #endregion

        /// <summary>
        /// プログラマから見たデータのエンディアンを返します
        /// </summary>
        public abstract Endian ProgrammerSideEndian { get; }
        /// <summary>
        /// C 領域からリードしたデータからバイトデータを取り出す．
        /// </summary>
        /// <returns>The c.</returns>
        /// <param name="di">Di.</param>
        public abstract Byte UnpackC(DiByte di);
        /// <summary>
        /// C領域に送るデータを送信可能な形式に変換する
        /// </summary>
        /// <returns>The c.</returns>
        /// <param name="mono">Mono.</param>
        public abstract DiByte PackC(Byte mono);
		/// <summary>
		/// D・R・T 領域のデータのフリップを打ち消す．
		/// 実装によっては打ち消されない場合もある．
		/// </summary>
		public abstract DiByte FlipDRT(DiByte di);
		/// <summary>
		/// F・L 領域のデータのフリップを打ち消す．
		/// 実装によっては打ち消されない場合もある．
		/// </summary>
        public abstract TetraByte FlipFL(TetraByte tetra);

		/// <summary>
		/// ProgrammerSideEndianに対応したBitConverterを返します．
		/// </summary>
		/// <returns>The bit converter.</returns>
		public EndianBitConverter BitConverter {
            get {
                return ProgrammerSideEndian.GetConverter();
            }
        }
    }

    /// <summary>
    /// パットマークのバイト配列のフリップを打ち消す
    /// </summary>
    public class PatmarkEndianFormatter : EndianFormatter
    {
        public override Endian ProgrammerSideEndian => Endian.Little;

        public override DiByte FlipDRT(DiByte di)
        {
            return di.Flip();
        }

        public override TetraByte FlipFL(TetraByte tetra)
		{
            return tetra.Flip();
        }

        public override DiByte PackC(byte mono)
        {
            return DiByte.Init(0, mono);
        }

        public override byte UnpackC(DiByte di)
        {
            return di.B;
        }
    }

    /// <summary>
    /// Markin box 用．
    /// </summary>
    public class MarkinBoxEndianFormatter: EndianFormatter
    {
        public override Endian ProgrammerSideEndian => Endian.Big;

        public override DiByte FlipDRT(DiByte di)
        {
            return di;
        }

        public override TetraByte FlipFL(TetraByte tetra)
        {
            return tetra;
        }

        public override DiByte PackC(byte mono)
        {
            return DiByte.Init(0, mono);
        }

        public override byte UnpackC(DiByte di)
        {
            return di.B;
        }
    }
}
