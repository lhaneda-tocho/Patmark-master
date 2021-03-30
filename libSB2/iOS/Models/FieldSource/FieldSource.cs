using System;
using System.Collections.Generic;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// フィールドを読み込むことができる要素を表します．
    /// </summary>
    public interface FieldSource
    {
        /// <summary>
        /// フィールドがどこから読み込まれるのかを返します．
        /// 人が読める形式で返すので，UIの表示に有効です．
        /// </summary>
        /// <value>From.</value>
        string From { get; }

        /// <summary>
        /// フィールドを読み取り，それを返します．読み込めなかった場合は空となります．
        /// </summary>
        /// <returns>The load.</returns>
        Maybe<IList<iOSOwner>> TryLoad ();

        /// <summary>
        /// 自動保存ファイルにデータを上書きします．
        /// </summary>
        void Autosave (iOSFieldContext context);
    }

    public static class FieldSourceExt
    {
        public static bool TryLoadTo (this FieldSource src, iOSFieldContext dest)
        {
            var maybeList = src.TryLoad ();
            if (maybeList.HasValue)
                dest.TrySubmitAll (maybeList.Value);
            return maybeList.HasValue;
        }

        /// <summary>
        /// フィールドを読み取り，その結果を iOSFieldContext で返します．
        /// </summary>
        /// <param name="src">Source.</param>
        public static Maybe<iOSFieldContext> TryLoad (this FieldSource src)
        {
            return src.TryLoad ().Select ( list => {
                var context = new iOSFieldContext ();
                context.TrySubmitAll (list);
                return context;
            });
        }

        /// <summary>
        /// コンテキストの内容を消してから新しいデータを挿入します．
        /// </summary>
        /// <param name="src">Source.</param>
        /// <param name="dest">Context.</param>
        public static bool TryLoadToAfterDeleteAll (this FieldSource src, iOSFieldContext dest)
        {
            var maybeList = src.TryLoad ();
            if (maybeList.HasValue) {
                dest.ForceDeleteAll ();
                dest.TrySubmitAll (maybeList.Value);
            }
            return maybeList.HasValue;
        }
    }

    public class FieldSourceFailureToLoadException : Exception
    {
        public FieldSourceFailureToLoadException ()
        {
        }

        public FieldSourceFailureToLoadException (string message) : base (message)
        {
        }

        public FieldSourceFailureToLoadException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public FieldSourceFailureToLoadException (string message, Exception innerException) : base (message, innerException)
        {
        }

        public FieldSourceFailureToLoadException (Exception innerException) : base ("Failure to load fields", innerException)
        {
        }
    }
}

