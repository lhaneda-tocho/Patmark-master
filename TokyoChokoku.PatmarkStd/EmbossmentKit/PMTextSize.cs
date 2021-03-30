using System;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.TextData;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// This is a value class represents a text height size [mm] for Patmark.
    /// This value resolution is 0.5.
    ///
    /// Valid Value Example) 0.5, 1.0, 1.5, 2.0 ...
    ///
    /// If this is instantiate with the value which above condition isn't satisfied,
    /// the value is rounded to a nearest value.
    /// </summary>
    public sealed class PMTextSize: AutoComparableValueType<PMTextSize>
    {
        private static string OutOfRangeErrorMessage => string.Format(
            "文字サイズ {0} < x <= {1} の範囲の有理数 x をサポートします.",
            MinValueExclusive, MaxValue
        );

        /// <summary>
        /// Floor Value (Exclusive).
        /// </summary>
        public static readonly PMTextSize MinValueExclusive
            = new PMTextSize(0.0m, ignoreCheck: true);

        /// <summary>
        /// Roof Value (Inclusive).
        /// </summary>
        public static readonly PMTextSize MaxValue
            = new PMTextSize(20.0m, ignoreCheck: true);

        /// <summary>
        /// Default value used for fallback.
        /// </summary>
        public static PMTextSize Default { get; }
            = new PMTextSize(5.0m, ignoreCheck: true);

        /// <summary>
        /// This function rounds <c>value</c> to a nearest valid value.
        /// </summary>
        /// <returns>rounded value</returns>
        /// <exception cref="ArithmeticException">if the value is too large or too small or NaN.</exception>
        public static decimal Round(decimal value)
        {
            if (value > decimal.MaxValue / 2)
                throw new ArithmeticException($"{value} is too large");
            // 0 = 0.0
            // 1 = 0.5
            // となるような index 次元に直し、四捨五入する.
            decimal indexcoord = decimal.Round(value * 2m);
            // 四捨五入したら戻す。
            return indexcoord / 2m;
            
        }

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// </summary>
        /// <param name="textsize">
        /// the text size in valid range.
        /// </param>
        /// <returns>
        /// A new instance belonging this class.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the specified <c>value</c> is out of valid range and the <c>igonreCheck</c> is false.
        /// </exception>
        public static PMTextSize Create(decimal textsize)
            => new PMTextSize(textsize);

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// Or if the argument is invalid, the method returns null.
        /// </summary>
        /// <param name="textsize">the valid/invalid text size.</param>
        /// <returns>
        /// A new instance belonging this class if the argument is valid. Otherwise, null.
        /// </returns>
        public static PMTextSize CreateOrNull(decimal textsize)
        {
            if (CheckValue(textsize))
                return new PMTextSize(textsize, ignoreCheck: true);
            return null;
        }

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// Or if the argument is invalid, the method fallbacks that it returns <c>Default</c> value.
        /// </summary>
        /// <param name="textsize">the valid/invalid text size.</param>
        /// <returns>
        /// A new instance belonging this class if the argument is valid. Otherwise, <c>Default</c>.
        /// </returns>
        public static PMTextSize CreateOrDefault(decimal textsize)
        {
            var q = CreateOrNull(textsize);
            if (ReferenceEquals(q, null))
            {
                System.Diagnostics.Debug.WriteLine($"テキストサイズが異常値(表示値: {textsize})です. 代わりに デフォルト表示値: {Default} を返します.");
                return Default;
            }
            return q;
        }

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// </summary>
        /// <param name="textsize">
        /// the text size in valid range.
        /// </param>
        /// <returns>
        /// A new instance belonging this class.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the specified <c>value</c> is out of valid range and the <c>igonreCheck</c> is false.
        /// </exception>
        public static PMTextSize CreateWithFloat(float textsize)
        {
            Checks.Required(CheckValue(textsize), OutOfRangeErrorMessage);
            return new PMTextSize((decimal)textsize, ignoreCheck: true);
        }

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// Or if the argument is invalid, the method returns null.
        /// </summary>
        /// <param name="textsize">the valid/invalid text size.</param>
        /// <returns>
        /// A new instance belonging this class if the argument is valid. Otherwise, null.
        /// </returns>
        public static PMTextSize CreateOrNullWithFloat(float textsize)
        {
            if (CheckValue(textsize))
                return new PMTextSize((decimal)textsize, ignoreCheck: true);
            return null;
        }

        /// <summary>
        /// This method instantiates this class with valid text size.
        /// Or if the argument is invalid, the method fallbacks that it returns <c>Default</c> value.
        /// </summary>
        /// <param name="textsize">the valid/invalid text size.</param>
        /// <returns>
        /// A new instance belonging this class if the argument is valid. Otherwise, <c>Default</c>.
        /// </returns>
        public static PMTextSize CreateOrDefaultWithFloat(float textsize)
        {
            var q = CreateOrNullWithFloat(textsize);
            if (ReferenceEquals(q, null))
            {
                System.Diagnostics.Debug.WriteLine($"テキストサイズが異常値(表示値: {textsize})です. 代わりに デフォルト表示値: {Default} を返します.");
                return Default;
            }
            return q;
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid text display value.
        /// </summary>
        /// <param name="text">valid/invalid value</param>
        /// <returns>This instance if <c>text</c> is valid. Otherwise null.</returns>
        public static PMTextSize CreateFromDisplayTextOrNull(string text)
        {
            decimal value;
            // 1. decimal に変換する. 失敗したら null を返す。
            if (!decimal.TryParse(text, out value))
                return null;
            return CreateOrNull(value);
        }

        /// <summary>
        /// This method checks that the <c>textsize</c> value is in valid range.
        /// </summary>
        /// <param name="textsize">Valid/Invalid Text Size</param>
        /// <returns>
        /// true if the value is in valid range. otherwize, false.
        /// </returns>
        /// <remarks>
        /// When this class isn't initialized, this method will cause undefined behavior.
        /// </remarks>
        private static bool CheckValue(float textsize) =>
            float.IsFinite(textsize) && CheckValue((decimal)textsize);


        /// <summary>
        /// This method checks that the <c>textsize</c> value is in valid range.
        /// </summary>
        /// <param name="textsize">Valid/Invalid Text Size</param>
        /// <returns>
        /// true if the value is in valid range. otherwize, false.
        /// </returns>
        /// <remarks>
        /// When this class isn't initialized, this method will cause undefined behavior.
        /// </remarks>
        private static bool CheckValue(decimal textsize) =>
            MinValueExclusive.Value < textsize && textsize <= MaxValue.Value;



        // ====

        /// <summary>
        /// Text Size Value [mm]
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Constructor with Text Size [mm]
        /// </summary>
        /// <param name="value">Text Size</param>
        /// <param name="ignoreCheck">If this is true, argument validation is ignored.</param>
        /// <remarks>
        /// The "ignoreCheck" parameter should be specified true while initializing this class.
        /// Otherwise, the program will crash during the initialization.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// If the specified <c>value</c> is out of valid range and the <c>igonreCheck</c> is false.
        /// </exception>
        private PMTextSize(decimal value, bool ignoreCheck=false)
        {
            if(!ignoreCheck)
                Checks.Required(CheckValue(value), OutOfRangeErrorMessage);
            Value = Round(value);
        }

        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(Value)
                .Build();
        }

        public override int CompareTo(PMTextSize other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            if (Value > other.Value)
                return 1;
            else if (Value == other.Value)
                return 0;
            return -1;
        }

        /// <summary>
        /// Convert to EmbossmentTextSize
        /// </summary>
        /// <returns></returns>
        public EmbossmentTextSize ToEmbossmentTextSize() => EmbossmentTextSize.InitWithHeightmm(ToFloat());

        /// <summary>
        /// Convert to ITextSize.
        /// </summary>
        /// <returns></returns>
        public ITextSize ToPresentationTextSize() => TextSize.OfHeightmm(ToFloat());

        /// <summary>
        /// This methods convert this instance to float display value.
        /// </summary>
        /// <returns></returns>
        public float ToFloat() => (float) Value;

        /// <summary>
        /// This methods convert this instance to decimal display value.
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal() => Value;

        /// <summary>
        /// This methods convert this instance to text display value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToFloat().ToString("0.0");
        }
    }
}
