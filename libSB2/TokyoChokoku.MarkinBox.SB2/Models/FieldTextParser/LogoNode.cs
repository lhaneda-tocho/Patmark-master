using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class LogoNode : IFieldTextNode
    {
        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Logo;
            }
        }


        public int    LogoIdentifier { get; }


        public LogoNode (int id)
        {
            LogoIdentifier = id;
        }

    }
}

