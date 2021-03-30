using System;
using System.Collections.Generic;
using System.Linq;
using TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class CombinedCollisionDrawable: ICollisionDrawable
    {
        CombinedCollision Collision { get; }
        List<(String Key, ICollisionDrawable CollisionDrawable)> DrawableList { get; }

        public CombinedCollisionDrawable(CombinedCollision collision, CollisionDrawableFactory factory)
        {
            Collision = collision;
            DrawableList = (
                from e in Collision.Collisions.ToList()
                select (Key: e.Key, CollisionDrawable: factory.CreateFrom(e.Collision))
            ).ToList();
        }

        public void Draw(FieldCanvas canvas)
        {
            foreach (var e in DrawableList)
            {
                canvas.Context.SaveState();
                try
                {
                    e.CollisionDrawable.Draw(canvas);
                }
                finally
                {
                    canvas.Context.RestoreState();
                }
            }
        }
    }
}
