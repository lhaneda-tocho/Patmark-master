using System;
using TokyoChokoku.SerialModule.Counter;
namespace TokyoChokoku.FieldTextStreamer
{
    /// <summary>
    /// テキストのシリアル部です.
    /// Value, ID には、文法上表現可能な範囲であらゆる値が設定可能です。
    /// SerialPart オブジェクトを受け取って処理する場合は、値が有効な値であるかバリデーションする必要があります。
    /// </summary>
    public class SerialPart
    {
        /// <summary>
        /// テキストに書き込まれている現在のシリアル値です
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; }

        /// <summary>
        /// シリアルID です．
        /// </summary>
        /// <value>The identifier.</value>
        public int ID    { get; }

        public SerialPart(int id, int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException($"{nameof(value)} is must be 0 or positive.");
            if (value > 9999)
                throw new ArgumentOutOfRangeException($"{nameof(value)} is must be less than 10000.");
            if (id < 0)
                throw new ArgumentOutOfRangeException($"{nameof(id)} is must be 0 or positive.");
            if (id > 9)
                throw new ArgumentOutOfRangeException($"{nameof(id)} is must be less than 10");
            ID    = id;
            Value = value;
        }

        public override string ToString()
        {
            return $"@S[{Value.ToString("D4")}-{ID}]";
        }

        public string ToString(SCSetting setting)
        {
            var format = setting.Format;
            var digits = DecimalDigits((int)setting.Range.MaxValue);
            var value = format.Match(
                fillZero: it => 
                    Value.ToString($"D{digits}"),
                rightJustify: it =>
                    Value.ToString(),
                leftJustify: it =>
                    Value.ToString()

            );
            return $"@S[{value}-{ID}]";
        }

        static int DecimalDigits(int value)
        {
            int rem = value;
            int i;
            for (i = 0; rem != 0; ++i)
                rem = rem / 10;
            return Math.Max(i, 1);
        }

        public SerialPart Copy(int? id = null, int? value = null)
        {
            return new SerialPart(
                id ?? ID, value ?? Value
            );
        }
    }
}
