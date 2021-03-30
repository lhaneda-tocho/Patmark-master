using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public class MetaMakingValidator : MetaValidator
    {
        public static MetaMakingValidator Create ()
        {
            var name = ValidationCategory.Marking.Suffix ();

            return new MetaMakingValidator (name);
        }


        MetaMakingValidator (string name)
            : base (name)
        {
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

