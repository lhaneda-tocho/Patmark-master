using Foundation;
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class CalendarSettingsMonthController : UICollectionViewController
	{
		public CalendarSettingsMonthController (IntPtr handle) : base (handle)
        {
        }

        override
        public void ViewDidLoad()
        {
            // ナビゲーションバーのタイトルを設定します。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-calendar-month.title", "");

            var containers = new List<CalendarSettingsYmdDataContainer>();
            var stores = CalendarSettingsManager.Instance.CreateMonthStores();
            for (var i = 0; i < stores.Count; i++)
            {
                containers.Add(new CalendarSettingsYmdDataContainer((i+1).ToString(), stores[i]));
            }
            InputCollection.RegisterNibForCell(CalendarSettingsYmdCell.Nib, CalendarSettingsYmdCell.CellId);
            InputCollection.Source = new CalendarSettingsYmdDataSource(containers);
		}
	}
}
