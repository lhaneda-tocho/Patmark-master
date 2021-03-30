using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public static class GeometryValidatorDefinitions
    {
        public static readonly MetaGeometryValidator HorizontalText = MetaGeometryValidator.Create (MetaHorizontalText.Instance);
        public static readonly MetaGeometryValidator YVerticalText  = MetaGeometryValidator.Create (MetaYVerticalText.Instance);
        public static readonly MetaGeometryValidator XVerticalText  = MetaGeometryValidator.Create (MetaXVerticalText.Instance);
        public static readonly MetaGeometryValidator InnerArcText   = MetaGeometryValidator.Create (MetaInnerArcText.Instance);
        public static readonly MetaGeometryValidator OuterArcText   = MetaGeometryValidator.Create (MetaOuterArcText.Instance);
        public static readonly MetaGeometryValidator QrCode         = MetaGeometryValidator.Create (MetaQrCode.Instance);
        public static readonly MetaGeometryValidator DataMatrix     = MetaGeometryValidator.Create (MetaDataMatrix.Instance);
        public static readonly MetaGeometryValidator Rectangle      = MetaGeometryValidator.Create (MetaRectangle.Instance);
        public static readonly MetaGeometryValidator Triangle       = MetaGeometryValidator.Create (MetaTriangle.Instance);
        public static readonly MetaGeometryValidator Circle         = MetaGeometryValidator.Create (MetaCircle.Instance);
        public static readonly MetaGeometryValidator Line           = MetaGeometryValidator.Create (MetaLine.Instance);
        public static readonly MetaGeometryValidator Bypass         = MetaGeometryValidator.Create (MetaBypass.Instance);
        public static readonly MetaGeometryValidator Ellipse        = MetaGeometryValidator.Create (MetaEllipse.Instance);


        public static readonly ReadOnlyDictionary<FieldDefinition, MetaGeometryValidator> Dictionary;

        static GeometryValidatorDefinitions ()
        {
            var dict = new Dictionary<FieldDefinition, MetaGeometryValidator> ();
            dict.Add (MetaHorizontalText.Instance, HorizontalText);
            dict.Add (MetaYVerticalText.Instance, YVerticalText);
            dict.Add (MetaXVerticalText.Instance, XVerticalText);
            dict.Add (MetaInnerArcText.Instance, InnerArcText);
            dict.Add (MetaOuterArcText.Instance, OuterArcText);
            dict.Add (MetaQrCode.Instance, QrCode);
            dict.Add (MetaDataMatrix.Instance, DataMatrix);
            dict.Add (MetaRectangle.Instance, Rectangle);
            dict.Add (MetaTriangle.Instance, Triangle);
            dict.Add (MetaCircle.Instance, Circle);
            dict.Add (MetaLine.Instance, Line);
            dict.Add (MetaBypass.Instance, Bypass);
            dict.Add (MetaEllipse.Instance, Ellipse);
            Dictionary = new ReadOnlyDictionary<FieldDefinition, MetaGeometryValidator> (dict);
        }
    }
}

