using System;


using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public class FileOuterArcTextParameter
    {
        OuterArcTextParameter p { get; }

        static double BasePointPhase(IBaseOuterArcTextParameter p)
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
            return offsetRate* (p.InnerTextLength / p.InnerRadius) * 180.0 / Math.PI;
        }

        static double BasePointRadiusOffset(IBaseOuterArcTextParameter p)
        {
            double offsetRate;
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointRB:
                    offsetRate = 0.0;
                    break;
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointRM:
                    offsetRate = 0.5;
                    break;
                case Consts.FieldBasePointLT:
                case Consts.FieldBasePointCT:
                case Consts.FieldBasePointRT:
                    offsetRate = 1.0;
                    break;
            }
            return offsetRate * (p.OuterRadius - p.InnerRadius);
        }

        public OuterArcTextParameter Unwrap => p;


        public FileOuterArcTextParameter(IBaseOuterArcTextParameter p)
        {
            this.p = OuterArcTextParameter.CopyOf(p);
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


        public static FileOuterArcTextParameter FromAppParameter(IBaseOuterArcTextParameter p)
        {
            var rf  = p.InnerRadius+BasePointRadiusOffset(p);
            var drb = -BasePointRadiusOffset(p);
            var phi = -BasePointPhase(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove(rf, (double)fTheta, drb, phi);
            var apTheta = fTheta + (decimal) phi;

            // 変換
            var f = MutableOuterArcTextParameter.CopyOf(p);
            f.Angle = -decimal.Round(         apTheta, 3);
            f.X    +=  decimal.Round((decimal)apPos.X, 3);
            f.Y    +=  decimal.Round((decimal)apPos.Y, 3);
            return new FileOuterArcTextParameter(f);
        }


        public MutableOuterArcTextParameter ToAppParameter()
        {
            var rf  = p.InnerRadius;
            var drb = BasePointRadiusOffset(p);
            var phi = BasePointPhase(p);
            var fTheta = -p.Angle;

            Position2D apPos = PivotMove(rf, (double)fTheta, drb, phi);
            var apTheta = fTheta + (decimal) phi;

            // 変換
            var ap = MutableOuterArcTextParameter.CopyOf(p);
            ap.Angle = -decimal.Round(         apTheta, 3);
            ap.X    +=  decimal.Round((decimal)apPos.X, 3);
            ap.Y    +=  decimal.Round((decimal)apPos.Y, 3);
            return ap;
        }


    }
}
