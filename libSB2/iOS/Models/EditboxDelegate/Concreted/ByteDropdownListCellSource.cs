using System;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class ByteDropdownListCellSource : SemiConcretedDropdownListCellSource<byte>
    {

        public ByteDropdownListCellSource (Store<byte> store, byte [] items, IEditBoxCommonDelegate commonDelegate)
            : base (store, items, commonDelegate)
        {
        }

        public override string AsString (byte value)
        {
            return value.ToString ();
        }

        public override nint GetStartRow ()
        {
            for (nint i = 0; i < items.Length; i++)
                if (items [i] == store.Content)
                    return i;
            return 0;
        }
    }
}

