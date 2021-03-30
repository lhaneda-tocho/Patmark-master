using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public class TriangleCollisionProductor
    {
        public static ICollision Create (Triangle.Constant target)
        {
            return CreatePrecision (target);
        }

        public static ICollision CreatePrecision (Triangle.Constant target) {
            var p = target.Parameter;
            var factory = new CollisionFactory ();
            var transform = factory.Transform;

            transform.Translate ((double)p.X, (double)p.Y);
            transform.Rotate    ((double)-p.Angle);

            var vertices = CreateTriangle (p);

            return factory.NewTriangle (vertices[0], vertices[1], vertices[2]);
        }




        static Homogeneous2D[] CreateTriangle (TriangleParameter p) {
            double x, y, width, height, hornX;

            width  = (double)p.Width;
            height = (double)p.Height;
            hornX  = (double)p.HornX;

            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointLT:
                x = 0;
                break;

            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointCT:
                x = -width / 2;
                break;

            case Consts.FieldBasePointRB:
            case Consts.FieldBasePointRM:
            case Consts.FieldBasePointRT:
                x = -width;
                break;
            }

            switch (p.BasePoint) {
            default:
            case Consts.FieldBasePointLT:
            case Consts.FieldBasePointCT:
            case Consts.FieldBasePointRT:
                y = 0;
                break;

            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointRM:
                y = -height / 2;
                break;

            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointRB:
                y = -height;
                break;
            }

            return new Homogeneous2D[] {
                new Homogeneous2D (    hornX,            y),
                new Homogeneous2D (x + width,   height + y),
                new Homogeneous2D (x        ,   height + y)
            };
        }
    }
}

