using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// インスタンス化不可能な空の型を表します.
    /// </summary>
    public sealed class Nil
    {
        private Nil()
        {
            throw new InvalidOperationException();
        }
    }
}

