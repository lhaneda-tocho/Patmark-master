using System;
using System.Runtime.InteropServices;
using static TokyoChokoku.Communication.BinalizerUtil;
using static BitConverter.EndianBitConverter;
using BitConverter;

namespace TokyoChokoku.Communication
{
	/// <summary>
	/// 2バイト．
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct DiByte
	{
		[FieldOffset(0)]
		public Byte A;

		[FieldOffset(1)]
		public Byte B;

		public static DiByte Init(Byte a, Byte b)
		{
			var v = new DiByte();
			v.A = a;
			v.B = b;
			return v;
		}

		public static DiByte Init(Byte[] src, int startIndex = 0)
		{
			return Init(src[0 + startIndex], src[1 + startIndex]);
		}

		/// <summary>
		/// バイトの並びを逆転します
		/// </summary>
		/// <returns>The flip.</returns>
		public DiByte Flip()
		{
			return Init(B, A);
		}

		public Byte[] ToBytes()
		{
			return new byte[] { A, B };
		}
	}


	/// <summary>
	/// 4バイト．
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct TetraByte
	{
		[FieldOffset(0)]
		public Byte A;

		[FieldOffset(1)]
		public Byte B;

		[FieldOffset(2)]
		public Byte C;

		[FieldOffset(3)]
		public Byte D;

		/// <summary>
		/// AB の順で Word を形成します.
		/// Wordから得られる値は システムのエンディアンに依存します
		/// </summary>
		[FieldOffset(0)]
		public DiByte AB;

		/// <summary>
		/// CD の順で Word を形成します.
		/// Wordから得られる値は システムのエンディアンに依存します
		/// </summary>
		[FieldOffset(2)]
		public DiByte CD;


		public static TetraByte Init(Byte[] src, int startIndex = 0)
		{
			return Init(src[0 + startIndex], src[1 + startIndex], src[2 + startIndex], src[3 + startIndex]);
		}

		public static TetraByte Init(Byte a, Byte b, Byte c, Byte d)
		{
			var v = new TetraByte();
			v.A = a;
			v.B = b;
			v.C = c;
			v.D = d;
			return v;
		}

		public static TetraByte Init(DiByte ab, DiByte cd)
		{
			var v = new TetraByte();
			v.AB = ab;
			v.CD = cd;
			return v;
		}

		/// <summary>
		/// バイトの並びを逆転します
		/// </summary>
		/// <returns>The flip.</returns>
		public TetraByte Flip()
		{
			return Init(D, C, B, A);
		}

		public Byte[] ToBytes()
		{
			return new byte[] { A, B, C, D };
		}

	}
}
