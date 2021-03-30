using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TokyoChokoku.Text;
using static TokyoChokoku.Communication.BinalizerUtil;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Communication
{
    /// <summary>
    /// Mapper chain.
    /// </summary>
    public class MapperChain
    {
        int index = 0;
        List<Mapper> list = new List<Mapper>();

        /// <summary>
        /// Alloc the specified size and type.
        /// </summary>
        /// <returns>The alloc.</returns>
        /// <param name="size">Size.</param>
        /// <param name="type">Type.</param>
        public Supplier Alloc(int size, IndexType type)
        {
            var bindex = index;
            var bcount = type.ToByteCount(size);
            index += bcount;

            return new Supplier((mapper) =>
            {
                mapper.ByteIndex = bindex;
                mapper.ByteCount = bcount;
                list.Add(mapper);
            });
        }

        public int TotalByteCount()
        {
            return index;
		}


		public ByteMapper AllocByte()
		{
			return Alloc(1, IndexType.Byte).AsByte();
		}


		public WordMapper AllocWord()
		{
			return Alloc(1, IndexType.Word).AsWord();
		}
		public UInt16Mapper AllocUInt16()
		{
			return Alloc(1, IndexType.Word).AsUInt16();
		}
		public SInt16Mapper AllocSInt16()
		{
			return Alloc(1, IndexType.Word).AsSInt16();
		}


		public DWordMapper AllocDWord()
		{
			return Alloc(1, IndexType.DWord).AsDWord();
		}
		public UInt32Mapper AllocUInt32()
		{
			return Alloc(1, IndexType.DWord).AsUInt32();
		}
		public SInt32Mapper AllocSInt32()
		{
			return Alloc(1, IndexType.DWord).AsSInt32();
		}
		public FloatMapper AllocFloat()
		{
			return Alloc(1, IndexType.DWord).AsFloat();
		}

        /// <summary>
        /// Supplier.
        /// </summary>
        public class Supplier
        {
            /// <summary>
            /// The register.
            /// </summary>
            Action<Mapper> Register;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:TokyoChokoku.Communication.MapperSet.Supplier"/> class.
            /// </summary>
            /// <param name="register">Register.</param>
            public Supplier(Action<Mapper> register)
            {
                Register = register ?? throw new NullReferenceException();
			}

            /// <summary>
            /// Ases the byte.
            /// </summary>
            /// <returns>The byte.</returns>
			public ByteMapper AsByte()
			{
				var mapper = new ByteMapper();
				Register(mapper);
				return mapper;
			}

            /// <summary>
            /// Ases the word.
            /// </summary>
            /// <returns>The word.</returns>
			public WordMapper AsWord()
			{
				var mapper = new WordMapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the DW ord.
            /// </summary>
            /// <returns>The DW ord.</returns>
			public DWordMapper AsDWord()
			{
				var mapper = new DWordMapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the user interface nt16.
            /// </summary>
            /// <returns>The user interface nt16.</returns>
			public UInt16Mapper AsUInt16()
			{
				var mapper = new UInt16Mapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the SI nt16.
            /// </summary>
            /// <returns>The SI nt16.</returns>
			public SInt16Mapper AsSInt16()
			{
				var mapper = new SInt16Mapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the user interface nt32.
            /// </summary>
            /// <returns>The user interface nt32.</returns>
			public UInt32Mapper AsUInt32()
			{
				var mapper = new UInt32Mapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the SI nt32.
            /// </summary>
            /// <returns>The SI nt32.</returns>
			public SInt32Mapper AsSInt32()
			{
				var mapper = new SInt32Mapper();
				Register(mapper);
                return mapper;
			}

            /// <summary>
            /// Ases the float.
            /// </summary>
            /// <returns>The float.</returns>
			public FloatMapper AsFloat()
			{
				var mapper = new FloatMapper();
				Register(mapper);
                return mapper;
			}

   //         /// <summary>
   //         /// Ases the mono text.
   //         /// </summary>
   //         /// <returns>The mono text.</returns>
			//public MonoTextMapper AsMonoText()
			//{
			//	var mapper = new MonoTextMapper();
			//	Register(mapper);
   //             return mapper;
			//}

   //         /// <summary>
   //         /// Ases the wide text.
   //         /// </summary>
   //         /// <returns>The wide text.</returns>
			//public WideTextMapper AsWideText()
			//{
			//	var mapper = new WideTextMapper();
			//	Register(mapper);
			//	return mapper;
			//}
        }
    }

    /// <summary>
    /// Base mapper target.
    /// </summary>
    public interface Mapper
	{
		int ByteIndex { get; set; }
		int ByteCount { get; set; }
    }

    public static class MapperExt
    {
        public static void CheckWritable(this Mapper self, int count, IndexType type)
        {
            var bcount = type.ToByteCount(count);
            if (self.ByteCount < bcount)
                throw new IndexOutOfRangeException();
        }
    }


    /// <summary>
    /// Base Mapper.
    /// </summary>
    public abstract class ConcreteMapper: Mapper
    {
        public int  ByteIndex { get; set; } = 0;
        public int  ByteCount { get; set; } = 0;
    }

    public abstract class ByteWrapMapper: Mapper
	{
		protected readonly ByteMapper Core = new ByteMapper();
        public ByteMapper AsByteMapper => Core;

        public int ByteIndex {
            get => Core.ByteIndex;
            set => Core.ByteIndex = value;
        }
        public int ByteCount { 
            get => Core.ByteCount;
            set => Core.ByteCount = value;
        }
	}

	public abstract class WordWrapMapper : Mapper
	{
		protected readonly WordMapper Core = new WordMapper();
		public WordMapper AsWordMapper => Core;

		public int ByteIndex
		{
			get => Core.ByteIndex;
			set => Core.ByteIndex = value;
		}
		public int ByteCount
		{
			get => Core.ByteCount;
			set => Core.ByteCount = value;
		}
	}

	public abstract class DWordWrapMapper : Mapper
	{
        protected readonly DWordMapper Core = new DWordMapper();
        public DWordMapper AsWordMapper => Core;

        public int ByteIndex
        {
            get => Core.ByteIndex;
            set => Core.ByteIndex = value;
        }
        public int ByteCount
        {
            get => Core.ByteCount;
            set => Core.ByteCount = value;
        }
	}

    /// <summary>
    /// Byte mapper.
    /// </summary>
    public class ByteMapper : ConcreteMapper
    {
        public void PutTo(Programmer programmer, IEnumerable<Byte> value, int index, int count)
        {
            var pre = value.Take(count);
            PutTo(programmer, pre, index);
        }

        public void PutTo(Programmer programmer, IEnumerable<Byte> value, int index = 0)
        {
            var array  = value.ToArray();
            var bindex = index;
            var bcount = array.Count();
            if (ByteCount - bindex < bcount)
                throw new IndexOutOfRangeException();
            programmer.PutBytes(array, ByteIndex + bindex, bcount, IndexType.Byte);
		}

		public void PutTo(Programmer programmer, Byte value, int index = 0)
		{
            PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

        public IEnumerable<Byte> ReadFrom(Programmer programmer)
		{
            return programmer.GetByteSequence(ByteIndex, ByteCount, IndexType.Byte);
        }
    }

    /// <summary>
    /// Word mapper.
    /// </summary>
	public class WordMapper : ConcreteMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<Word> value, int index, int count)
		{
			var pre = value.Take(count);
			PutTo(programmer, pre, index);
		}

		public void PutTo(Programmer programmer, IEnumerable<Word> value, int index = 0)
		{
            var type   = IndexType.Word;
			var array  = value.ToArray();
            var bindex = type.ToByteIndex(index);
            var bcount = type.ToByteCount(array.Count());
			if (ByteCount - bindex < bcount)
				throw new IndexOutOfRangeException();
			programmer.PutWords(array, ByteIndex + bindex, bcount, IndexType.Byte);
		}

		public void PutTo(Programmer programmer, Word value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<Word> ReadFrom(Programmer programmer)
		{
			return programmer.GetWordSequence(ByteIndex, ByteCount, IndexType.Byte);
		}

        public ByteMapper CreateByteUnion(int offset, int count, IndexType type)
		{
			var boffset = type.ToByteIndex(offset);
			var bcount  = type.ToByteCount(count);
            if (ByteCount - boffset > bcount)
                throw new IndexOutOfRangeException();
			var union = new ByteMapper();
            union.ByteIndex = boffset;
            union.ByteCount = bcount;
            return union;
        }


        public ByteMapper CreateByteUnion()
        {
            return CreateByteUnion(0, ByteCount, IndexType.Byte);
        }
	}

	/// <summary>
	/// DWord mapper.
	/// </summary>
	public class DWordMapper : ConcreteMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<DWord> value, int index, int count)
		{
			var pre = value.Take(count);
			PutTo(programmer, pre, index);
		}

		public void PutTo(Programmer programmer, IEnumerable<DWord> value, int index = 0)
		{
			var type = IndexType.DWord;
			var array = value.ToArray();
			var bindex = type.ToByteIndex(index);
			var bcount = type.ToByteCount(array.Count());
			if (ByteCount - bindex < bcount)
				throw new IndexOutOfRangeException();
			programmer.PutDWords(array, ByteIndex + bindex, bcount, IndexType.Byte);
		}

		public void PutTo(Programmer programmer, DWord value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<DWord> ReadFrom(Programmer programmer)
		{
			return programmer.GetDWordSequence(ByteIndex, ByteCount, IndexType.Byte);
		}
	}

    /// <summary>
    /// UInt16 mapper.
    /// </summary>
    public class UInt16Mapper : WordWrapMapper
	{
        public void PutTo(Programmer programmer, IEnumerable<UInt16> value, int index, int count)
		{
            var seq = value.Select((v) => Word.Init(v));
            Core.PutTo(programmer, seq, index, count);
		}

		public void PutTo(Programmer programmer, IEnumerable<UInt16> value, int index = 0)
		{
			var seq = value.Select((v) => Word.Init(v));
			Core.PutTo(programmer, seq, index);
		}

		public void PutTo(Programmer programmer, UInt16 value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<UInt16> ReadFrom(Programmer programmer)
		{
            return Core.ReadFrom(programmer).Select((v) => 
            {
                return v.UInt;
            });
		}
    }

    /// <summary>
    /// Int16 mapper.
    /// </summary>
    public class SInt16Mapper : WordWrapMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<Int16> value, int index, int count)
		{
			var seq = value.Select((v) => Word.Init(v));
			Core.PutTo(programmer, seq, index, count);
		}

		public void PutTo(Programmer programmer, IEnumerable<Int16> value, int index = 0)
		{
			var seq = value.Select((v) => Word.Init(v));
			Core.PutTo(programmer, seq, index);
		}

		public void PutTo(Programmer programmer, Int16 value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<Int16> ReadFrom(Programmer programmer)
		{
			return Core.ReadFrom(programmer).Select((v) =>
			{
				return v.SInt;
			});
		}
	}

	/// <summary>
	/// UInt32 mapper.
	/// </summary>
    public class UInt32Mapper : DWordWrapMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<UInt32> value, int index, int count)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index, count);
		}

		public void PutTo(Programmer programmer, IEnumerable<UInt32> value, int index = 0)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index);
		}

		public void PutTo(Programmer programmer, UInt32 value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<UInt32> ReadFrom(Programmer programmer)
		{
			return Core.ReadFrom(programmer).Select((v) =>
			{
				return v.UInt;
			});
		}
	}

	/// <summary>
	/// SInt32 mapper.
	/// </summary>
    public class SInt32Mapper : DWordWrapMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<Int32> value, int index, int count)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index, count);
		}

		public void PutTo(Programmer programmer, IEnumerable<Int32> value, int index = 0)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index);
		}

		public void PutTo(Programmer programmer, Int32 value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<Int32> ReadFrom(Programmer programmer)
		{
			return Core.ReadFrom(programmer).Select((v) =>
			{
				return v.SInt;
			});
		}
	}


    /// <summary>
    /// Float mapper.
    /// </summary>
    public class FloatMapper : DWordWrapMapper
	{
		public void PutTo(Programmer programmer, IEnumerable<Single> value, int index, int count)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index, count);
		}

		public void PutTo(Programmer programmer, IEnumerable<Single> value, int index = 0)
		{
			var seq = value.Select((v) => DWord.Init(v));
			Core.PutTo(programmer, seq, index);
		}

		public void PutTo(Programmer programmer, Single value, int index = 0)
		{
			PutTo(programmer, Enumerable.Repeat(value, 1), index);
		}

		public IEnumerable<Single> ReadFrom(Programmer programmer)
		{
			return Core.ReadFrom(programmer).Select((v) =>
			{
				return v.Float;
			});
		}
    }
}
