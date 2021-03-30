using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

using CoreGraphics;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class LineDrawable : FieldDrawable
    {
        private static readonly double PointSize = 0.4;

        private readonly LineParameter  p;

        private readonly CGPoint        start;
        private readonly CGPoint        center;
        private readonly CGPoint        end;

        private static UIBezierPath CreateCurve (CGPoint start, CGPoint center, CGPoint end, bool isBezier) {
            UIBezierPath path = new UIBezierPath ();

            if (isBezier) {
                path.MoveTo (start);
                path.AddQuadCurveToPoint (end, center);
            } else {
                path.MoveTo    (start);
                path.AddLineTo (end);
            }

            return path;
        }

        private static UIBezierPath CreateHandlePath (CGPoint start, CGPoint center, CGPoint end) {
            UIBezierPath path = new UIBezierPath ();

            path.MoveTo    (start);
            path.AddLineTo (center);
            path.AddLineTo (end);

            return path;
        }

        private static CGPoint CreatePoint (decimal x, decimal y) {
            return new CGPoint ((nfloat)x, (nfloat)y);
        }




        public LineDrawable (Line.Constant mbobject)
        {
            this.p = mbobject.Parameter;

            start  = CreatePoint (p.X   , p.Y   );
            end    = CreatePoint (p.EndX, p.EndY);

            if (p.IsBezierCurve) {
                center = CreatePoint (
                    p.CenterX, p.CenterY);
            } else {
                center = new CGPoint (
                    (start.X + end.X) / 2, (start.Y + end.Y) / 2);
            }


        }




        public void Draw (FieldCanvas canvas)
        {
            var curve  = CreateCurve      (start, center, end, p.IsBezierCurve);
            curve.ApplyTransform (canvas.CanvasViewMatrix);

            canvas.Context.BeginPath    ();
            canvas.Context.AddPath      (curve.CGPath);
            canvas.Context.StrokePath   ();

//            if (p.IsBezierCurve) {
//                var handle = CreateHandlePath (start, center, end);
//                handle.ApplyTransform (canvas.CanvasViewMatrix);
//
//                canvas.Context.BeginPath    ();
//                canvas.Context.AddPath      (handle.CGPath);
//                canvas.Context.StrokePath   ();
//            }

//            canvas.DrawPoint (canvas.CanvasViewMatrix, start , PointSize);
//            canvas.DrawPoint (canvas.CanvasViewMatrix, center, PointSize);
//            canvas.DrawPoint (canvas.CanvasViewMatrix, end   , PointSize);

        }

        public void DrawBorder (FieldCanvas canvas)
        {
            var curve  = CreateCurve      (start, center, end, p.IsBezierCurve);
            curve.ApplyTransform (canvas.CanvasViewMatrix);

            canvas.Context.BeginPath    ();
            canvas.Context.AddPath      (curve.CGPath);
            canvas.Context.StrokePath   ();

//            if (p.IsBezierCurve) {
//                var handle = CreateHandlePath (start, center, end);
//                handle.ApplyTransform (canvas.CanvasViewMatrix);
//
//                canvas.Context.BeginPath    ();
//                canvas.Context.AddPath      (handle.CGPath);
//                canvas.Context.StrokePath   ();
//            }

            canvas.DrawPoint (canvas.CanvasViewMatrix, start , PointSize);
            canvas.DrawPoint (canvas.CanvasViewMatrix, center, PointSize);
            canvas.DrawPoint (canvas.CanvasViewMatrix, end   , PointSize);
        }

        public void DrawBasePoint (FieldCanvas canvas)
        {
            canvas.DrawBasePoint (canvas.CanvasViewMatrix, start.X, start.Y);
        }


    }
}

