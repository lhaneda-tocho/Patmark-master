using System;
using UIKit;
using System.Collections.Generic;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class CalendarSettingsShiftTableSource : UITableViewSource
	{
		//データ
		List<UITableViewCellIndexContainer> TableItems;
		

        public CalendarSettingsShiftTableSource(List<UITableViewCellIndexContainer> containers)
		{
            TableItems = containers;
		}

		override 
		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
			// UITableViewCellの取得
			var cell = tableView.DequeueReusableCell(indexPath.Row.ToString());
			if (cell == null) {
                cell = TableItems [indexPath.Row].Cell.AsTableViewCell ();
				cell.AccessibilityIdentifier = indexPath.Row.ToString ();
			}
			return cell;
		}



		override
		public nfloat GetHeightForRow(UITableView view, NSIndexPath indexPath)
		{
            var cell = TableItems [indexPath.Row].Cell.AsTableViewCell ();
			cell.AccessibilityIdentifier = indexPath.Row.ToString ();
			return cell.Frame.Height;
		}

		override
		public nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Count;
		}
	}
}

