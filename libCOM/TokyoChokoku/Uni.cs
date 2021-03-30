using System;
namespace TokyoChokoku
{
    /// <summary>
    /// void の代わりに利用する型です。
    /// この型の値を返すコールバックは Uni.I を返す必要があります。
    /// </summary>
    public class Uni
    {
        public static Uni I { get; } = new Uni();

        private Uni() {
            
        }
    }
}
