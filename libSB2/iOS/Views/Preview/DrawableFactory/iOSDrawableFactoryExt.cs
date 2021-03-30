using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class iOSDrawableFactoryExt
    {
        public static FieldDrawable CreateDrawable (this IField<IConstantParameter> field)
        {
            return field.Accept (new VisitorOfiOSDrawableFactory (), null);
        }

        public static FieldDrawable CreateDrawable (this IMutableField<IMutableParameter> field)
        {
            return field.Accept (new VisitorOfiOSDrawableFactory (), null);
        }
    }
}
