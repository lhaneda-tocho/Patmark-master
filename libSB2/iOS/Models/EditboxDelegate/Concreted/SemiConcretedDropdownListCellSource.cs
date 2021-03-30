using System;
using Foundation;
using UIKit;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class SemiConcretedDropdownListCellSource<Type> : DropdownListCellSource
    {
        ModificationListener OnModified;

        protected readonly Store<Type> store;
        protected readonly Type []     items;
        MyPickerViewModel<Type> model;


        public abstract nint   GetStartRow ();
        public abstract string AsString (Type value);

        protected SemiConcretedDropdownListCellSource (Store<Type> store, Type [] items, IEditBoxCommonDelegate commonDelegate)
            : base (commonDelegate)
        {
            this.store = store;
            this.items = items;
            model = new MyPickerViewModel <Type> (this, store, items);
        }


        // 値の確定
        public override void Commit (PickerViewCell cell, UIPickerView pickerView)
        {
            model.Selected (pickerView, pickerView.SelectedRowInComponent (0), 0);
        }


        public override UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView pickerView)
        {
            return model;
        }


        public override void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
        {
            // 後始末
            if (OnModified != null)
                model.store.OnModified -= OnModified;
            // 変換
            OnModified = (result, sender) => {
                action (this);
            };
            // 設定
            model.store.OnModified += OnModified;
        }




        public override string GetCurrentValue ()
        {
            return AsString (model.store.Content);
        }

        public override void InitializePickerView (PickerViewCell cell, UIPickerView pickerView)
        {
            pickerView.Select (GetStartRow (), 0, false);
        }

        public override bool IsSingleOrEmpty ()
        {
            return items.Length <= 1;
        }


        sealed class MyPickerViewModel<ContentType> : UIPickerViewModel
        {
            readonly SemiConcretedDropdownListCellSource<ContentType> source;

            public Store<ContentType> store;
            public ContentType [] items;

            public MyPickerViewModel (
                SemiConcretedDropdownListCellSource<ContentType> source, Store<ContentType> store, ContentType [] items)
            {
                if (store == null || items == null)
                    throw new NullReferenceException ();

                this.source = source;
                this.store = store;
                this.items = items;
            }

            // カラム数
            public override nint GetComponentCount (UIPickerView pickerView)
            {
                return 1;
            }


            // 行数
            public override nint GetRowsInComponent (UIPickerView pickerView, nint component)
            {
                return items.Length;
            }


            // 文字列取得
            public override string GetTitle (UIPickerView pickerView, nint row, nint component)
            {
                return source.AsString (items [row]);
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
                store.SetIfValid (items [row]);
                source.CommonDelegate.Apply ();
            }
        }

    }
}

