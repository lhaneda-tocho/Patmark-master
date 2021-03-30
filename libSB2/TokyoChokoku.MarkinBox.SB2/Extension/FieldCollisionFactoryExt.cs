namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using CollisionUtil;
    using Fields;
    using Parameters;

    public static class FieldCollisionFactoryExt
    {
        public static ICollision CreateCollision (this IField<IConstantParameter> field)
        {
            return field.Accept (new VisitorOfFieldCollisionFactory (), null);
        }

        public static ICollision CreateCollision (this IMutableField<IMutableParameter> field)
        {
            return field.Accept (new VisitorOfFieldCollisionFactory (), null);
        }

        public static ICollision CreatePreciseCollision (this IField<IConstantParameter> field)
        {
            return field.Accept (new VisitorOfFieldCollisionFactory (true), null);
        }

        public static ICollision CreatePreciseCollision (this IMutableField<IMutableParameter> field)
        {
            return field.Accept (new VisitorOfFieldCollisionFactory (true), null);
        }
    }
}
