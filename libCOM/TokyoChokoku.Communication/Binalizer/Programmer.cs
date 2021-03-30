using System;
using System.Linq;
using System.Collections.Generic;
using BitConverter;

namespace TokyoChokoku.Communication
{
    /// <summary>
    /// 受送信するデータの書き込みと読み込みを担当するクラスです
    /// </summary>
    public class Programmer
    {
        readonly Byte[] Buffer;

        public readonly EndianFormatter    TheFormatter;
        public readonly EndianBitConverter TheConverter;
        public readonly Endian             TheEndian;


        public int ByteCount {
            get {
                return Buffer.Length;
            }
        }

        /// <summary>
        /// コマンドで送信可能な形式に変換します
        /// </summary>
        /// <value>The C memory data</value>
        public Byte[] ToCommandFormat(CommandDataType dataType)
		{
            return TheFormatter.CommandFormat(Buffer, dataType);
		}

		/// <summary>
		/// コマンドデータを読み込み，Programmerとして返します．
		/// </summary>
		/// <returns>The command data.</returns>
		/// <param name="formatter">Formatter.</param>
		/// <param name="commandData">Command data.</param>
		/// <param name="dataType">Data type.</param>
		public static Programmer ReadCommandData(EndianFormatter formatter, Byte[] commandData, CommandDataType dataType) 
        {
            var buffer = formatter.ProgrammerFormat(commandData, dataType);
            return new Programmer(formatter, buffer);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TokyoChokoku.Communication.Programmer"/> class.
		/// </summary>
		/// <param name="formatter">Formatter.</param>
		/// <param name="dataStore">Write/Read target buffer.</param>
		public Programmer(EndianFormatter formatter, Byte[] dataStore)
		{
			TheFormatter = formatter;
			TheConverter = formatter.BitConverter;
			TheEndian = formatter.ProgrammerSideEndian;
            Buffer = dataStore;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TokyoChokoku.Communication.Programmer"/> class.
		/// </summary>
		/// <param name="formatter">Formatter.</param>
		/// <param name="size">Size of the data.</param>
        public Programmer(EndianFormatter formatter, int size): this(formatter, new byte[size])
        {}

        /// <summary>
        /// Puts the byte.
        /// </summary>
        /// <param name="value">Value.</param>
        public Programmer PutByte(Byte value)
        {
            // 書き込み
            Buffer[0] = value;
            return this;
        }

        /// <summary>
        /// Puts the word.
        /// </summary>
        /// <param name="value">Value.</param>
        public Programmer PutWord(Word value)
        {
            return PutWord(value, 0, IndexType.Byte);
        }

        /// <summary>
        /// Puts the word.
        /// </summary>
        /// <param name="value">Value.</param>
        public Programmer PutRevWord(Word value)
        {
            return PutRevWord(value, 0, IndexType.Byte);
        }

        /// <summary>
        /// Puts the DWord.
        /// </summary>
        /// <param name="value">Value.</param>
        public Programmer PutDWord(DWord value)
		{
			return PutDWord(value, 0, IndexType.Byte);
        }

        /// <summary>
        /// Puts the byte.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="destinationIndex">Destination index.</param>
        /// <param name="type">Type.</param>
        public Programmer PutByte(Byte value, int destinationIndex, IndexType type)
		{
			// 書き込み場所の決定
			var bindex = type.ToByteIndex(destinationIndex);
            // 書き込み
            Buffer[bindex] = value;
            return this;
		}

        /// <summary>
        /// Puts the word.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="destinationIndex">Destination index.</param>
        /// <param name="type">Type.</param>
        public Programmer PutWord(Word value, int destinationIndex, IndexType type)
        {
            // エンディアン解決
            var bytes = TheConverter.GetBytes(value.UInt);
            // 書き込み場所の決定
            var bindex = type.ToByteIndex(destinationIndex);
            // そもそも書き込めるかどうか確認する
            if (Buffer.Length - bindex < 2) // 出力側
                throw new IndexOutOfRangeException();
            // 書き込み
            Buffer[bindex + 0] = bytes[0];
            Buffer[bindex + 1] = bytes[1];
            return this;

        }


        /// <summary>
        /// Puts the word.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="destinationIndex">Destination index.</param>
        /// <param name="type">Type.</param>
        public Programmer PutRevWord(Word value, int destinationIndex, IndexType type)
        {
            // エンディアン解決
            var bytes = TheConverter.GetBytes(value.UInt);
            // 書き込み場所の決定
            var bindex = type.ToByteIndex(destinationIndex);
            // そもそも書き込めるかどうか確認する
            if (Buffer.Length - bindex < 2) // 出力側
                throw new IndexOutOfRangeException();
            // 書き込み
            Buffer[bindex + 0] = bytes[1];
            Buffer[bindex + 1] = bytes[0];
            return this;

        }

        /// <summary>
        /// Puts the DWord.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="destinationIndex">Destination index.</param>
        /// <param name="type">Type.</param>
		public Programmer PutDWord(DWord value, int destinationIndex, IndexType type)
		{
			// エンディアン解決
			var bytes = TheConverter.GetBytes(value.UInt);
			// 書き込み場所の決定
			var bindex = type.ToByteIndex(destinationIndex);
			// そもそも書き込めるかどうか確認する
			if (Buffer.Length - bindex < 4) // 出力側
				throw new IndexOutOfRangeException();
			// 書き込み
			Buffer[bindex + 0] = bytes[0];
			Buffer[bindex + 1] = bytes[1];
			Buffer[bindex + 2] = bytes[2];
			Buffer[bindex + 3] = bytes[3];
            return this;
		}

		/// <summary>
		/// Puts the byte array.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutBytes(Byte[] array, int destinationIndex, int count, IndexType type)
		{
			// 書き込み場所の決定
            var bcount = Math.Min(type.ToByteCount(count), array.Count());
			var bindex = type.ToByteIndex(destinationIndex);
            // そもそも書き込めるかどうか確認する
            //if (array.Length < bcount) // 入力側
                //throw new IndexOutOfRangeException(
                //    "input size: " + array.Length + ", byte count:" + bcount
                //);
            if (Buffer.Length - bindex < bcount) // 出力側
                throw new IndexOutOfRangeException(
                    "Length: " + Buffer.Length + ", count:(" + count + ", " + bcount + "), index:(" + destinationIndex + ", " + bindex + "), index type:" + type
                );
            // 書き込み
            Array.Copy(array, 0, Buffer, bindex, bcount);
            return this;
		}
		/// <summary>
		/// Puts the Word array.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutWords(Word[] array, int destinationIndex, int count, IndexType type)
		{
			Byte[] source = array.SelectMany((v) =>
			{
				// エンディアンの変換
				return TheConverter.GetBytes(v.UInt);
			}).ToArray();
			// 送信
			return PutBytes(source, destinationIndex, count, type);
		}
		/// <summary>
		/// Puts the Dword array.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutDWords(DWord[] array, int destinationIndex, int count, IndexType type)
		{
			Byte[] source = array.SelectMany((v) =>
			{
				// エンディアンの変換
				return TheConverter.GetBytes(v.UInt);
			}).ToArray();
			return PutBytes(source, destinationIndex, count, type);
		}

		/// <summary>
		/// Puts the byte sequence.
		/// </summary>
		/// <param name="seq">Seq.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutBytes (IEnumerable<Byte > seq, int destinationIndex, int count, IndexType type)
        {
            return PutBytes(seq.ToArray(), destinationIndex, count, type);
        }

		/// <summary>
		/// Puts the Word sequence.
		/// </summary>
		/// <param name="seq">Seq.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutWords (IEnumerable<Word > seq, int destinationIndex, int count, IndexType type)
        {
            Byte[] source = seq.SelectMany((v) =>
            {
                // エンディアンの変換
                return TheConverter.GetBytes(v.UInt);
            }).ToArray();
            // 送信
            return PutBytes(source, destinationIndex, count ,type);
        }
		/// <summary>
		/// Puts the Dword sequence.
		/// </summary>
		/// <param name="seq">Seq.</param>
		/// <param name="destinationIndex">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Programmer PutDWords(IEnumerable<DWord> seq, int destinationIndex, int count, IndexType type)
		{
			Byte[] source = seq.SelectMany((v) =>
			{
				// エンディアンの変換
				return TheConverter.GetBytes(v.UInt);
			}).ToArray();
            return PutBytes(source, destinationIndex, count, type);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="type">Type.</param>
        public Byte[] GetBytes(int index, int count, IndexType type)
        {
			// 読み込み場所の決定
			var bcount = type.ToByteCount(count);
			var bindex = type.ToByteIndex(index);
            // 読み込み
            Byte[] dest = new byte[bcount];
            Log.Debug(String.Format("Copying byte ... buffsize:{0}, destsize:{1}, bindex:{2}, bcount:{3}, ", Buffer.Length, dest.Length, bindex, bcount));
            Array.Copy(Buffer, bindex, dest, 0, bcount);
            return dest;
		}


		/// <summary>
		/// Gets the word array.
		/// </summary>
		/// <returns>The word.</returns>
		/// <param name="">.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public Word[] GetWords(int index, int count, IndexType type)
		{
            return GetWordSequence(index, count, type).ToArray();
		}

		/// <summary>
		/// Gets the DWord array.
		/// </summary>
		/// <returns>The DW ord sequence.</returns>
		/// <param name="index">Index.</param>
		/// <param name="count">Count.</param>
		/// <param name="type">Type.</param>
		public DWord[] GetDWords(int index, int count, IndexType type)
		{
            return GetDWordSequence(index, count, type).ToArray();
		}

        /// <summary>
        /// Gets the byte sequence.
        /// </summary>
        /// <returns>The byte sequence.</returns>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="type">Type.</param>
        public IEnumerable<Byte> GetByteSequence(int index, int count, IndexType type)
        {
            return GetBytes(index, count, type);
        }

        /// <summary>
        /// Gets the word sequence
        /// </summary>
        /// <returns>The word.</returns>
        /// <param name="">.</param>
        /// <param name="count">Count.</param>
        /// <param name="type">Type.</param>
        public IEnumerable<Word> GetWordSequence(int index, int count, IndexType type)
		{
            return GetBytes(index, count, type).DiPack().Select(
                (v) => TheConverter.ToWord(v)
            );
        }

        /// <summary>
        /// Gets the DWord sequence.
        /// </summary>
        /// <returns>The DW ord sequence.</returns>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="type">Type.</param>
        public IEnumerable<DWord> GetDWordSequence(int index, int count, IndexType type)
        {
            return GetBytes(index, count, type).TetraPack().Select(
                (v) => TheConverter.ToDWord(v)
            );
        }


        /// <summary>
        /// Gets all bytes.
        /// </summary>
        /// <returns>The all bytes.</returns>
		public Byte[] GetAllBytes()
		{
            return GetBytes(0, Buffer.Length, IndexType.Byte);
		}

        /// <summary>
        /// Gets all words.
        /// </summary>
        /// <returns>The all words.</returns>
		public Word[] GetAllWords()
		{
            return GetWords(0, Buffer.Length, IndexType.Byte);
		}

        /// <summary>
        /// Gets all DWords.
        /// </summary>
        /// <returns>The all DW ords.</returns>
		public DWord[] GetAllDWords()
		{
            return GetDWords(0, Buffer.Length, IndexType.Byte);
		}

        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <returns>The byte.</returns>
        /// <param name="index">Index.</param>
        /// <param name="type">Type.</param>
		public Byte GetByte(int index, IndexType type)
		{
            var s = type.ToByteIndex(index);
            var a = Buffer[s];
            return a;
		}

        /// <summary>
        /// Gets the word.
        /// </summary>
        /// <returns>The word.</returns>
        /// <param name="index">Index.</param>
        /// <param name="type">Type.</param>
		public Word GetWord(int index, IndexType type)
		{
			var s = type.ToByteIndex(index);
			var a = Buffer[s];
			var b = Buffer[s+1];
            return TheConverter.ToWord(DiByte.Init(a, b));
		}

        /// <summary>
        /// Gets the DWord.
        /// </summary>
        /// <returns>The DW ord.</returns>
        /// <param name="index">Index.</param>
        /// <param name="type">Type.</param>
		public DWord GetDWord(int index, IndexType type)
		{
			var s = type.ToByteIndex(index);
			var a = Buffer[s];
			var b = Buffer[s + 1];
			var c = Buffer[s + 2];
			var d = Buffer[s + 3];
			return TheConverter.ToDWord(TetraByte.Init(a, b, c, d));
		}

		/// <summary>
		/// Gets the first byte.
		/// </summary>
		/// <returns>The byte.</returns>
		public Byte GetByte()
		{
            return GetByte(0, IndexType.Byte);
		}

		/// <summary>
		/// Gets the first word.
		/// </summary>
		/// <returns>The word.</returns>
		public Word GetWord()
		{
            return GetWord(0, IndexType.Byte);
		}

        /// <summary>
        /// Gets the first Dword
        /// </summary>
        /// <returns>The DW ord.</returns>
		public DWord GetDWord()
		{
            return GetDWord(0, IndexType.Byte);
		}

    }
}
