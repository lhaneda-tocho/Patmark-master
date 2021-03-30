using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileHorizontalTextParameter
    {
        HorizontalTextParameter p { get; }

        static double BasePointXOffset(IBaseHorizontalTextParameter p)
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
            return offsetRate * (double)p.BoxWidth;
        }

        static double BasePointYOffset(IBaseHorizontalTextParameter p)
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
            return offsetRate * (double)p.BoxHeight;
        }

        public HorizontalTextParameter Unwrap => p;



        static Position2D PivotMove(double startTheta, double deltaX, double deltaY)
        {
            MatrixContext mc = new MatrixContext();
            mc.SetEntity();
            mc.Rotate(startTheta); 
            return mc.CurrentMatrix * new Position2D(deltaX, deltaY);
        }


        public FileHorizontalTextParameter(IBaseHorizontalTextParameter p)
        {
            this.p = HorizontalTextParameter.CopyOf(p);
        }

        public static FileHorizontalTextParameter FromAppParameter(IBaseHorizontalTextParameter p)
        {
            var dx = -BasePointXOffset(p);
            var dy = -BasePointYOffset(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var f = MutableHorizontalTextParameter.CopyOf(p);
            f.X += decimal.Round((decimal)apPos.X, 3);
            f.Y += decimal.Round((decimal)apPos.Y, 3);
            return new FileHorizontalTextParameter(f);
        }


        public MutableHorizontalTextParameter ToAppParameter()
        {
            var dx = BasePointXOffset(p);
            var dy = BasePointYOffset(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove((double)fTheta, dx, dy);

            // 変換
            var ap = MutableHorizontalTextParameter.CopyOf(p);
            ap.X += decimal.Round((decimal)apPos.X, 3);
            ap.Y += decimal.Round((decimal)apPos.Y, 3);
            return ap;
        }

    }
}
