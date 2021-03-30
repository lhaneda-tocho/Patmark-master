using System;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class UITableViewCellIndexContainer
	{
		public string Code;
        public IEditBoxCell Cell;
        public UITableViewCellIndexContainer(string code, IEditBoxCell cell){
			this.Code = code;
			this.Cell = cell;
		}
	}
}

