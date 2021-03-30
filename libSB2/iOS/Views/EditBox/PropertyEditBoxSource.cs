using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Collections;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class PropertyEditBoxSource
        :   UITableViewSource, 
            IEnumerable<KeyValuePair<string, List<IEditBoxCell>>>
	{
		// 元データ
        Dictionary<string, List<IEditBoxCell>> Cells;
		string[] IndexKeys;

        // arguments: row , section, sender
        public event Action<NSIndexPath, PropertyEditBoxSource> OnInsertRequested;
        public event Action<NSIndexPath, PropertyEditBoxSource> OnRemoveRequested;

        public PropertyEditBoxSource(List<IEditBoxCell> containers)
		{
            Cells = new Dictionary<string, List<IEditBoxCell>>();

			foreach (var item in containers) {
                var key = item.SectionName;
				if (Cells.ContainsKey (key)) {
					
					Cells[key].Add(item);

				} else {
					
					Cells.Add (key, new List<IEditBoxCell> {item});
				}
			}

			IndexKeys = new string[Cells.Keys.Count];
			Cells.Keys.CopyTo (IndexKeys, 0);
		}


        public void AcceptInteraction (string interactionId, IEditBoxInteractionDelegate sender)
        {
            sender.Interact (interactionId, this);
        }

        public void PurgeTableViewUpdateEvent ()
        {
            OnInsertRequested = null;
            OnRemoveRequested = null;
        }


        // 表の操作関係
        public void Insert (nint row, nint section, Func<IEditBoxCell> factory)
        {
            if (section < 0 || section >= IndexKeys.Length) {
                Console.Error.WriteLine ("PropertyEditBoxSource#Insert : Invalid row or section number.");
                return;
            }
                
                
            var path = NSIndexPath.FromRowSection (row, section);
            var list = Cells [IndexKeys [section]];
            if (row >= 0 && row < list.Count+1) {
                // 表の操作が可能である場合
                list.Insert ((int)row, factory());
                if (OnInsertRequested != null)
                    OnInsertRequested (path, this);
            }
            else
                Console.Error.WriteLine ("PropertyEditBoxSource#Insert : Invalid row or section number.");
        }

        public void Remove (nint row, nint section)
        {
            var path = NSIndexPath.FromRowSection (row, section);
            var list = Cells [IndexKeys [section]];
            if (row < list.Count) {
                // 表の操作が可能である場合
                list.RemoveAt ((int)row);
                if (OnRemoveRequested != null)
                    OnRemoveRequested (path, this);
            }
            else
                Console.Error.WriteLine ("PropertyEditBoxSource#Remove : Invalid row or section number.");
        }

        /// <summary>
        /// 指定されたセクションから，同じIDを持つ行を見つけ出し，その行番号を返します．
        /// セクションまたは該当する行が見つからなかった時は -1 を返します．
        /// </summary>
        /// <returns>The row in section.</returns>
        /// <param name="sectionName">Section name.</param>
        public nint SearchRowInSection (string sectionName, string id)
        {
            if (!Cells.ContainsKey (sectionName))
                return -1;
            var list = Cells [sectionName];
            nint row = 0;
            foreach (var e in list) {
                if (e.Identifier.Equals (id)) {
                    return row;
                }
                ++row;
            }
            return -1;
        }

        /// <summary>
        /// 指定されたセクションを見つけ出し，そのセクション番号を返します．
        /// 見つからなかった時は -1 を返します．
        /// </summary>
        /// <returns>The row in section.</returns>
        /// <param name="sectionName">Section name.</param>
        public nint SearchSection (string sectionName)
        {
            nint index = 0;
            foreach (var e in IndexKeys) {
                if (e.Equals (sectionName))
                    return index;
                ++index;
            }
            return -1;
        }






        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        public IEnumerator<KeyValuePair<string, List<IEditBoxCell>>> GetEnumerator ()
        {
            return Cells.GetEnumerator ();
        }


        public override
        nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var sectionKey = IndexKeys[ indexPath.Section ];
			var rowList = Cells [sectionKey];
            return rowList [indexPath.Row].AsTableViewCell ().Frame.Height;
		}


        public override
        UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            // UITableViewCellの取得
            var sectionKey = IndexKeys [indexPath.Section];
            var cell = Cells [sectionKey][indexPath.Row].AsTableViewCell ();

            if (cell is IEditBoxInteractionDelegate) {
                var interactable = (IEditBoxInteractionDelegate)cell;
                interactable.ClearListeners ();
                interactable.RequestInteraction += AcceptInteraction;
            }

            return cell;
        }


        /// <summary>
        /// section名と cell id が合致するセルを返します．見つからなかった場合は null を返します．
        /// </summary>
        /// <returns>The cell from identifier.</returns>
        /// <param name="section">Section.</param>
        /// <param name="id">Identifier.</param>
        public IEditBoxCell GetCellFromId (string section, string id)
        {
            nint rowNo = SearchRowInSection (section, id);
            if (rowNo == -1)
                return null;
            return Cells [section] [(int)rowNo];
        }


		// Index
        public override
        nint NumberOfSections (UITableView tableView)
		{
			return IndexKeys.Length;
		}

        public override
        nint RowsInSection (UITableView tableview, nint section)
		{
			return Cells[ IndexKeys[section] ].Count;
		}

        public override
        string[] SectionIndexTitles (UITableView tableView)
		{
			return IndexKeys;
		}
    }
}

