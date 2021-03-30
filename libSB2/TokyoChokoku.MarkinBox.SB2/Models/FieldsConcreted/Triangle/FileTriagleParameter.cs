using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileTriangleParameter
    {
        TriangleParameter p { get; }

        static double BasePointXOffset(IBaseTriangleParameter p)
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

        static double BasePointYOffset(IBaseTriangleParameter p)
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

        public TriangleParameter Unwrap => p;



        static Position2D PivotMove(double startTheta, double deltaX, double deltaY)
        {
            MatrixContext mc = new MatrixContext();
            mc.SetEntity();
            // mc.Rotate(startTheta); // 回転すると位置がずれる．
            return mc.CurrentMatrix * new Position2D(deltaX, deltaY);
        }


        public FileTriangleParameter(IBaseTriangleParameter p)
        {
            this.p = TriangleParameter.CopyOf(p);
        }

        public static FileTriangleParameter FromAppParameter(IBaseTriangleParameter p)
        {
            var dx = -BasePointXOffset(p);
            var dy = -BasePointYOffset(p);
            var apTheta = -p.Angle;
            var apHornX = (double)p.HornX;

            Position2D fPos = PivotMove((double)apTheta, dx, dy);
            double fHornX   = ((double)p.X) + apHornX;

            // 変換
            var f = MutableTriangleParameter.CopyOf(p);
            f.X     += decimal.Round((decimal)fPos.X, 3);
            f.Y     += decimal.Round((decimal)fPos.Y, 3);
            f.HornX  = decimal.Round((decimal)fHornX, 3);
            return new FileTriangleParameter(f);
        }


        public MutableTriangleParameter ToAppParameter()
        {
            var dx = BasePointXOffset(p);
            var dy = BasePointYOffset(p);
            var fTheta = -p.Angle;
            var fHornX = (double) p.HornX;

            Position2D apPos   = PivotMove((double)fTheta, dx, dy);
            double     apHornX = fHornX - ((double)p.X + dx);

            // 変換
            var ap = MutableTriangleParameter.CopyOf(p);
            ap.X     += decimal.Round((decimal)apPos.X, 3);
            ap.Y     += decimal.Round((decimal)apPos.Y, 3);
            ap.HornX  = decimal.Round((decimal)apHornX, 3);
            return ap;
        }

    }
}
