using System;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// 打刻設定のデータベースです。テキストサイズ、打刻力、品質値の読み書きに対応します。
    ///
    /// このインタフェースを実装するクラスで、バリデーションが厳密に行われる保証はないため、
    /// 直接アクセスするのではなく、クエリクラスを使用してアクセスしてください。
    /// 
    /// </summary>
    public interface IPMMarkingParameterDB
    {
        /// <summary>
        /// 人が読める形式の DB 名
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// この DB が可変であれば true, そうでなければ false です。
        /// </summary>
        public bool IsMutable { get; }

        /// <summary>
        /// TextSizeLevel に対応する PMTextSize を取得します。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value (not null)</returns>
        PMTextSize  GetTextSize(TextSizeLevel key);

        /// <summary>
        /// ForceLevel に対応する PMForce を取得します。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value (not null)</returns>
        PMForce     GetForce(ForceLevel key);

        /// <summary>
        /// QualityLevel に対応する PMQuality を取得します。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value (not null)</returns>
        PMQuality   GetQuality(QualityLevel key);

        /// <summary>
        /// 不変オブジェクトに PMParameterDB の内容を複製して返します。
        /// </summary>
        /// <returns></returns>
        IPMMarkingParameterDB Baked();
    }

    /// <summary>
    /// IPMMarkingParameterDB の可変クラスです。
    /// このインタフェースを実装するオブジェクトは、スレッドセーフでないことが多いです。
    /// 呼び出し側で適切に同期処理を行う必要があります。
    /// </summary>
    public interface IPMMutableMarkingParameterDB : IPMMarkingParameterDB
    {
        /// <summary>
        /// テキストサイズを保存します。実装によっては、 <c>Commit</c> の実行が要求される場合があります。
        /// </summary>
        /// <param name="level">key</param>
        /// <param name="value">value</param>
        /// <exception cref="InvalidOperationException">
        /// この DB が不変の場合
        /// </exception>
        void SetTextSize(TextSizeLevel key, PMTextSize value);

        /// <summary>
        /// テキストサイズを保存します。実装によっては、 <c>Commit</c> の実行が要求される場合があります。
        /// </summary>
        /// <param name="level">key</param>
        /// <param name="value">value</param>
        /// <exception cref="InvalidOperationException">
        /// この DB が不変の場合
        /// </exception>
        void SetForce(ForceLevel key, PMForce value);

        /// <summary>
        /// テキストサイズを保存します。実装によっては、 <c>Commit</c> の実行が要求される場合があります。
        /// </summary>
        /// <param name="level">key</param>
        /// <param name="value">value</param>
        /// <exception cref="InvalidOperationException">
        /// この DB が不変の場合
        /// </exception>
        void SetQuality(QualityLevel key, PMQuality value);

        /// <summary>
        /// 引数に指定された DBの内容をコピーします。実装によっては、 <c>Commit</c> の実行が要求される場合があります。
        /// </summary>
        void Drain(IPMMarkingParameterDB src);

        /// <summary>
        /// (Option) 変更した内容を確定させます。このメソッド実行後に、このDBの内容の変更が反映されます。
        /// このメソッドを必要としない実装では、何も起こりません。
        /// </summary>
        public void Commit();
    }

    /// <summary>
    /// IPMMutableMarkingParameterDB の補助クラスです。
    /// </summary>
    public static class PMMarkingParameterDBs
    {
        /// <summary>
        /// この DB が可変DB であれば PMMutableMarkingParameterDB にキャストし、そうでなければ、例外をスローします。
        /// </summary>
        /// <returns>self</returns>
        /// <exception cref="InvalidOperationException">
        /// DB is immutable.
        /// </exception>
        public static IPMMutableMarkingParameterDB AsMutableOrExcept(this IPMMarkingParameterDB self)
        {
            if (!self.IsMutable)
                throw new InvalidOperationException("DB is immutable");
            return self as IPMMutableMarkingParameterDB ?? throw new InvalidOperationException("DB is immutable");
        }

        /// <summary>
        /// この DB が可変DB であれば PMMutableMarkingParameterDB にキャストし、そうでなければ、例外をスローします。
        /// </summary>
        /// <returns>self or null</returns>
        public static IPMMutableMarkingParameterDB AsMutableOrNull(this IPMMarkingParameterDB self)
        {
            if (!self.IsMutable)
                return null;
            return self as IPMMutableMarkingParameterDB;
        }

        /// <summary>
        /// Materializable な形式に変換します。
        /// </summary>
        /// <returns></returns>
        public static MaterializableTextSizeLevel MaterializableOf(this IPMMarkingParameterDB self, TextSizeLevel level)
        {
            return MaterializableTextSizeLevel.Create(level, (it) => self.GetTextSize(it), self.DisplayName);
        }

        /// <summary>
        /// 不変オブジェクトに PMParameterDB の内容を複製して返す、標準実装です。
        /// </summary>
        /// <returns>複製された不変オブジェクト</returns>
        public static PMImmutableMarkingParameterDB StandardImmutableCopy(this IPMMarkingParameterDB self)
        {
            return new PMImmutableMarkingParameterDB(self);
        }

        /// <summary>
        /// 可変オブジェクトに PMParameterDB の内容を書き込みます。
        /// </summary>
        public static void StandardDrain(this IPMMutableMarkingParameterDB self, IPMMarkingParameterDB src)
        {
            foreach(var it in TextSizeLevels.AllItems)
            {
                var key     = it;
                var value   = src.GetTextSize(it);
                self.SetTextSize(key, value);
            }
            foreach (var it in ForceLevels.AllItems)
            {
                var key     = it;
                var value   = src.GetForce(it);
                self.SetForce(key, value);
            }
            foreach (var it in QualityLevels.AllItems)
            {
                var key     = it;
                var value   = src.GetQuality(it);
                self.SetQuality(key, value);
            }
        }
    }
}
