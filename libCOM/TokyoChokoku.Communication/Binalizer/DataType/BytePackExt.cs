using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
	public static class BytePackEnumerableExt
	{
		public static IEnumerable<Byte[]> Pack(this IEnumerable<Byte> source, int size)
		{
			return new BytePackEnumerable(source, size);
		}

		public static IEnumerable<DiByte> DiPack(this IEnumerable<Byte> source)
		{
			return new DiBytePackEnumerable(source);
		}

		public static IEnumerable<TetraByte> TetraPack(this IEnumerable<Byte> source)
		{
			return new TetraBytePackEnumerable(source);
		}

		public static IEnumerable<Byte> Flatten(this IEnumerable<DiByte> source)
		{
			return source.SelectMany((v) => v.ToBytes());
		}

		public static IEnumerable<Byte> Flatten(this IEnumerable<TetraByte> source)
		{
			return source.SelectMany((v) => v.ToBytes());
		}
	}
}
