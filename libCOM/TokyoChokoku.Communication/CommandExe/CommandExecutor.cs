using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.Text;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.Communication
{


    public partial class CommandExecutor
    {
        public CommunicationClient Client { get; }
        public CommandTaskManager Manager { get; }

		//public RemoteFileManager     RemoteFileManager     { get { return Client.RemoteFileManager    ; } }
        //public SerialSettingsManager SerialSettingsManager { get { return Client.SerialSettingsManager; } }

		#region assets
		internal static readonly ILog Log = CommunicationLogger.Supply();
        public EndianFormatter Formatter { get; }


        // 組み込み関数
        Structure.Binary.MBDataBinarizer CreateMBDataBinarizer(MarkinBox.Sketchbook.MBData field)
        {
            return new Structure.Binary.MBDataBinarizer(Formatter, field);
        }

        Structure.Binary.MBCalendarShiftDataBinarizer CreateMBCalendarShiftDataBinarizer(MarkinBox.MBCalendarData cdata)
        {
            return new Structure.Binary.MBCalendarShiftDataBinarizer(Formatter, cdata);
        }

        Structure.Binary.MBCalendarYmdDataBinarizer CreateMBCalendarYmdDataBinarizer(MarkinBox.MBCalendarData cdata)
        {
            return new Structure.Binary.MBCalendarYmdDataBinarizer(Formatter, cdata);
        }

        Structure.Binary.MBSerialSettingDataBinarizer CreateMBSerialSettingDataBinarizer(IEnumerable<MarkinBox.MBSerialData> sdata)
        {
            return new Structure.Binary.MBSerialSettingDataBinarizer(Formatter, sdata);
        }

        Structure.Binary.MBSerialCounterDataBinarizer CreateMBSerialCounterDataBinarizer(SCCountStateList sdata) 
        {
            return new Structure.Binary.MBSerialCounterDataBinarizer(Formatter, sdata);
        }

        Programmer Alloc(int size, IndexType type)
        {
            var bsize = type.ToByteCount(size);
            return new Programmer(Formatter, bsize);
        }

		public Programmer AllocByte(int size)
		{
            return Alloc(size, IndexType.Byte);
		}

		public Programmer AllocWord(int size)
		{
            return Alloc(size, IndexType.Word);
		}

        public Programmer AllocDWord(int size)
        {
            return Alloc(size, IndexType.DWord);
		}

		public Programmer ByteOf(byte value)
		{
			return AllocByte(1).PutByte(value);
		}

        public Programmer BytesOf(byte[] value)
        {
            var len  = value.Length;
            var type = IndexType.Byte;
            return AllocByte(len).PutBytes(value, 0, len, type);
        }

        public Programmer WordOf(UInt16 value)
        {
            return AllocWord(1).PutWord(Word.Init(value));
		}

        public Programmer WordOf(Int16 value)
        {
            return AllocWord(1).PutWord(Word.Init(value));
        }

        public Programmer RevWordOf(Int16 value)
        {
            return AllocWord(1).PutRevWord(Word.Init(value));
        }

		public Programmer WordsOf(params UInt16[] value)
		{
			var len = value.Length;
			var type = IndexType.Word;
			return AllocWord(len).PutWords(value.Select((v) => Word.Init(v)), 0, len, type);
		}

		public Programmer WordsOf(params Int16[] value)
		{
			var len = value.Length;
			var type = IndexType.Word;
			return AllocWord(len).PutWords(value.Select((v) => Word.Init(v)), 0, len, type);
		}

		public Programmer DWordOf(UInt32 value)
		{
			return AllocDWord(1).PutDWord(DWord.Init(value));
		}

		public Programmer DWordOf(Int32 value)
		{
			return AllocDWord(1).PutDWord(DWord.Init(value));
		}

		public Programmer DWordOf(Single value)
		{
			return AllocDWord(1).PutDWord(DWord.Init(value));
		}

		public Programmer DWordsOf(params UInt32[] value)
		{
			var len = value.Length;
			var type = IndexType.DWord;
			return AllocDWord(len).PutDWords(value.Select((v) => DWord.Init(v)), 0, len, type);
		}

		public Programmer DWordsOf(params Int32[] value)
		{
			var len = value.Length;
			var type = IndexType.DWord;
			return AllocDWord(len).PutDWords(value.Select((v) => DWord.Init(v)), 0, len, type);
		}

		public Programmer DWordsOf(params Single[] value)
		{
			var len = value.Length;
			var type = IndexType.DWord;
			return AllocDWord(len).PutDWords(value.Select((v) => DWord.Init(v)), 0, len, type);
		}

        /// <summary>
        /// TextEncode.Byte1 から プログラマを作成します
        /// </summary>
        /// <returns>The text of.</returns>
        /// <param name="text">Text.</param>
        public Programmer MonoTextOf(string text, int byteSize)
        {
            return AllocByte(byteSize).PutBytes(
                TextEncodings.Byte1.GetBytes(text), 0, byteSize, IndexType.Byte
            );
        }

		/// <summary>
		/// TextEncode.Byte2 から プログラマを作成します
		/// </summary>
		/// <returns>The text of.</returns>
		/// <param name="text">Text.</param>
		public Programmer WideTextOf(string text, int wordSize)
        {
            var words = TextEncodings.Byte2.GetBytes(text).DiPack().Select(
                // システムのエンディアンに合わせる
                (v) => (System.BitConverter.IsLittleEndian) ? v.Flip() : v
            ).Select(
                // Word に変換
                (v) => Word.Init(System.BitConverter.ToUInt16(v.ToBytes(), 0))
            );
            return AllocWord(wordSize).PutWords(
                words, 0, wordSize, IndexType.Word
            );
        }

        public Programmer WideTextNoStrideOf(string text, int wordSize)
        {
            var array = TextEncodings.Byte2.GetBytesNoStride(text, wordSize*2);
            return new Programmer(Formatter, array);
        }

		#endregion

		#region write ready

        public WritingCommandBuilder readyWrite(MemoryAddress address, Programmer b)
		{
			return new WritingCommandBuilder(
				address, b
			);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TokyoChokoku.Communication.CoreCommands"/> class.
		/// </summary>
		/// <param name="coder">Coder.</param>
        public CommandExecutor(CommunicationClient client, EndianFormatter formatter, CommandTaskManager manager)
        {
            Client = client;
            Formatter = formatter;
            Manager = manager;
        }

        async Task<WriteResponse> Write(List<ICommand> commands)
        {
            var res = new byte[] { };
            foreach (var command in commands)
            {
                res = await Manager.StartCommand(command, (data) =>
                {
                    return data;
                });
                if (!WritingResponseExtracter.IsOk(res))
                {
                    break;
                }
                await Task.Delay(10);
            }
            return new WriteResponse(
                    WritingResponseExtracter.IsOk(res),
                    WritingResponseExtracter.Extract(res)
            );
        }

        async Task<WriteResponse> Write(ICommand command)
		{
            var res = await Manager.StartCommand(command, (data) => {
                return data;
            });
			return new WriteResponse(
                    WritingResponseExtracter.IsOk(res),
                    WritingResponseExtracter.Extract(res)
			);
		}

		async Task<ReadResponse> Read(List<ICommand> commands, MemoryAddress address)
		{
			var res = new List<byte>();
			foreach (var command in commands)
			{
                var tmpRes = await Manager.StartCommand(command, (data) =>
                {
                    return ReadingResponseExtracter.Extract(data).ToArray();
                });
                res.AddRange(tmpRes);
			}
            return new ReadResponse(res.Count > 0, res, address, Formatter);
		}
    }
}
