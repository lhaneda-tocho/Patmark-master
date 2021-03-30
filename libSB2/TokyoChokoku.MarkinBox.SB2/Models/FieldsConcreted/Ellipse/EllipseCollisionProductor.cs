using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public class EllipseCollisionProductor
    {

        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (Ellipse.Constant target) {
            return CreatePrecision (target);
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (Ellipse.Constant target)
        {
            var p = target.Parameter;
            double x = (double)p.X;
            double y = (double)p.Y;
            double width = (double)p.Width;
            double height = (double)p.Height;
            double angle = (double)p.Angle;

            CollisionFactory factory = new CollisionFactory ();
            MatrixContext transform = factory.Transform;

            transform.Translate (x, y);
            transform.Rotate (-angle);
            transform.Translate (GetEllipseOriginFromBasePoint (p));
            transform.Scale (width, height);

            return factory.NewCircle (FamousVectors.Zero.OfHomogeneous2D, 0.5);
        }




        static Position2D GetEllipseOriginFromBasePoint (EllipseParameter p) {
            double x, y, width, height;

            width  = (double)p.Width;
            height = (double)p.Height;


            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointLT:
                x =  width / 2;
                break;

            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointCT:
                x = 0;
                break;

            case Consts.FieldBasePointRB:
            case Consts.FieldBasePointRM:
            case Consts.FieldBasePointRT:
                x = -width / 2;;
                break;
            }

            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLT:
            case Consts.FieldBasePointCT:
            case Consts.FieldBasePointRT:
                y =  height / 2;
                break;

            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointRM:
                y = 0;
                break;

            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointRB:
                y = -height / 2;
                break;
            }

            return new Position2D (x, y);
        }
    }
}

