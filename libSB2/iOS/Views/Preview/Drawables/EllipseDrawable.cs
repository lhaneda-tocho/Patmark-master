using System;

using UIKit;
using CoreGraphics;
using CoreAnimation;
using Foundation;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class EllipseDrawable : FieldDrawable
	{
		private readonly EllipseParameter  p;



        private readonly float             baseX;
        private readonly float             baseY;
        private readonly float             angle;

		private readonly CGRect            rect;
		


        public EllipseDrawable (Ellipse.Constant mbobject)
		{
			this.p = mbobject.Parameter;

            baseX = (float)p.X;
            baseY = (float)p.Y;
            angle = (float)p.Angle;
			
			rect = CreateEllipse ();
		}



        public void Draw (FieldCanvas canvas)
		{
            var context = canvas.Context;
            var scale = canvas.CanvasViewScaling;

            float absoluteBaseX;
            float absoluteBaseY;

            {
                var m = FieldCanvas.Translate (scale, baseX, baseY);
                absoluteBaseX = (float) m.x0;
                absoluteBaseY = (float) m.y0;
            }

            var absoluteScaleRect = scale.TransformRect (rect);

			canvas.Context.SaveState ();

			UIColor.FromRGBA(  0,   0,   0, 255).SetStroke();
			context.BeginPath ();
                FieldCanvas.Concat (context, canvas.CanvasViewTranslationRotation);
                FieldCanvas.Translate (context, absoluteBaseX, absoluteBaseY);
                FieldCanvas.Rotate (context, -angle); 

                context.AddEllipseInRect (absoluteScaleRect);
			    context.StrokePath ();
			context.RestoreState ();
		}


        public void DrawBorder (FieldCanvas canvas)
		{
            var scale = canvas.CanvasViewScaling;

            float absoluteBaseX;
            float absoluteBaseY;

            {
                var m = FieldCanvas.Translate (scale, baseX, baseY);
                absoluteBaseX = (float) m.x0;
                absoluteBaseY = (float) m.y0;
            }

            var absoluteRect = scale.TransformRect (rect);

            canvas.Context.SaveState ();
                canvas.Context.BeginPath ();
                FieldCanvas.Concat (canvas.Context, canvas.CanvasViewTranslationRotation);
                FieldCanvas.Translate (canvas.Context, absoluteBaseX, absoluteBaseY);
                FieldCanvas.Rotate (canvas.Context, -angle); 

                canvas.Context.AddRect (absoluteRect);
                canvas.Context.StrokePath ();
            canvas.Context.RestoreState ();
		}


        public void DrawBasePoint (FieldCanvas canvas)
        {
            canvas.DrawBasePoint (canvas.CanvasViewMatrix, baseX, baseY);
        }


		private static CGPoint[] Transform (CGAffineTransform matrix, CGPoint[] points) {
			points = (CGPoint[]) points.Clone ();

			for (int i = 0; i < points.Length; i++) {
				points [i] = matrix.TransformPoint (points [i]);
			}

			return points;
		}


		private CGRect CreateEllipse () {
			nfloat x, y, width, height;

			width  = (nfloat)p.Width;
			height = (nfloat)p.Height;


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

			return new CGRect (x, y, width, height);
		}

	}
}

