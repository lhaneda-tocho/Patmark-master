using System;
using Android.Content;
using TokyoChokoku.MarkinBox.Sketchbook;


namespace TokyoChokoku.Patmark.Droid
{
    public static class FieldTypeNameExt
    {
        public static int LabelId(this Structure.FieldType t){
            switch(t){
                case Structure.FieldType.Text:
                    return Resource.String.FieldTypeName_Text;
                case Structure.FieldType.OuterArcText:
                    return Resource.String.FieldTypeName_OuterArcText;
                case Structure.FieldType.InnerArcText:
                    return Resource.String.FieldTypeName_InnerArcText;
                case Structure.FieldType.XVerticalText:
                    return Resource.String.FieldTypeName_XVerticalText;
                case Structure.FieldType.YVerticalText:
                    return Resource.String.FieldTypeName_YVerticalText;
                case Structure.FieldType.QrCode:
                    return Resource.String.FieldTypeName_QrCode;
                case Structure.FieldType.DataMatrix:
                    return Resource.String.FieldTypeName_DataMatrix;
                case Structure.FieldType.Rectangle:
                    return Resource.String.FieldTypeName_Rectangle;
                case Structure.FieldType.Circle:
                    return Resource.String.FieldTypeName_Circle;
                case Structure.FieldType.Line:
                    return Resource.String.FieldTypeName_Line;
                case Structure.FieldType.Ellipse:
                    return Resource.String.FieldTypeName_Ellipse;
                case Structure.FieldType.Triangle:
                    return Resource.String.FieldTypeName_Triangle;
                case Structure.FieldType.Bypass:
                    return Resource.String.FieldTypeName_Bypass;
                default:
                    return Resource.String.FieldTypeName_Unknown;
            }
        }
    }
}
