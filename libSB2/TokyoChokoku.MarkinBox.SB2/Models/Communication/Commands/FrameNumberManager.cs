using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public class FrameNumberManager
	{
		private static FrameNumberManager _Instance = new FrameNumberManager();
		public static FrameNumberManager Instance { get { return _Instance; } }

		int Number = 0;

		public byte GetNumber(){
			Number = (Number + 1) % 255;
			return (byte)Number;
		}
	}
}

