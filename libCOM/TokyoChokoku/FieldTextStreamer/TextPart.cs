using System;
namespace TokyoChokoku.FieldTextStreamer
{
    /// <summary>
    /// テキストのシリアル部です.
    /// </summary>
    public class TextPart
    {
        /// <summary>
        /// テキストです.
        /// </summary>
        /// <value>The value.</value>
        public string Text { get; }

        public TextPart(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
