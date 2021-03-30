using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// 空の方を表します. Nil とは異なり，<c>I</c> プロパティでインスタンスを取得できます.
    /// </summary>
    public sealed class Void
    {
        /** インスタンスを取得します. */
        public static readonly Void I = new Void();
        static readonly bool Initialized = true;

        private Void()
        {
            if (Initialized)
                throw new InvalidOperationException();
        }

        public override string ToString()
        {
            return "Void";
        }
    }
}
