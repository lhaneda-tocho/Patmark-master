using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public sealed class EmptyEditBoxCommonDelegate : IEditBoxCommonDelegate
    {
        public static EmptyEditBoxCommonDelegate Instance { get; } = new EmptyEditBoxCommonDelegate ();

        private EmptyEditBoxCommonDelegate ()
        {
        }

        public void Apply ()
        {
        }

        public void Rebuild ()
        {
        }
    }
}

