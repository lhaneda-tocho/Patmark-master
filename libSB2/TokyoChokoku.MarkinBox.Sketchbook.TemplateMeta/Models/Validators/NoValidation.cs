using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public class NoValidation : MetaValidator
    {
        public static NoValidation Instance { get; } = new NoValidation ();

        private NoValidation () : base ("NullValidator")
        {
        }

        public override string GenValidationMethodName (PropertyDefinition property)
        {
            return "NoValidate";
        }



        public override string GenAdjustMethodName (PropertyDefinition property)
        {
            return "NoAdjust";
        }
    }
}

