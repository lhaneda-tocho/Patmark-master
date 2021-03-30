using System;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class BoolToggleSwitchCellSource : ToggleSwitchCellSource
    {
        private ModificationListener OnModified = null;

        private readonly Store<bool> store;

        public BoolToggleSwitchCellSource (Store<bool> store, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
        {
            if (store == null)
                throw new NullReferenceException ();

            this.store = store;
        }

        public override bool CurrentState ()
        {
            return store.Content;
        }

        public override void ChangedState (bool state)
        {
            store.SetIfValidExcluding (state, OnModified);
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

