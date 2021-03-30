using System;
namespace TokyoChokoku.Patmark.Common
{
    /// <summary>
    /// 処理結果を説明するオブジェクトです。成功状態と失敗状態があります。
    /// このインターフェースを実装するオブジェクトは 値型であり、かつ、不変である必要があります。
    /// このオブジェクトは、 <c>IUnstable</c> の等値比較に使用します。
    ///
    /// IUnstable　にて, Where 演算を使用する場合はさらに以下の制約を満たす必要があります。
    ///
    /// 1. デフォルトコンストラクタを持つこと  (引数のないコンストラクタのこと)
    /// 2. デフォルトコンストラクタで失敗オブジェクトを取得できること。つまり、 <c>(new T()).IsSuccess == false</c> であること。 
    ///
    /// 2 の制約を満たさない場合、  <c>Where</c>にて例外がスローされます。
    /// </summary>
    public interface IUnstableDescription
    {
        /// <summary>
        /// 発生した例外. 該当する例外がない場合か、エラーが発生していない場合は <c>null</c> となります。
        /// また、処理成功時は必ず  `null` が設定されます。
        /// このプロパティは等値比較に使用されません。
        /// </summary>
        public Exception InnerException { get; }

        /// <summary>
        /// 成功したことを表す場合は true, そうでなければ false です。
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// 処理結果のメッセージです。  ToString と同じ内容となります。
        /// <c>null</c>は許可されません。
        /// </summary>
        public string Message { get; }
    }

    /// <summary>
    /// 不安定な処理の返り値を表します。不変オブジェクトです。
    /// Equals で等値比較ができます。
    /// </summary>
    /// <typeparam name="DescriptionType">エラーを説明する値型</typeparam>
    public interface IUnstable<out DescriptionType> where DescriptionType : class, IUnstableDescription
    {
        /// <summary>
        /// 結果が成功であれば <c>true</c>、失敗であれば <c>false</c> を返します。
        /// このプロパティは等値比較に使用されません。
        /// </summary>
        public bool IsSuccess => Description.IsSuccess;

        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// 発生した例外. 該当する例外がない場合か、エラーが発生していない場合は <c>null</c> となります。
        /// このプロパティは等値比較に使用されません。
        /// </summary>
        public Exception InnerException => Description.InnerException;

        /// <summary>
        /// 処理結果の内容を説明するオブジェクトです。任意の型を使用できます。
        /// 失敗時は、それを説明するオブジェクトが、成功したらそれを説明するオブジェクトが代入されます。
        /// このプロパティは等値比較に使用されます。
        /// </summary>
        public IUnstableDescription Description => SuccessDescription ?? ErrorDescription;

        /// <summary>
        /// 処理に成功した場合に、その説明オブジェクトを取得できます。
        /// 失敗した場合は null となります。
        /// </summary>
        public DescriptionType SuccessDescription { get; }

        /// <summary>
        /// エラーの時にだけ取得できる説明オブジェクトです。
        /// 成功した場合は null となります。 
        /// </summary>
        public IUnstableDescription ErrorDescription { get; }

        /// <summary>
        /// <c>IsSuccess == true</c> であれば、<c>mapperExpression</c>の処理を実行してその結果を返します。
        /// 
        /// map operator
        /// </summary>
        /// <typeparam name="TResult">Destination Type</typeparam>
        /// <param name="mapperExpression">mapper</param>
        /// <returns>mapped object</returns>
        public IUnstable<TResult> Select<TResult>(Func<DescriptionType, TResult> mapperExpression) where TResult: class, IUnstableDescription;

        /// <summary>
        /// <c>IsSuccess == true</c> であれば、<c>mapperExpression</c>の処理を実行してその結果を返します。
        /// 
        /// flatMap operator
        /// </summary>
        /// <typeparam name="TResult">Destination Type</typeparam>
        /// <param name="mapperExpression">mapper</param>
        /// <returns>flatMapped object</returns>
        public IUnstable<TResult> SelectMany<TResult>(Func<DescriptionType, IUnstable<TResult>> mapperExpression) where TResult : class, IUnstableDescription;
    }

    public static class IUnstableResultExt
    {
        /// <summary>
        /// Unstable でラップします。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IUnstable<TSource> ToReturnable<TSource>(this TSource self) where TSource : class, IUnstableDescription
        {
            return new Unstable<TSource>(self, null);
        }
    }

    public static class IUnstableExt
    {
        /// <summary>
        /// 引数の <c>predicate</c> が false を返す場合は、エラーオブジェクトを返します。
        /// 型引数に指定された型は、 IUnstableDescription インタフェースで示されるデフォルトコンストラクタを実装する必要があります。 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IUnstable<TSource> Where<TSource>(this IUnstable<TSource> self, Func<TSource, bool> predicate) where TSource : class, IUnstableDescription, new()
        {
            if (self.IsSuccess)
            {
                if (predicate(self.SuccessDescription))
                {
                    return self;
                }
                    
                else
                {
                    return new Unstable<TSource>(null, new TSource());
                }
            }
            else
            {
                return self;
            }
        }

        public static TResult  Aggregate<TSource, TResult>(
            this IUnstable<TSource>             self,
            Func<IUnstableDescription, TResult> onFailure,
            Func<TSource,              TResult> onSuccess
        ) where TSource : class, IUnstableDescription, new()
        {
            if (self.IsSuccess)
            {
                return onSuccess(self.SuccessDescription);
            }
            else
            {
                return onFailure(self.ErrorDescription);
            }
        }
    }

    /// <summary>
    /// <c>IUnstable</c> の実体となります。
    /// </summary>
    /// <typeparam name="DescriptionType">エラーを説明する値型</typeparam>
    public class Unstable<DescriptionType> : AutoValueType<Unstable<DescriptionType>>, IUnstable<DescriptionType> where DescriptionType : class, IUnstableDescription
    {
        public DescriptionType SuccessDescription { get; }

        public IUnstableDescription ErrorDescription { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="description">説明オブジェクト. 成功失敗を問わない.</param>
        /// <param name="failure">失敗オブジェクト。成功のものを入れた場合は例外がスローされる。 </param>
        /// <exception cref="ArgumentException">description, failure の両方が <c>null</c>でなかった場合。もしくは、failure に指定されたオブジェクトが IsSuccess==true を満たす場合</exception>
        /// <exception cref="ArgumentNullException">引数の両方が null の場合</exception>
        public Unstable(DescriptionType description, IUnstableDescription failure)
        {
            var isDescriptionNull = ReferenceEquals(description, null);
            var isFailNull        = ReferenceEquals(failure, null);
            if (isDescriptionNull && isFailNull)
                throw new ArgumentNullException($"{nameof(description)} & {nameof(failure)}");
            Checks.Required(
                ( isDescriptionNull && !isFailNull) ||
                (!isDescriptionNull &&  isFailNull)
            );
            Checks.Required(
                isFailNull || !failure.IsSuccess
            );
            if(isDescriptionNull)
            {
                SuccessDescription = null;
                ErrorDescription = failure;
            } else if(description.IsSuccess)
            {
                SuccessDescription = description;
                ErrorDescription = null;
            } else
            {
                SuccessDescription = null;
                ErrorDescription = description;
            }
        }

        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(SuccessDescription ?? ErrorDescription)
                .Build();
        }

        public IUnstable<TResult> Select<TResult>(Func<DescriptionType, TResult> mapperExpression) where TResult : class, IUnstableDescription
        {
            var self = (IUnstable<DescriptionType>)this;
            if (self.IsSuccess)
            {
                return new Unstable<TResult>(mapperExpression(SuccessDescription), null);
            } else
            {
                return new Unstable<TResult>(null, self.ErrorDescription);
            }
        }

        public IUnstable<TResult> SelectMany<TResult>(Func<DescriptionType, IUnstable<TResult>> mapperExpression) where TResult : class, IUnstableDescription
        {
            var self = (IUnstable<DescriptionType>)this;
            if (self.IsSuccess)
            {
                return mapperExpression(SuccessDescription);
            }
            else
            {
                return new Unstable<TResult>(null, self.ErrorDescription);
            }
        }
    }


}
