using System;
namespace TokyoChokoku.Communication
{
    public enum IndexType {
        Byte, Word, DWord
    }

    public static class IndexTypeExt {
        public static int ToByteCount(this IndexType self, int count)
		{
			if (count < 0)
				throw new IndexOutOfRangeException(count.ToString());
            return self.ToByteIndex(count);
		}
		public static int ToWordCount(this IndexType self, int count)
		{
			if (count < 0)
				throw new IndexOutOfRangeException(count.ToString());
			return self.ToWordIndex(count);
		}
		public static int ToDWordCount(this IndexType self, int count)
		{
			if (count < 0)
				throw new IndexOutOfRangeException(count.ToString());
			return self.ToDWordIndex(count);
		}

        //public static int IndexToByteIndex(this IndexType self, int index, Endian endian, BytePack pack)
        //{
        //    if (index < 0)
        //        throw new IndexOutOfRangeException(index.ToString());
        //    switch(endian)
        //    {
        //        case Endian.Big:
        //            return self.IndexToByteIndexForBigEndian(index);
        //        case Endian.Little:
        //            return self.IndexToByteIndexForLittleEndian(index, pack);
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        public static int ToByteIndex(this IndexType self, int index)
        {
			switch (self)
			{
				case IndexType.Byte:
					return index;
				case IndexType.Word:
					return index * 2;
				case IndexType.DWord:
					return index * 4;
				default:
					throw new ArgumentOutOfRangeException();
			}
        }

        /// <summary>
        /// Tos the index of the word.
        /// </summary>
        /// <returns>The word index.</returns>
        /// <param name="self">Self.</param>
        /// <param name="index">Index.</param>
		public static int ToWordIndex(this IndexType self, int index)
		{
			switch (self)
			{
				case IndexType.Byte:
					return index / 2;
				case IndexType.Word:
					return index;
				case IndexType.DWord:
					return index * 2;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}


        /// <summary>
        /// Tos the index of the dword.
        /// </summary>
        /// <returns>The word index.</returns>
        /// <param name="self">Self.</param>
        /// <param name="index">Index.</param>
		public static int ToDWordIndex(this IndexType self, int index)
		{
			switch (self)
			{
				case IndexType.Byte:
					return index / 4;
				case IndexType.Word:
					return index / 2;
				case IndexType.DWord:
					return index;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		//static int IndexToByteIndexForLittleEndian(this IndexType self, int index, BytePack pack)
		//{
  //          var interval = pack.Size();
  //          var bindex = self.IndexToByteIndexForBigEndian(index);
  //          var floor  = (bindex / interval) * interval;
  //          var rem    = bindex % interval;
  //          var offset = (interval - rem) % interval;

  //          return floor + offset;
		//}
    }
}
