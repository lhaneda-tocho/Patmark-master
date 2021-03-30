using System;
using System.Linq;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// DataMatrixの衝突判定ファクトリ
    /// </summary>
    public class DataMatrixCollisionProductor
    {

        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (DataMatrix.Constant target) {
            return CreatePrecision (target);
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (DataMatrix.Constant target)
        {
            var p = target.Parameter;
            var factory = new CollisionFactory ();
            var transform = factory.Transform;
            
            var vertices = CreateRectangle (p);

            return factory.NewConvexRectangle (vertices [0], vertices [1], vertices [2], vertices [3]);
        }


        static Homogeneous2D[] CreateRectangle (DataMatrixParameter p) {
            var c = p.CalcCornerPoints();
            return c.Select(v => new Homogeneous2D(v.X, v.Y)).Reverse().ToArray();
        }
    }
}

