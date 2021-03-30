using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileInnerArcTextParameter
    {
        InnerArcTextParameter p { get; }

        static double BasePointPhase(IBaseInnerArcTextParameter p)
        {
            double offsetRate;
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointLT:
                    offsetRate = -1.0;
                    break;
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointCT:
                    offsetRate = -0.5;
                    break;
                case Consts.FieldBasePointRB:
                case Consts.FieldBasePointRM:
                case Consts.FieldBasePointRT:
                    offsetRate = -0.0;
                    break;
            }
            //offsetRate = 0;
            return offsetRate * (p.OuterTextLength / p.OuterRadius) * 180.0 / Math.PI;
        }

        static double BasePointRadiusOffset(IBaseInnerArcTextParameter p)
        {
            double offsetRate;
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointRB:
                    offsetRate = -1.0;
                    break;
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointRM:
                    offsetRate = -0.5;
                    break;
                case Consts.FieldBasePointLT:
                case Consts.FieldBasePointCT:
                case Consts.FieldBasePointRT:
                    offsetRate = -0.0;
                    break;
            }
            //offsetRate = 0;
            return offsetRate * (p.OuterRadius - p.InnerRadius);
        }

        public InnerArcTextParameter Unwrap => p;


        public FileInnerArcTextParameter(IBaseInnerArcTextParameter p)
        {
            this.p = InnerArcTextParameter.CopyOf(p);
        }


        static Position2D PivotMove(double startR, double startTheta, double deltaR, double phi)
        {
            MatrixContext mc = new MatrixContext();
            mc.SetEntity();
            mc.Rotate(startTheta);
            mc.Translate(new Position2D(0, startR));
            mc.Rotate(phi);
            mc.Translate(new Position2D(0, -startR - deltaR));
            return mc.CurrentMatrix * Position2D.Zero;
        }


        public static FileInnerArcTextParameter FromAppParameter(IBaseInnerArcTextParameter p)
        {
            var rf = p.OuterRadius + BasePointRadiusOffset(p);
            var drb = -BasePointRadiusOffset(p);
            var phi = -BasePointPhase(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove(rf, (double)fTheta, drb, phi);
            var apTheta = fTheta + (decimal)phi;

            // 変換
            var f = MutableInnerArcTextParameter.CopyOf(p);
            f.Angle = -decimal.Round(        apTheta, 3);
            f.X    += decimal.Round((decimal)apPos.X, 3);
            f.Y    += decimal.Round((decimal)apPos.Y, 3);
            return new FileInnerArcTextParameter(f);
        }


        public MutableInnerArcTextParameter ToAppParameter()
        {
            var rf = p.OuterRadius;
            var drb = BasePointRadiusOffset(p);
            var phi = BasePointPhase(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove(rf, (double)fTheta, drb, phi);
            var apTheta = fTheta + (decimal)phi;

            // 変換
            var ap = MutableInnerArcTextParameter.CopyOf(p);
            ap.Angle = -decimal.Round(        apTheta, 3);
            ap.X    += decimal.Round((decimal)apPos.X, 3);
            ap.Y    += decimal.Round((decimal)apPos.Y, 3);
            return ap;
        }

    }
}
