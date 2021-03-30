using System;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class DotCountDropdownListCellSource : SemiConcretedDropdownListCellSource<DotCount2D>
    {

        public DotCountDropdownListCellSource (Store<DotCount2D> store, DotCount2D [] items, IEditBoxCommonDelegate commonDelegate)
            : base (store, AddInitialValueAsNeeded (items, store.Content), commonDelegate)
        {
        }

        public override string AsString (DotCount2D value)
        {
            return value.ToTitle ();
        }

        public override nint GetStartRow ()
        {
            for (nint i = 0; i < items.Length; i++)
                if (items [i] == store.Content)
                    return i;
            return 0;
        }

        public static DotCount2D [] AddInitialValueAsNeeded (DotCount2D [] items, DotCount2D initialValue)
        {
            for (nint i = 0; i < items.Length; i++)
                if (items [i] == initialValue)
                    return items;

            var newItems = new DotCount2D [items.Length + 1];
            newItems [0] = initialValue;
            Array.Copy (items, 0, newItems, 1, items.Length);
            return newItems;
        }
    }
}

