using System;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    using Common;

    /// <summary>
    /// 品質値を表すクラスです.
    /// </summary>
    public sealed class PMQuality: AutoComparableValueType<PMQuality>
    {
        /// <summary>
        /// Floor value (Inclusive).
        /// </summary>
        public static readonly PMQuality MinValue = new PMQuality(1);

        /// <summary>
        /// Roof value (Inclusive).
        /// </summary>
        public static readonly PMQuality MaxValue = new PMQuality(9);

        /// <summary>
        /// Default value used by fallback.
        /// </summary>
        public static PMQuality Default { get; } = MaxValue;

        /// <summary>
        /// This method instantiate this class with valid value.
        /// </summary>
        /// <param name="quality">valid force</param>
        /// <returns>This instance</returns>
        /// <exception cref="ArgumentException">If the argument value is invalid.</exception>
        public static PMQuality Create(int quality) => new PMQuality(quality);

        /// <summary>
        /// This method instantiate this class with valid/invalid int display value.
        /// </summary>
        /// <param name="quality">valid/invalid value</param>
        /// <returns>This instance if <c>quality</c> is valid. Otherwise null.</returns>
        public static PMQuality CreateOrNull(int quality)
        {
            if (CheckValue(quality))
                return new PMQuality(quality);
            return null;
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid int display value.
        /// </summary>
        /// <param name="force">valid/invalid value</param>
        /// <returns>This instance if <c>force</c> is valid. Otherwise default value.</returns>
        public static PMQuality CreateOrDefault(int quality)
        {
            var q = CreateOrNull(quality);
            if (ReferenceEquals(q, null))
            {
                var d = Default;
                System.Diagnostics.Debug.WriteLine($"品質値が異常値(表示値: {quality})です. 代わりに デフォルト表示値: {d} を返します.");
                return d;
            }
            return q;
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid text display value.
        /// </summary>
        /// <param name="text">valid/invalid value</param>
        /// <returns>This instance if <c>text</c> is valid. Otherwise null.</returns>
        public static PMQuality CreateFromDisplayTextOrNull(string text)
        {
            int value;
            // 1. Int32 に変換する. 失敗したら null を返す。
            if (!int.TryParse(text, out value))
                return null;
            return CreateOrNull(value);
        }

        /// <summary>
        /// This method instantiate this class with valid binary value.
        /// </summary>
        /// <param name="binary">binary value</param>
        /// <returns>this instance</returns>
        /// <exception cref="ArgumentException">If the argument value is invalid.</exception>
        public static PMQuality CreateFromBinary(int binary) => new PMQuality(binary+1);

        /// <summary>
        /// This method instantiate this class with valid/invalid binary value.
        /// </summary>
        /// <param name="binary">binary value</param>
        /// <returns>this instance if <c>binary</c> is valid. Otherwise null.</returns>
        static PMQuality CreateFromBinaryOrNull(int binary)
        {
            int quality = binary+1;
            return CreateOrNull(quality);
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid binary value.
        /// </summary>
        /// <param name="binary">binary value</param>
        /// <returns>This instance if <c>binary</c> is valid. Otherwise default value.</returns>
        public static PMQuality CreateFromBinaryOrDefault(int binary)
        {
            var q = CreateFromBinaryOrNull(binary);
            if(ReferenceEquals(q, null))
            {
                var d = Default;
                System.Diagnostics.Debug.WriteLine($"品質値が異常値(バイナリ値: {binary})です. 代わりに デフォルト表示値: {d} を返します.");
                return d;
            }
            return q;
        }

        static bool CheckValue (int quality) => (1 <= quality && quality <= 9);

        // ====

        /// <summary>
        /// Quality Value
        /// </summary>
        public int Value { get; }

        private PMQuality(int quality)
        {
            Checks.Required(CheckValue(quality), "打刻精度は 1~9 の範囲を想定しています.");
            Value = quality;
        }

        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(Value)
                .Build();
        }

        public override int CompareTo(PMQuality other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            return Value - other.Value;
        }

        /// <summary>
        /// This method converts this instance to the display value.
        /// </summary>
        /// <returns>int display value.</returns>
        public int ToInt() => Value;

        /// <summary>
        /// This method converts this instance to the binary value.
        /// </summary>
        /// <returns>binary value.</returns>
        public short ToBinary() => (short) (Value - 1);

        /// <summary>
        /// This method converts this instance to the display value.
        /// </summary>
        /// <returns>text display value.</returns>
        public override string ToString()
        {
            return ToInt().ToString();
        }
    }

    
}
