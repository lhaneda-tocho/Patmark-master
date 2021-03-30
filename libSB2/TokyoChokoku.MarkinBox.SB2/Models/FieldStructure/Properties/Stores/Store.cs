using System;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores
{

    public class Store<Type>
    {
        /// <summary>
        /// Occurs when on failure to be modified store.
        /// </summary>
        public event ModificationListener OnFailure;

        /// <summary>
        /// Occurs when on modified store.
        /// </summary>
        public event ModificationListener OnModified;



        /// <summary>
        /// Get the content.
        /// 
        /// When on set the content new valid value, 
        /// 	assigns new value and fires OnModified event.
        /// 
        /// otherwize keeps the content old value and fires OnFailure event.
        /// 	
        /// </summary>
        /// <value>The content.</value>
        public Type Content
        {
            get { return GetContent(); }
            set { SetIfValid(value); }
        }

        public readonly ValidatorDelegate<Type> Validator;
        public readonly Getter<Type> GetContent;
        public readonly Setter<Type> SetContent;


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores.Store`1"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        /// <param name="Getter">Getter.</param>
        /// <param name="Setter">Setter.</param>
        public Store(
            ValidatorDelegate<Type> validator,
            Getter<Type> Getter,
            Setter<Type> Setter)
        {
            this.Validator = validator;
            GetContent = Getter;
            SetContent = Setter;
        }

        public Store(
            ClosedValidator validator,
            Getter<Type> Getter,
            Setter<Type> Setter)
        {
            this.Validator = (value) => { return validator.Validate(); };
            GetContent = Getter;
            SetContent = Setter;
        }

        /// <summary>
        /// 指定された値を適用する結果，妥当な状態となる場合に変更を受け入れます．
        /// 変更を受け入れた場合は OnModified, 受け入れなかった場合は OnFailure イベントを呼び出します．
        /// 
        /// anExcluded にイベントリスナを指定することで，そのリスナだけ呼ばないようにします．
        /// 
        /// </summary>
        /// <returns>The if valid excluding.</returns>
        /// <param name="value">Value.</param>
        /// <param name="anExcluded">An excluded.</param>
        public void SetIfValidExcluding(Type value, ModificationListener anExcluded)
        {
            var result = Validator(value);

            if (!result.HasError)
                SetContent(value);

            if (result.HasError)
                result.FireEvent(OnFailure.Exclude(anExcluded), this);
            else
                result.FireEvent(OnModified.Exclude(anExcluded), this);
        }

        /// <summary>
        /// 指定された値を適用する結果，妥当な状態となる場合に変更を受け入れます．
        /// 変更を受け入れた場合は OnModified, 受け入れなかった場合は OnFailure イベントを呼び出します．
        /// 
        /// </summary>
        /// <returns>The if valid.</returns>
        /// <param name="value">Value.</param>
        public void SetIfValid(Type value)
        {
            var result = Validator(value);

            if (!result.HasError)
                SetContent(value);

            result.FireEventIf(result.HasError, OnFailure, this);
            result.FireEventIf(!result.HasError, OnModified, this);
        }

        /// <summary>
        /// 指定された値に強制的に設定します．Validate メソッドを呼び出さない分高速ですが，
        /// オブジェクトが不整合な状態になりえます．
        /// 
        /// 必ず変更を受け入れるため， OnModified イベントを呼び出します．
        /// 
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="value">Value.</param>
        public void ForceSet(Type value)
        {
            SetContent(value);
            var result = ValidationResult.Empty;
            result.FireEvent(OnModified, this);
        }


        /// <summary>
        /// 指定された値に強制的に設定します．Validate メソッドを呼び出さない分高速ですが，
        /// オブジェクトが不整合な状態になりえます．
        /// 
        /// 必ず変更を受け入れるため， OnModified イベントを呼び出します．
        /// 
        /// anExcluded にイベントリスナを指定することで，そのリスナだけ呼ばないようにします．
        /// 
        /// </summary>
        /// <returns>The set excluding.</returns>
        /// <param name="value">Value.</param>
        /// <param name="anExcluded">An excluded.</param>
        public void ForceSetExcluding(Type value, ModificationListener anExcluded)
        {
            SetContent(value);
            var result = ValidationResult.Empty;
            result.FireEvent(OnModified.Exclude(anExcluded), this);
        }


        public bool IsValid(Type value)
        {
            var result = Validator(value);
            return !result.HasError;
        }



        public ValidationResult ValidateAndGetResult(Type value)
        {
            var old = GetContent();
            SetContent(value);
            var result = Validator(value);
            SetContent(old);
            return result;
        }
    }
}

