using System;

namespace TokyoChokoku.FieldTextKit
{
    public class FTLogoNode : IFieldTextNode
    {
        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Logo;
            }
        }


        public int    LogoIdentifier { get; }


        public FTLogoNode (int id)
        {
            LogoIdentifier = id;
        }

    }
}

