using System;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    using Common;

    internal static class PMForceInfo
    {
        public const int MinValue     = 0;
        public const int MaxValue     = 9;
        public const int DefaultValue = 5;
        public static readonly string ValueRange = $"{MinValue}~{MaxValue}";

        public static bool ContainsValue(int value)
        {
            var roof    = MaxValue;
            var floor   = MinValue;
            var current = value;
            return floor <= current && current <= roof;
        }

        public static int CheckValue(int value, Func<string> errMessage)
        {
            Checks.Required(ContainsValue(value), errMessage);
            return value;
        }

        public static short ToBinary(int value, Func<string> errMessage)
        {
            CheckValue(value, errMessage);
            if (value == MinValue)
                return BinaryB;
            return (short)(value - 1);
        }

        // ==========

        const int MinBinaryA = 0;
        const int MaxBinaryA = 8;
        const int BinaryB = 100;
        public static readonly string BinaryRange = $"{MinBinaryA}~{MaxBinaryA} or {BinaryB}";


        static bool ContainsBinary(int binary)
        {
            if (binary == BinaryB)
                return true;
            var roof    = MaxBinaryA;
            var floor   = MinBinaryA;
            var current = binary;
            return floor <= current && current <= roof;
        }

        public static int ParseBinary(int binary, Func<string> errMessage)
        {
            int value;
            Checks.Required(TryParseBinary(binary, out value), errMessage);
            return value;
        }

        public static bool TryParseBinary(int binary, out int value)
        {
            if (ContainsBinary(binary))
            {
                if (binary == BinaryB)
                    value = MinValue;
                else
                    value = binary + 1;
                return true;
            }
            value = 0;
            return false;
        }
    }

    /// <summary>
    /// 打刻力を表すクラスです.
    /// </summary>
    public sealed class PMForce : AutoComparableValueType<PMForce>
    {
        /// <summary>
        /// Floor value (Inclusive).
        /// </summary>
        public static readonly PMForce MinValue = new PMForce(PMForceInfo.MinValue);

        /// <summary>
        /// Roof value (Inclusive).
        /// </summary>
        public static readonly PMForce MaxValue = new PMForce(PMForceInfo.MaxValue);

        /// <summary>
        /// Default value used by fallback.
        /// </summary>
        public static readonly PMForce Default  = new PMForce(PMForceInfo.DefaultValue);

        /// <summary>
        /// This method instantiate this class with valid value.
        /// </summary>
        /// <param name="force">valid force</param>
        /// <returns>This instance</returns>
        /// <exception cref="ArgumentException">If the argument value is invalid.</exception>
        public static PMForce Create(int force)
        {
            return new PMForce(
                PMForceInfo.CheckValue(force, () => $"打刻力は {PMForceInfo.ValueRange} の範囲を想定しています.")
            );
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid int display value.
        /// </summary>
        /// <param name="force">valid/invalid value</param>
        /// <returns>This instance if <c>force</c> is valid. Otherwise null.</returns>
        public static PMForce CreateOrNull(int force)
        {
            if (PMForceInfo.ContainsValue(force))
                return new PMForce(force);
            else
                return null;
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid int display value.
        /// </summary>
        /// <param name="force">valid/invalid value</param>
        /// <returns>This instance if <c>force</c> is valid. Otherwise default value.</returns>
        public static PMForce CreateOrDefault(int force)
        {
            if (PMForceInfo.ContainsValue(force))
                return new PMForce(force);
            else
            {
                var d = Default;
                System.Diagnostics.Debug.WriteLine($"打刻値が異常値(表示値: {force})です. 代わりに デフォルト表示値: {d} を返します.");
                return d;
            }
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid text display value.
        /// </summary>
        /// <param name="text">valid/invalid value</param>
        /// <returns>This instance if <c>text</c> is valid. Otherwise null.</returns>
        public static PMForce CreateFromDisplayTextOrNull(string text)
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
        public static PMForce CreateFromBinary(int binary)
        {
            return new PMForce(
                PMForceInfo.ParseBinary(binary, () => $"打刻力のバイナリ値は {PMForceInfo.BinaryRange} の範囲を想定しています.")
            );
        }

        /// <summary>
        /// This method instantiate this class with valid/invalid binary value.
        /// </summary>
        /// <param name="binary">binary value</param>
        /// <returns>This instance if <c>binary</c> is valid. Otherwise default value.</returns>
        public static PMForce CreateFromBinaryOrDefault(int binary)
        {
            int force;
            if (PMForceInfo.TryParseBinary(binary, out force))
                return new PMForce(force);
            else
            {
                var d = Default;
                System.Diagnostics.Debug.WriteLine($"打刻力が異常値(バイナリ値: {binary})です. 代わりに デフォルト表示値: {d} を返します.");
                return d;
            }
        }


        //static PMForce CreateFromBinaryOrNull(int binary)
        //{
        //    int force;
        //    if (PMForceInfo.TryParseBinary(binary, out force))
        //        return new PMForce(force);
        //    else
        //        return null;
        //}

        // ====

        /// <summary>
        /// Force Value
        /// </summary>
        public int Value { get; }

        private PMForce(int force)
        {
            PMForceInfo.CheckValue(force, () => $"(Factory Implementation Bug) 打刻力は {PMForceInfo.ValueRange} の範囲を想定しています.");
            Value = force;
        }

        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(Value)
                .Build();
        }

        public override int CompareTo(PMForce other)
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
        public short ToBinary() => PMForceInfo.ToBinary(Value, () => $"Detected invalid state: Value={Value}");

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
