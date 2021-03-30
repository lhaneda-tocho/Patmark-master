using System;
using System.Diagnostics;

using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.iOS.Models.Logging
{
	public class LogPlatform : LogDefault
	{
		public override void Verbose (params String[] messages)
		{
			base.Verbose (messages);
		}

		public override void Info (params String[] messages)
        {
			base.Info (messages);
		}

		public override void Debug (params String[] messages)
		{
			base.Debug (messages);
		}

		public override void Warn (params String[] messages)
		{
			base.Warn (messages);
		}

		public override void Error (params String[] messages)
		{
			base.Error (messages);
		}
	}
}

