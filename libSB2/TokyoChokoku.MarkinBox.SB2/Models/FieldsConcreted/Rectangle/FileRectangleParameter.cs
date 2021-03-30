using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileRectangleParameter
    {
        RectangleParameter p { get; }

        static double BasePointXOffset(IBaseRectangleParameter p)
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
            return offsetRate * (double) p.Width;
        }

        static double BasePointYOffset(IBaseRectangleParameter p)
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
            return offsetRate * (double)p.Height;
        }

        public RectangleParameter Unwrap => p;



        static Position2D PivotMove(double startTheta, double deltaX, double deltaY)
        {
            MatrixContext mc = new MatrixContext();
            mc.SetEntity();
            // mc.Rotate(startTheta); // 回転すると位置がずれる．
            return mc.CurrentMatrix * new Position2D(deltaX, deltaY);
        }


        public FileRectangleParameter(IBaseRectangleParameter p)
        {
            this.p = RectangleParameter.CopyOf(p);
        }

        public static FileRectangleParameter FromAppParameter(IBaseRectangleParameter p)
        {
            var dx = -BasePointXOffset(p);
            var dy = -BasePointYOffset(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var f = MutableRectangleParameter.CopyOf(p);
            f.X += decimal.Round((decimal)apPos.X, 3);
            f.Y += decimal.Round((decimal)apPos.Y, 3);
            return new FileRectangleParameter(f);
        }


        public MutableRectangleParameter ToAppParameter()
        {
            var dx = BasePointXOffset(p);
            var dy = BasePointYOffset(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var ap = MutableRectangleParameter.CopyOf(p);
            ap.X += decimal.Round((decimal)apPos.X, 3);
            ap.Y += decimal.Round((decimal)apPos.Y, 3);
            return ap;
        }

    }
}
