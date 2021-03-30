using System;
using Foundation;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class DropdownListCellSource : IEditBoxCellDelegate, IPickerViewCellDelegate
    {
        // Nonnull
        IEditBoxCommonDelegate commonDelegate = EmptyEditBoxCommonDelegate.Instance;

        /// <summary>
        /// EditBoxCell 全てにおいて 共通で呼び出されるデリゲートを表します，
        /// このプロパティは null を返しません．
        /// もし，null を設定したのであれば，その代わりに EmptyEditBoxCommonDelegate.Instance が設定されます
        /// null チェックを行う場合，代わりに CommonDelegate == EmptyEditBoxCommonDelegate.Instance
        /// で実現できます．
        /// 
        /// </summary>
        /// <value>The common delegate.</value>
        protected IEditBoxCommonDelegate CommonDelegate {
            get {
                return commonDelegate;
            }
            set {
                if (value == null)
                    commonDelegate = EmptyEditBoxCommonDelegate.Instance;
                else
                    commonDelegate = value;
            }
        }


        public DropdownListCellSource (IEditBoxCommonDelegate commonDelegate)
        {
            CommonDelegate = commonDelegate;
        }


        public abstract void SetUpdatedListener (Action<IEditBoxCellDelegate> action);


        public abstract void Commit (PickerViewCell cell, UIPickerView pickerView);
        public abstract string GetCurrentValue ();

        public abstract UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView pickerView);
        public abstract void InitializePickerView (PickerViewCell cell, UIPickerView pickerView);

        public abstract bool IsSingleOrEmpty ();
    }
}

