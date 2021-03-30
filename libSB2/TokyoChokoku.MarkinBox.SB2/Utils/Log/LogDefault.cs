using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public class LogDefault : ILog
	{
		public virtual void Verbose (params String[] messages)
		{
			System.Diagnostics.Debug.WriteLine (String.Format ("{0} : {1}", "Verbose", String.Join(" : ", messages)));
		}

		public virtual void Info (params String[] messages)
		{
			System.Diagnostics.Debug.WriteLine (String.Format ("{0} : {1}", "Info", String.Join(" : ", messages)));
		}

		public virtual void Debug (params String[] messages)
		{
			System.Diagnostics.Debug.WriteLine (String.Format ("{0} : {1}", "Debug", String.Join(" : ", messages)));
		}

		public virtual void Warn (params String[] messages)
		{
			System.Diagnostics.Debug.WriteLine (String.Format ("{0} : {1}", "Warn", String.Join(" : ", messages)));
		}

		public virtual void Error (params String[] messages)
		{
			System.Diagnostics.Debug.WriteLine (String.Format ("{0} : {1}", "Error", String.Join(" : ", messages)));
		}
	}
}

