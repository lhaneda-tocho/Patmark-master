using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// バイパスの衝突判定作成
    /// </summary>
    public class BypassCollisionProductor
    {

        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (Bypass.Constant target)
        {
            var p = target.Parameter;
            var factory = new CollisionFactory ();
            var transform = factory.Transform;

            transform.Translate ((double)p.X, (double)p.Y);

            var vertices = CreateBoundingBox (p);

            return factory.NewConvexRectangle (vertices [0], vertices [1], vertices [2], vertices [3]);
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (Bypass.Constant target)
        {
            var p = target.Parameter;
            var factory = new CollisionFactory ();

            return factory.NewPoint ((double)p.X, (double)p.Y);
        }


        static Homogeneous2D[] CreateBoundingBox (BypassParameter p) {
            double x, y, width, height;

            width  = height = BypassConstant.MarkerSize;
            x = y = -width / 2;

            return new Homogeneous2D[] {
                new Homogeneous2D (x        ,            y),
                new Homogeneous2D (x + width,            y),
                new Homogeneous2D (x + width,   height + y),
                new Homogeneous2D (x        ,   height + y)
            };
        }
    }
}

