using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class EditBoxCellController <DelegateType>
        where DelegateType : class, IEditBoxCellDelegate
    {
        PropertyEditBox           TableView { get; }
        EditBoxCell<DelegateType> Cell { get; }


        internal EditBoxCellController (PropertyEditBox parent, EditBoxCell<DelegateType> child)
        {
            TableView = parent;
            Cell = child;
        }

    }
}

