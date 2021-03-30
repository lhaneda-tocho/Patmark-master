using System;
using System.Collections;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{

	class DiBytePackEnumerable : IEnumerable<DiByte>
	{
		readonly IEnumerable<Byte> Source;
		public DiBytePackEnumerable(IEnumerable<Byte> enumerable)
		{
			Source = enumerable;
		}

		public IEnumerator<DiByte> GetEnumerator()
		{
			return new DiBytePackEnumerator(Source);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	class DiBytePackEnumerator : IEnumerator<DiByte>
	{
        volatile
		Boolean Readable = false;
		DiByte Value = DiByte.Init(0, 0);

		readonly IEnumerator<Byte> Source;

		public DiBytePackEnumerator(IEnumerable<Byte> source)
		{
			Source = source.GetEnumerator();
		}

		object IEnumerator.Current => Current;
		public DiByte Current
		{
			get
			{
				if (!Readable)
					throw new InvalidOperationException();
				return Value;
			}
		}

		public void Dispose()
		{
			Source.Dispose();
		}

		public bool MoveNext()
		{
			DiByte v = Value;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.A = Source.Current;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.B = Source.Current;

			Value = v;
			Readable = true;
			return true;
		}

		public void Reset()
		{
			Source.Reset();
			Readable = false;
		}
	}




	class TetraBytePackEnumerable : IEnumerable<TetraByte>
	{
		readonly IEnumerable<Byte> Source;
		public TetraBytePackEnumerable(IEnumerable<Byte> enumerable)
		{
			Source = enumerable;
		}

		public IEnumerator<TetraByte> GetEnumerator()
		{
			return new TetraBytePackEnumerator(Source);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	class TetraBytePackEnumerator : IEnumerator<TetraByte>
	{
		Boolean Readable = false;
		TetraByte Value = TetraByte.Init(0, 0, 0, 0);

		readonly IEnumerator<Byte> Source;

		public TetraBytePackEnumerator(IEnumerable<Byte> source)
		{
			Source = source.GetEnumerator();
		}

		object IEnumerator.Current => Current;
		public TetraByte Current
		{
			get
			{
				if (!Readable)
					throw new InvalidOperationException();
				return Value;
			}
		}

		public void Dispose()
		{
			Source.Dispose();
		}

		public bool MoveNext()
		{
			TetraByte v = Value;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.A = Source.Current;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.B = Source.Current;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.C = Source.Current;

			if (!Source.MoveNext())
			{
				Readable = false;
				return false;
			}
			v.D = Source.Current;

			Value = v;
			Readable = true;
			return true;
		}

		public void Reset()
		{
			Source.Reset();
			Readable = false;
		}
	}



    class BytePackEnumerable : IEnumerable<Byte[]>
    {
        readonly IEnumerable<Byte> Source;
        readonly int PackSize;
        public BytePackEnumerable(IEnumerable<Byte> enumerable, int packSize)
        {
            Source = enumerable;
            PackSize = packSize;
        }

        public IEnumerator<Byte[]> GetEnumerator()
        {
            return new BytePackEnumerator(Source, PackSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class BytePackEnumerator : IEnumerator<Byte[]>
    {
        Byte[] Value = null;

        readonly IEnumerator<Byte> Source;
        readonly int PackSize;

        public BytePackEnumerator(IEnumerable<Byte> source, int packSize)
        {
            Source = source.GetEnumerator();
            PackSize = packSize;
        }

        object IEnumerator.Current => Current;
        public Byte[] Current
        {
            get
            {
                if (Value == null)
                    throw new InvalidOperationException();
                return (Byte[])Value.Clone();
            }
        }

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            var size = PackSize;
            var bytes = new Byte[size];
            for (int i = 0; i < size; ++i)
            {
                if (!Source.MoveNext())
                {
                    Value = null;
                    return false;
                }
                bytes[i] = Source.Current;
            }

            Value = bytes;
            return true;
        }

        public void Reset()
        {
            Source.Reset();
            Value = null;
        }
    }
}
