using System;

namespace TokyoChokoku.FieldTextKit
{
    public class FTTextNode : IFieldTextNode
    {
        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Text;
            }
        }


        public string Text { get; }


        public int CharCount  {
            get => Text.Length;
        }

        public FTTextNode (string text)
        {
            Text = text;
        }


    }
}

