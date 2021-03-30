using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public static class TextValidatorDefinitions
    {
        public static readonly ReadOnlyDictionary<FieldDefinition, MetaTextValidator> Dictionary;

        static TextValidatorDefinitions ()
        {
            var dict = new Dictionary<FieldDefinition, MetaTextValidator> ();
            dict.Add ( MetaHorizontalText.Instance, MetaTextValidator.Create (TextValidationClass.Markup));
            dict.Add (  MetaYVerticalText.Instance, MetaTextValidator.Create (TextValidationClass.Markup));
            dict.Add (  MetaXVerticalText.Instance, MetaTextValidator.Create (TextValidationClass.Markup));
            dict.Add (   MetaInnerArcText.Instance, MetaTextValidator.Create (TextValidationClass.Markup));
            dict.Add (   MetaOuterArcText.Instance, MetaTextValidator.Create (TextValidationClass.Markup));
            dict.Add (         MetaQrCode.Instance, MetaTextValidator.Create (TextValidationClass.QrCode));
            dict.Add (     MetaDataMatrix.Instance, MetaTextValidator.Create (TextValidationClass.DataMatrix));
            Dictionary = new ReadOnlyDictionary<FieldDefinition, MetaTextValidator> (dict);
        }
    }
}

