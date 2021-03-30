using System;
using System.Diagnostics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
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
			base.Verbose (messages);
			DevelopInfo (messages);
		}

		public override void Info (params String[] messages)
        {
            foreach (String msg in messages)
            {
                Console.WriteLine(msg);
            }
			base.Info (messages);
			DevelopInfo (messages);
		}

		public override void Debug (params String[] messages)
		{
			base.Debug (messages);
			DevelopInfo (messages);
		}

		public override void Warn (params String[] messages)
		{
			base.Warn (messages);
			DevelopInfo (messages);
		}

		public override void Error (params String[] messages)
		{
			base.Error (messages);
			DevelopInfo (messages);
		}
	}
}

