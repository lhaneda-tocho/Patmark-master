using System;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class SemiConcretedUpDownTextBoxCellSource<Type> : UpDownTextBoxCellSource
    {
        private ModificationListener OnModified = null;

        protected readonly Store<Type> store;
        private readonly string unit;
        private readonly Type increment;

        public SemiConcretedUpDownTextBoxCellSource (
            Store<Type> store, string unit, Type increment, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
        {
            if (store == null)
                throw new NullReferenceException ();

            this.store = store;
            this.unit = unit;
            this.increment = increment;
        }

        public abstract Type Add (Type left, Type right);
        public abstract Type Sub (Type left, Type right);
        public abstract bool TryParse (string raw, out Type result);

        public override bool Validate (string newText)
        {
            Type test;

            if (!TryParse (newText, out test))
                return false;
            else
                return store.IsValid (test);
        }

        public override bool ChangedText (string newText)
        {
            Type result;
            if (TryParse (newText, out result) &&
                store.IsValid (result)       ) {

                store.ForceSetExcluding (result, OnModified);
                CommonDelegate.Apply ();
                return true;
            }
            return false;
        }

        public override string GetUnit ()
        {
            return unit;
        }

        public override void TouchedIncrease (string currentText)
        {
            var incremented = Add (store.Content, increment);
            store.ForceSetExcluding ( incremented, OnModified );
            CommonDelegate.Apply ();
        }

        public override void TouchedDecrease (string currentText)
        {
            var decremented = Sub (store.Content, increment);
            store.ForceSetExcluding (decremented, OnModified);
            CommonDelegate.Apply ();
        }


        public override void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
        {
            // 後始末
            if (OnModified != null) 
                store.OnModified -= OnModified;
            // 変換
            OnModified = (result, sender) => {
                action (this);
            };
            // 設定
            store.OnModified += OnModified;
        }
    }
}

