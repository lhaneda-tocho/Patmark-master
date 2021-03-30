using System;
using UIKit;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface IEditBoxCell
    {

        /// <summary>
        /// セクション名
        /// </summary>
        /// <returns>The section name.</returns>
        string SectionName { get; }

        /// <summary>
        /// セルに割り当てる ID． セルを探す際に利用します
        /// </summary>
        /// <returns>The identifier.</returns>
        string Identifier { get; }

        UITableViewCell AsTableViewCell ();
        CellType As<CellType> () where CellType : UITableViewCell;

        void UpdateContains (IEditBoxCellDelegate source);
    }
}

