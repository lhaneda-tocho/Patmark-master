using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class CalendarSettingsYmdDataSource : UICollectionViewSource
    {
        protected const int SectionCount = 1;

        private List<CalendarSettingsYmdDataContainer> Containers;

        public CalendarSettingsYmdDataSource (List<CalendarSettingsYmdDataContainer> containers)
        {
            this.Containers = containers;
        }

        public override nint NumberOfSections (UICollectionView collectionView)
        {
            return SectionCount;
        }

        public override nint GetItemsCount (UICollectionView collectionView, nint section)
        {
            return Containers.Count / SectionCount;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(CalendarSettingsYmdCell.CellId, indexPath) as CalendarSettingsYmdCell;
            Console.WriteLine (cell);
            if (cell != null) {
                var container = this.Containers[indexPath.Row];
                cell.Update (container.Title, container.ValueStore);
            }
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath) as CalendarSettingsYmdCell;
            if (cell != null) {
                cell.OnSelected ();
            }
        }
    }
}

