using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public enum ValidationCategory
    {
        Text,
        Geometry,
        Marking,
    }

    public static class ValidationCategoryExt
    {
        public static string Suffix (this ValidationCategory category)
        {
            switch (category) {
            case ValidationCategory.Text:
                return "TextValidator";

            case ValidationCategory.Geometry:
                return "GeometryValidator";

            case ValidationCategory.Marking:
                return "MarkingValidator";

            default:
                throw new ArgumentOutOfRangeException ();
            }
        }
    }
}