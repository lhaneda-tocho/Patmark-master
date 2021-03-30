using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public abstract class MetaValidator
    {
        public string Name { get; }
        public string TypeName { get; }

        public MetaValidator (string name)
        {
            TypeName = name;
            Name = name;
        }


        public abstract string GenValidationMethodName (PropertyDefinition property);
        public abstract string GenAdjustMethodName (PropertyDefinition property);
    }
}

