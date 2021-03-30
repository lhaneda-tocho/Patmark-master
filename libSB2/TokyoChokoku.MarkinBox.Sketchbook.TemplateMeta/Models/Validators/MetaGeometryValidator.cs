using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public class MetaGeometryValidator : MetaValidator
    {
        public ReadOnlyCollection<PropertyDefinition> propertyDefinitions;



        public static MetaGeometryValidator Create (FieldDefinition field)
        {
            return Create (field.Name, field.Parameter.PropertyDefinitions);
        }

        static MetaGeometryValidator Create (string fieldName, ReadOnlyCollection<PropertyDefinition> properties)
        {
            var name = fieldName + ValidationCategory.Geometry.Suffix ();

            properties = properties.Where ((arg) => {
                return arg.Categories.OfGeometry;
            }).ToList ().AsReadOnly ();

            return new MetaGeometryValidator (name, properties);
        }





        private MetaGeometryValidator (string name, ReadOnlyCollection<PropertyDefinition> propertyDefinitions)
            : base (name)
        {
            this.propertyDefinitions = propertyDefinitions;
        }

        public override string GenValidationMethodName (PropertyDefinition property)
        {
            return "Validate" + property.Name;
        }

        public override string GenAdjustMethodName (PropertyDefinition property)
        {
            return "Adjust" + property.Name;
        }
    }
}

