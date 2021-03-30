using System;
using System.Linq;
using CoreGraphics;
using TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class ConvexHullDrawable: ICollisionDrawable
    {
        ConvexHull    Collision { get; }
        CGPoint[]     Vertices  { get; }

        public ConvexHullDrawable(ConvexHull collision)
        {
            Collision = collision ?? throw new ArgumentNullException("null not allowed");
            Vertices = (
                from v in collision.Vertices
                select new CGPoint(v.X, v.Y)
            ).ToArray();
        }

        public void Draw(FieldCanvas canvas)
        {
            var m = canvas.CanvasViewMatrix;
            canvas.DrawLoopedLines(m, Vertices, fill: true);
        }
    }
}
