using System;

namespace TokyoChokoku
{
	/// <summary>
	/// クロスプラットフォームでログ出力を行うためのクラスです。
	/// シングルトンです。
	/// 
	/// プラットフォーム毎に
	/// Log.Instance = ILogの実装
	/// を行ってください。
	/// </summary>
	public class Log
	{
		/// <summary>
		/// 
		/// </summary>
		private Log ()
		{
		}
		static ILog _I = new LogDefault();
		public static ILog I
		{
			get{ return _I; }
			set{ _I = value; }
		}

		/// <summary>
		/// </summary>
		public static void Verbose (params String[] messages)
		{
			I.Verbose (messages);
		}

		/// <summary>
		/// </summary>
		public static void Info (params String[] messages)
		{
			I.Info (messages);
		}

		/// <summary>
		/// </summary>
		public static void Debug (params String[] messages)
		{
			I.Debug (messages);
		}

		/// <summary>
		/// </summary>
		public static void Warn (params String[] messages)
		{
			I.Warn (messages);
		}

		/// <summary>
		/// </summary>
		public static void Error (params String[] messages)
		{
			I.Error (messages);
		}
	}
}

