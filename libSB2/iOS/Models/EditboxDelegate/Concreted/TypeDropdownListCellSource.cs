using System;
using UIKit;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class TypeDropdownListCellSource : DropdownListCellSource
    {
        Editor editor;

        FieldType [] Items {
            get {
                var items = new FieldType [editor.ChangeableTypes.Count + 1];
                items [0] = editor.FieldType;
                editor.ChangeableTypes.CopyTo (items, 1);
                return items;
            }
        }



        public TypeDropdownListCellSource (Editor editor, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
        {
            if (editor == null)
                throw new NullReferenceException ();

            this.editor = editor;
        }

        public override void Commit (PickerViewCell cell, UIPickerView pickerView)
        {
            nint row = pickerView.SelectedRowInComponent (0);
            if (row == 0)
                return;
            var result = editor.TryChangeTypeTo (Items [row]);
            if (result.HasError) {
                cell.PopupErrorMessage (result);
                CommonDelegate.Apply ();
            } else {
                CommonDelegate.Rebuild ();
            }
        }

        public override UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView pickerView)
        {
            return new MyPickerViewModel (this);
        }

        public override void InitializePickerView (PickerViewCell cell, UIPickerView pickerView)
        {
            nint row = 0;
            {
                var items = Items;
                for (nint i = 0; i < items.Length; i++) {
                    if (items [i] == editor.FieldType) {
                        row = i;
                        break;
                    }
                }
            }
            pickerView.Select (row, 0, false);
        }

        public override string GetCurrentValue ()
        {
            return editor.FieldType.GetName ();
        }

        public override void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
        {
            // undetectable
        }

        public override bool IsSingleOrEmpty ()
        {
            return Items.Length <= 1;
        }


        sealed class MyPickerViewModel : UIPickerViewModel
        {
            TypeDropdownListCellSource source;

            public MyPickerViewModel (TypeDropdownListCellSource source)
            {
                this.source = source;
            }

            // カラム数
            public override nint GetComponentCount (UIPickerView pickerView)
            {
                return 1;
            }


            // 行数
            public override nint GetRowsInComponent (UIPickerView pickerView, nint component)
            {
                return source.editor.ChangeableTypes.Count + 1;
            }


            // 文字列取得
            public override string GetTitle (UIPickerView pickerView, nint row, nint component)
            {
                return source.Items [row].GetName ();
            }


            // 高さの指定
            public override nfloat GetRowHeight (UIPickerView pickerView, nint component)
            {
                return 20;
            }


            //幅の指定
            public override nfloat GetComponentWidth (UIPickerView pickerView, nint component)
            {
                return 200;
            }

            //値の変更
            public override void Selected (UIPickerView pickerView, nint row, nint component)
            {
                // 何もしない
            }
        }
    }
}

