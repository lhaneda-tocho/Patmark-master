using System;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class TextBoxCellSource : IEditBoxCellDelegate
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

        protected TextBoxCellSource (IEditBoxCommonDelegate commonDelegate)
        {
            CommonDelegate = commonDelegate;
        }

        public abstract void SetUpdatedListener (Action<IEditBoxCellDelegate> action);

        public abstract ValidationResult Validate (string newValue);
        public abstract string GetText ();
        public abstract string GetFontTitle ();
        public abstract ValidationResult ChangedText (string newText);


        public static TextBoxCellSource Create (Store<string> textStore, Store<FontMode> fontStore, IEditBoxCommonDelegate commonDelegate)
        {
            return new SimpleSource (textStore, fontStore, commonDelegate);
        }

        public static TextBoxCellSource Create (Store<string> textStore, FontMode font, IEditBoxCommonDelegate commonDelegate)
        {
            return new ConstantFontSource (textStore, font, commonDelegate);
        }





        class SimpleSource : TextBoxCellSource
        {
            ModificationListener OnModified;

            readonly Store<string> textStore;
            readonly Store<FontMode> fontStore;

            public SimpleSource (Store<string> textStore, Store<FontMode> fontStore, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
            {
                this.textStore = textStore;
                this.fontStore = fontStore;
            }

            public override void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
            {
                // 後始末
                if (OnModified != null) {
                    textStore.OnModified -= OnModified;
                    fontStore.OnModified -= OnModified;
                }
                // 変換
                OnModified = (result, sender) => {
                    action (this);
                };
                // 設定
                textStore.OnModified += OnModified;
                fontStore.OnModified += OnModified;
            }

            public override ValidationResult Validate (string newValue)
            {
                return textStore.ValidateAndGetResult (newValue)
                            .SelectCategory (ValidationCategory.Text);
            }

            public override string GetText ()
            {
                return textStore.Content;
            }

            public override string GetFontTitle ()
            {
                return fontStore.Content.GetLocalizationId ().Localize ();
            }

            public override ValidationResult ChangedText (string newText)
            {
                var result = Validate (newText);
                if (!result.HasError) {
                    textStore.ForceSetExcluding (newText, OnModified);
                    CommonDelegate.Apply ();
                }
                return result;
            }
        }

        class ConstantFontSource : TextBoxCellSource
        {
            ModificationListener OnModified;

            readonly Store<string> textStore;
            readonly FontMode font;

            public ConstantFontSource (Store<string> textStore, FontMode font, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
            {
                this.textStore = textStore;
                this.font = font;
            }

            public override ValidationResult Validate (string newValue)
            {
                return textStore.ValidateAndGetResult (newValue)
                            .SelectCategory (ValidationCategory.Text);
            }

            public override string GetText ()
            {
                return textStore.Content;
            }

            public override string GetFontTitle ()
            {
                return font.GetLocalizationId ().Localize ();
            }

            public override ValidationResult ChangedText (string newText)
            {
                var result = Validate (newText);
                if (!result.HasError)
                    textStore.ForceSetExcluding (newText, OnModified);
                CommonDelegate.Apply ();
                return result;
            }

            public override void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
            {
                // 後始末
                if (OnModified != null)
                    textStore.OnModified -= OnModified;
                // 変換
                OnModified = (result, sender) => {
                    action (this);
                };
                // 設定
                textStore.OnModified += OnModified;
            }
        }
    }
}

