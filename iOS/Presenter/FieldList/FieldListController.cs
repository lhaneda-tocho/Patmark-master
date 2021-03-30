using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using TokyoChokoku.Structure;



namespace TokyoChokoku.Patmark.iOS
{

    public partial class FieldListController : UITableViewController
    {
        List<CellSource> CellSourceList { get; set; }

        public FieldListController() : base("FieldListController", null)
        {
        }

        public FieldListController(IntPtr handle) : base(handle)
        {
        }

        public void SetFields(IEnumerable<FieldReader> fields)
        {
            CellSourceList = ToCellSourceList(fields);
            Reload();
        }

        void Reload()
        {
            if (FieldList != null)
            {
                FieldList.ReloadData();
            }
        }

        // ---- TableView Delegate ----
        override
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //var element = GetElement(indexPath);
            //TableView.DeselectRow(indexPath, true);
        }

        override
        public nint RowsInSection(UITableView tableView, nint section)
        {
            if (section == 0)
                return CellSourceList.Count;
            return 0;
        }

        override
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (CellSourceList.Count == 0)
                CellSource.Empty.ToCell(tableView);
            return CellSourceList[indexPath.Row].ToCell(tableView);
        }


        CellSource GetElement(NSIndexPath path)
        {
            return CellSourceList[path.Row];
        }


        // ---- Utility ----
        static CellSource ToCellSource(FieldReader source)
        {
            return CellSource.Create(String.Format("FieldTypeName_{0}", source.FieldType).Localize(), source.Text);
        }

        static List<CellSource> ToCellSourceList(IEnumerable<FieldReader> fields)
        {
            return (
                from field in fields
                select ToCellSource(field)
            ).ToList();
        }



        sealed class CellSource
        {
            static readonly string CellId = "fsFileMenuCell";

            public string Text { get; }
            public string TypeName { get; }

            internal CellSource(string typeName, string text)
            {
                Text = text;
                TypeName = typeName;
            }

            internal static CellSource Create(string typeName, string text)
            {
                return new CellSource(typeName, text);
            }

            internal static CellSource Empty
            {
                get
                {
                    return new CellSource("", "");
                }
            }

            // cell abstract factory
            internal UITableViewCell ToCell(UITableView tableView)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(CellId);
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellId);
                }
                cell.TextLabel.Text = TypeName;
                if (!String.IsNullOrEmpty(Text))
                {
                    cell.TextLabel.Text += String.Format(" : {0}", Text);
                }
                return cell;
            }
        }
    }
}