using System;
using UIKit;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface IPickerViewCellDelegate
    {
        UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView pickerView);

        /// <summary>
        /// 現在選択されている値を取得します．
        /// </summary>
        /// <returns>The current value.</returns>
        string GetCurrentValue ();

        /// <summary>
        /// PickerViewCellで選択されている内容を実際のデータに反映するメソッドです．
        /// PickerViewCellが閉じられる時に呼び出されます．
        /// </summary>
        /// <param name="cell">Cell.</param>
        /// <param name="pickerView">Picker view.</param>
        void Commit (PickerViewCell cell, UIPickerView pickerView);

        void InitializePickerView (PickerViewCell cell, UIPickerView pickerView);
    }
}

