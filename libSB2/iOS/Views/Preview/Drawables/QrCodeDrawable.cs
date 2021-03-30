using System;

using UIKit;
using CoreGraphics;
using CoreAnimation;
using Foundation;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class QrCodeDrawable : FieldDrawable
	{
		private readonly QrCodeParameter p;



        private readonly float  baseX;
        private readonly float  baseY;
        private readonly float  angle;


		private readonly CGRect rectangle;


        public QrCodeDrawable (QrCode.Constant mbobject)
		{
			this.p = mbobject.Parameter;


            this.baseX = (float)p.X;
            this.baseY = (float)p.Y;
            this.angle = (float)p.Angle;

            rectangle = CreateRectangle ();
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

            var absoluteScaleRect = scale.TransformRect (rectangle);

            context.SaveState ();

                FieldCanvas.Concat    (context, canvas.CanvasViewTranslationRotation);
                FieldCanvas.Translate (context, absoluteBaseX, absoluteBaseY);
                FieldCanvas.Rotate    (context, -angle); 

                context.SaveState ();
                    UIColor.FromRGBA(255, 255, 255, 255).SetFill();

                    context.BeginPath ();
                        context.AddRect (absoluteScaleRect);
                    context.FillPath ();
                        context.AddRect (absoluteScaleRect);
                    context.StrokePath ();
                context.RestoreState ();

                
                if (p.Mirrored)
                    FieldCanvas.XAxisMirror (canvas.Context, absoluteScaleRect.Width / 2);
                

                FieldCanvas.Translate (
                    context,
                    absoluteScaleRect.X + absoluteScaleRect.Width  / 4,
                    absoluteScaleRect.Y + absoluteScaleRect.Height / 3);

                FieldCanvas.Scale (
                    context, 
                    absoluteScaleRect.Width  / 3,
                    absoluteScaleRect.Height / 3);


                context.SelectFont    (FieldCanvas.FontDefault, 1, CGTextEncoding.MacRoman);
                canvas.DrawStringHere ("QR");

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

            var absoluteRect = scale.TransformRect (rectangle);

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


		private CGRect CreateRectangle () {
			float x, y, width, height;

            width = height = (float)p.Height;


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

			return new CGRect (
				(nfloat)x, (nfloat)y, (nfloat)width, (nfloat)height);
		}

	}
}

