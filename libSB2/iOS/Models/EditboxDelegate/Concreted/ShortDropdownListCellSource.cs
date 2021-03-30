using System;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// 16bit整数型のデータをドロップダウンリストの選択肢として扱うクラスです．
    /// </summary>
    public class ShortDropdownListCellSource : SemiConcretedDropdownListCellSource<short>
    {

        public ShortDropdownListCellSource (Store<short> store, short [] items, IEditBoxCommonDelegate commonDelegate)
            : base (store, items, commonDelegate)
        {
        }

        public override string AsString (short value)
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

