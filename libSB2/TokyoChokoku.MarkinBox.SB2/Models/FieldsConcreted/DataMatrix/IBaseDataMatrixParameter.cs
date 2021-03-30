using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public partial class IBaseDataMatrixParameter
    {
        public RectangleArea Bounds {
            get {
                return CalcBoundingBox ();
            }
        }

        public decimal Width {
            get
            {
                return Height * (decimal) DotCount.AspectWidth;
            }
            set {
                Height = value * (decimal) DotCount.AspectHeight;
            }
        }

        public RootFieldTextNode ParseText () {
            return FieldTextParser.ParseTextWithoutLogo (Text);
        }


        public RectangleArea CalcBoundingBox () {
            Position2D[] corners = CalcCornerPoints ();

            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            foreach (var p in corners) {
                maxX = Math.Max (p.X, maxX);
                maxY = Math.Max (p.Y, maxY);
                minX = Math.Min (p.X, minX);
                minY = Math.Min (p.Y, minY);
            }

            System.Diagnostics.Debug.WriteLine (minX + ", " + minY + "; " + maxX + ", " + maxY);

            return new RectangleArea (minX, minY, maxX-minX, maxY-minY);
        }

        public Position2D[] CalcCornerPoints () {

            var m = new MatrixContext ();
            m.Translate ( (double)X, (double)Y );
            m.Rotate    ( -(double)Angle );

            return CreateRectangle ().ApplyTransform (m);
        }



        private RectangleArea CreateRectangle () {
            double x, y, width, height;

            width  = (double)Width;
            height = (double)Height;


            switch (BasePoint) {
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

            switch (BasePoint) {
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

            return new RectangleArea (x, y, width, height);
        }
    }
}
