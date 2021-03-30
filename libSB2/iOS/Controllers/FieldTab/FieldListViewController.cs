using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class FieldListViewController : UITableViewController
    {
        static readonly string EmptyIdentifier = "-";

        public FieldListViewController (IntPtr handle) : base (handle)
        {
        }

        public iOSFieldManager FieldManager;

        /// <summary>
        /// Views the did load.
        /// </summary>
        override
        public void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (FieldManager == null)
                throw new NullReferenceException("[RemoteFileMenuController - ViewDidLoad]Previewをセットしてください。");

            SetupCancelButton();
            SetupTable();
        }

        void SetupCancelButton()
        {
            CancelButton.Clicked += (sender, args) =>
            {
                DismissViewController(true, null);
            };
        }

        void SetupTable()
        {
            var elements = new List<MyTableElement>();
            int i = 0;
            foreach (var field in FieldManager.FieldList) {
                elements.Add (new MyTableElement (i + 1, field));
                ++i;
            }
            FieldTable.DataSource = new MyTableViewSource(elements);
            FieldTable.ReloadData();
            FieldTable.SetNeedsDisplay();
        }

        // UITableViewDelegate

        override
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var source = (MyTableViewSource)FieldTable.DataSource;
            var element = source.GetElement(indexPath);
            DismissViewController(true, null);
            InvokeOnMainThread(() => {
                FieldManager.Edit (element.Owner);
            });
        }


        sealed class MyTableElement
        {
            public int Number { get; }
            public iOSOwner Owner { get; }

            public string Identitifer
            {
                get
                {
                    return Number.ToString();
                }
            }

            public MyTableElement(int number, iOSOwner owner)
            {
                Number = number;
                Owner = owner;
            }
        }


        sealed class MyTableViewSource : UITableViewDataSource
        {


            List<MyTableElement> elementList = new List<MyTableElement>();


            public MyTableViewSource(List<MyTableElement> cellList)
            {
                elementList = cellList;
            }


            override
            public nint RowsInSection(UITableView tableView, nint section)
            {
                if (section == 0)
                {
                    return elementList.Count;
                }
                return 0;
            }

            override
            public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (elementList.Count == 0)
                {
                    UITableViewCell empty = tableView.DequeueReusableCell(EmptyIdentifier);

                    if (empty == null)
                    {
                        empty = new UITableViewCell(UITableViewCellStyle.Default, EmptyIdentifier);
                    }

                    empty.TextLabel.Text = "Empty";
                }

                var element = elementList[indexPath.Row];

                UITableViewCell cell = tableView.DequeueReusableCell(element.Identitifer);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, element.Identitifer);
                }
                cell.TextLabel.Text = element.Number + ":" + element.Owner.FieldType;
                return cell;
            }

            public MyTableElement GetElement(NSIndexPath path)
            {
                return elementList[path.Row];
            }



        }
    }
}