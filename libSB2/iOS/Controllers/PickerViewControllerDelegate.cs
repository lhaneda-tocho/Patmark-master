using System;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class PickerViewControllerDelegate : UIPickerViewModel
    {
        public abstract void   Commit (UIPickerView pickerView);

        public abstract void   Initialize (UIPickerView pickerView);
        public abstract string GetSummary ();

        public abstract nfloat GetPopupWidth ();
        public abstract nfloat GetPopupHeight ();
    }
}

