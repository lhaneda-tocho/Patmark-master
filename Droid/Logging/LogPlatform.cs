using System;
using System.Diagnostics;

using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.Droid.Logging
{
	public class LogPlatform : LogDefault
	{
		/// <summary>
		/// 呼び出しメソッドの情報を記録します。
		/// 開発中のみ実行されます。
		/// </summary>
		private void DevelopInfo(params String[] messages)
		{
			/*
			#if DEBUG
			StackFrame sf = new StackFrame(2, true);
			System.Diagnostics.Debug.WriteLine (
				String.Format (
					"-> {1}, {2}, {2}",
					sf.GetMethod().ToString(),
					sf.GetFileName(),
					sf.GetFileLineNumber()
				)
			);
			#endif
			*/
            foreach (String msg in messages)
            {
                Console.WriteLine(msg);
            }
		}

		public override void Verbose (params String[] messages)
		{
            Android.Util.Log.Verbose(ToString(), String.Join(", ",messages));
		}

		public override void Info (params String[] messages)
        {
            Android.Util.Log.Info(ToString(), String.Join(", ", messages));
		}

		public override void Debug (params String[] messages)
		{
            Android.Util.Log.Debug(ToString(), String.Join(", ", messages));
		}

		public override void Warn (params String[] messages)
		{
            Android.Util.Log.Warn(ToString(), String.Join(", ", messages));
		}

		public override void Error (params String[] messages)
		{
            Android.Util.Log.Error(ToString(), String.Join(", ", messages));
		}
	}
}

