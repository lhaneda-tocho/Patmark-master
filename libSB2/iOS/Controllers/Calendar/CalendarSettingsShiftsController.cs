using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class CalendarSettingsShiftsController : UITableViewController
    {
        public CalendarSettingsShiftsController (IntPtr handle) : base (handle)
        {
        }

        List<Properties.Stores.CalendarShiftStore> ShiftStores;

        override
        public void ViewDidLoad()
        {
            base.ViewDidLoad();

            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-calendar-shifts.title", "");

            //SetupTable();
        }

        override
        public void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SetupTable();
        }

        private void SetupTable()
        {
            ShiftStores = CalendarSettingsManager.Instance.CreateShiftStores();
            var names = new List<string>();
            for (var i = 0; i < ShiftStores.Count; i++)
            {
                var store = ShiftStores[i];
                names.Add(
                    String.Format(
                        "No.{0}「{1}」{2:00}:{3:00} 〜 {4:00}:{5:00}",
                        i + 1,
                        store.CodeStore.Content.ToString(),
                        (int)store.StartingHourStore.Content,
                        (int)store.StartingMinuteStore.Content,
                        (int)store.EndingHourStore.Content,
                        (int)store.EndingMinuteStore.Content
                    )
                );
            }
            ShiftsView.Source = new MyTableSource(this, names);
            ShiftsView.LayoutIfNeeded();
        }

        private class MyTableSource : StringTableSource
        {
            UITableViewController Ctrl;

            public MyTableSource(UITableViewController ctrl, List<string> items) : base(items)
            {
                this.Ctrl = ctrl;
            }

            override
            public void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Ctrl.PerformSegue("ShowShiftEditor", this);
            }
        }

        override
        public void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("ShowShiftEditor"))
            {
                var dst = (CalendarSettingsShiftController)segue.DestinationViewController;
                dst.Title = "No." + (ShiftsView.IndexPathForSelectedRow.Row + 1);
                dst.ShiftStore = ShiftStores[ShiftsView.IndexPathForSelectedRow.Row];
            }
        }

        /// <summary>
        /// Unwind segue's jump point.
        /// </summary>
        /// <param name="segue">Segue.</param>
        [Action("OnShiftCommited:")]
        public void OnShiftCommited(UIStoryboardSegue segue)
        {
            SetupTable();
        }
    }
}