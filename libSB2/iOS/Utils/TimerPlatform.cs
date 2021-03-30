using System;
using System.Threading;
using System.ComponentModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class TimerPlatform
	{
		/// <summary>
		/// like a 'setTimeout' of JavaScript
		/// </summary>
		public static BackgroundWorker SetTimeout(Action fn, int time)
		{
			BackgroundWorker worker = new BackgroundWorker();

			worker.DoWork += (sender, e) => 
			{
				Thread.Sleep(time);
			};

			worker.RunWorkerCompleted += (sender, e) => 
			{
				fn();
			};

			worker.RunWorkerAsync();

			return worker;
		}
	}
}

