using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class StringTableSource : UITableViewSource
    {
        List<string> TableItems;
        string CellIdentifier = "TableCell";

        public StringTableSource(List<string> items)
        {
            TableItems = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = item;

            return cell;
        }
    }
}

