using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoChokoku.Patmark.Common
{
    /// <summary>
    /// Float 配列作成機です。桁落ちのリスクを最小限に抑えて float 配列を生成します。
    /// </summary>
    public static class FloatArrayMaker
    {
        /// <summary>
        /// 開始値とステップ幅、生成数を指定して整数範囲から、float 配列を作成します。
        /// </summary>
        public static IEnumerable<float> CreateFloats(decimal start, decimal step, int count)
        {
            Checks.Required(step > 0, () => $"{nameof(step)} should be non 0 & positive & finite.");
            Checks.Required(count >= 0, () => "count should be 0 or positive.");

            return Enumerable.Range(0, count).Select(i => (float)(start + i * step));
        }



        /// <summary>
        /// 有理数範囲から、float 配列を作成します。
        /// NOTE: 現状は、昇順に値を生成することのみサポートします。
        /// start > endInclusive の場合は空集合と扱います。
        /// </summary>
        /// <param name="start">開始値 (decimal で指定してください)</param>
        /// <param name="endInclusive">終了値 (含める) (start < endInclusive)</param>
        /// <param name="step">刻み幅 (正の有限値)</param>
        /// <returns>指定された範囲の float 値</returns>
        /// <exception cref="ArgumentException">step が (非0の) 有限の正の値でない場合</exception>
        /// <exception cref="ArithmeticException">
        /// この処理の間に算術オーバーフローが生じ、処理が継続できなくなった場合。
        /// この例外がスローされる場合、 step 引数が極端に小さいか、 start ~ endInclusive の範囲が極端に大きい可能性があります。
        /// </exception>
        /// <remarks>
        /// (内部仕様)
        /// 1 + (endInclusive - start) / step が、Int32で表現できない範囲になると、ArithmeticException がスローされます。
        /// 
        /// この仕様は将来変わる可能性があります。
        /// </remarks>
        public static IEnumerable<float> CreateFloatsWithDecimalRange(decimal start, decimal endInclusive, decimal step = 1.0m)
        {
            Checks.Required(step > 0, () => $"{nameof(step)} should be non 0 & positive & finite.");
            if (start > endInclusive)
                return Enumerable.Empty<float>();

            int elementCount = 1 + (int)decimal.Floor(
                (endInclusive - start) / step
            );

            // とても大きい float value を int に cast すると、 Int.MAX_VALUE になる。
            // それに 1 を加えることでオーバーフローし、 Int.MIN_VALUE になる。
            // なので、 elementCount が負であるかどうかを調べることで、
            // オーバーフローしたかを検知できる。
            if (elementCount < 0)
                throw new ArithmeticException();

            return CreateFloats(start, step, elementCount);
        }

    }
}
