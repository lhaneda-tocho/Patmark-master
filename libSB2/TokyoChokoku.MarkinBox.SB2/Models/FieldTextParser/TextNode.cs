using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class TextNode : IFieldTextNode
    {
        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Text;
            }
        }


        public string Text { get; }


        public int CharCount () {
            return Text.Length;
        }

        public TextNode (string text)
        {
            Text = text;
        }


    }
}

