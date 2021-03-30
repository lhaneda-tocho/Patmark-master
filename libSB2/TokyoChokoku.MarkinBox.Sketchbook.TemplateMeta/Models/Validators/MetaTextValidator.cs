using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public class MetaTextValidator : MetaValidator
    {
        public static MetaTextValidator Create (TextValidationClass textValidationClass)
        {
            var name = textValidationClass.GetName ();

            return new MetaTextValidator (name);
        }


        MetaTextValidator (string name)
            : base (name)
        {
        }


        public override string GenValidationMethodName (PropertyDefinition property)
        {
            if (property.StoreType == MetaStores.Text)
                return "ValidateText";
            else
                throw new ArgumentOutOfRangeException ();
        }

        public override string GenAdjustMethodName (PropertyDefinition property)
        {
                if (property.StoreType == MetaStores.Text)
                    return "AdjustText";
                else
                    throw new ArgumentOutOfRangeException ();
        }
    }
}

