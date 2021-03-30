using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public interface ICollisionVisitor<T, R>
    {
        R Visit(EmptyCollision    collision, T args);
        R Visit(CombinedCollision collision, T args);
        R Visit(PointCollision    collision, T args);
        R Visit(TriangulateStrip  collision, T args);
        R Visit(ConvexHull        collision, T args);
        R Visit(LineStrip         collision, T args);

    }
}
