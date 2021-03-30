using System;
namespace TokyoChokoku.Patmark.iOS
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

