using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public sealed class Holder <T>
    {
        public T Content { get; set; }

        public Holder (T init)
        {
            Content = init;
        }
    }
}

