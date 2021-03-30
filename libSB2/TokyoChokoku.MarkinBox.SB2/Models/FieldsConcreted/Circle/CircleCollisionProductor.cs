using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// 円フィールドの衝突判定作成
    /// </summary>
    public class CircleCollisionProductor
    {
        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (Circle.Constant target) {
            return CreatePrecision (target);
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (Circle.Constant target)
        {
            var p         = target.Parameter;
            var factory   = new CollisionFactory ();
            var transform = factory.Transform;

            transform.Translate ((double)p.X, (double)p.Y);
            transform.Translate (GetCircleOriginFromBasePoint (p));

            return factory.NewCircle (FamousVectors.Zero.OfHomogeneous2D, (double)p.Radius);
        }



        static Position2D GetCircleOriginFromBasePoint (CircleParameter p) {
            double radius = (double)p.Radius;
            double x, y;


            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointLT:
                x = radius;
                break;

            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointCT:
                x = 0;
                break;

            case Consts.FieldBasePointRB:
            case Consts.FieldBasePointRM:
            case Consts.FieldBasePointRT:
                x = -radius;
                break;
            }

            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLT:
            case Consts.FieldBasePointCT:
            case Consts.FieldBasePointRT:
                y = radius;
                break;

            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointRM:
                y = 0;
                break;

            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointRB:
                y = -radius;
                break;
            }

            return new Position2D (x, y);
        }
    }
}

