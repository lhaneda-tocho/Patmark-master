using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public partial class IBaseLineParameter
    {
        public RectangleArea Bounds {
            get {
                return CalcBoundingBox ();
            }
        }

        public RectangleArea CalcBoundingBox () {
            double startX, endX, startY, endY;

            if (IsBezierCurve) {
                var curve = CreateCurve ();

                startX = curve.MinX;
                endX   = curve.MaxX;

                startY = curve.MinY;
                endY   = curve.MaxY;


                for (int i = 0; i <= 10; i++) {
                    float ratio = (float)i / 10;
                    System.Diagnostics.Debug.WriteLine (
                        "ratio " + ratio + " : " + curve.CalculateX(ratio));
                }
                System.Diagnostics.Debug.WriteLine (
                    "extreme " + curve.XExtremePoint);
            } else {
                startX = (double) ( (StartX <  EndX) ? StartX : EndX );
                endX   = (double) ( (StartX >  EndX) ? StartX : EndX );

                startY = (double) ( (StartY <  EndY) ? StartY : EndY );
                endY   = (double) ( (StartY >  EndY) ? StartY : EndY );
            }


            return new RectangleArea (startX, startY, endX - startX, endY - startY);
        }


        public QuadBezierCurve CreateCurve () {
            return new QuadBezierCurve (
                new Position2D ((double )StartX,(double)  StartY),
                new Position2D ((double)CenterX,(double) CenterY),
                new Position2D ((double)   EndX,(double)    EndY)
            );
        }

    }
}
