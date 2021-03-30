using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileCircleParameter
    {
        CircleParameter p { get; }

        static double BasePointXOffset(IBaseCircleParameter p)
        {
            double offsetRate;
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointLT:
                    offsetRate = 0.0;
                    break;
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointCT:
                    offsetRate = 0.5;
                    break;
                case Consts.FieldBasePointRB:
                case Consts.FieldBasePointRM:
                case Consts.FieldBasePointRT:
                    offsetRate = 1.0;
                    break;
            }
            return offsetRate * (double) p.Radius * 2;
        }

        static double BasePointYOffset(IBaseCircleParameter p)
        {
            double offsetRate;
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointRB:
                    offsetRate = -0.0;
                    break;
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointRM:
                    offsetRate = -0.5;
                    break;
                case Consts.FieldBasePointLT:
                case Consts.FieldBasePointCT:
                case Consts.FieldBasePointRT:
                    offsetRate = -1.0;
                    break;
            }
            return offsetRate * (double)p.Radius * 2;
        }

        public CircleParameter Unwrap => p;



        static Position2D PivotMove(double startTheta, double deltaX, double deltaY)
        {
            MatrixContext mc = new MatrixContext();
            mc.SetEntity();
            mc.Rotate(startTheta);
            return mc.CurrentMatrix * new Position2D(deltaX, deltaY);
        }


        public FileCircleParameter(IBaseCircleParameter p)
        {
            this.p = CircleParameter.CopyOf(p);
        }

        public static FileCircleParameter FromAppParameter(IBaseCircleParameter p)
        {
            var dx = -BasePointXOffset(p);
            var dy = -BasePointYOffset(p);
            var fTheta = 0;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var f = MutableCircleParameter.CopyOf(p);
            f.X += decimal.Round((decimal)apPos.X, 3);
            f.Y += decimal.Round((decimal)apPos.Y, 3);
            return new FileCircleParameter(f);
        }


        public MutableCircleParameter ToAppParameter()
        {
            var dx = BasePointXOffset(p);
            var dy = BasePointYOffset(p);
            var fTheta = 0;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var ap = MutableCircleParameter.CopyOf(p);
            ap.X += decimal.Round((decimal)apPos.X, 3);
            ap.Y += decimal.Round((decimal)apPos.Y, 3);
            return ap;
        }

    }
}
