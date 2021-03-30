using System;

using CoreGraphics;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class CircleDrawable : FieldDrawable
	{
		private readonly CircleParameter p;


        private readonly float baseX;
        private readonly float baseY;

		private readonly CGRect rectangle;


        public CircleDrawable (Circle.Constant mbobject)
		{
			this.p = mbobject.Parameter;

            baseX = (float)p.X;
            baseY = (float)p.Y;

			CGAffineTransform t = CGAffineTransform.MakeTranslation (
				baseX,
				baseY
			);

			rectangle = t.TransformRect (CreateRectangle());
		}



        public void Draw (FieldCanvas canvas)
		{
			var canvasView = canvas.CanvasViewMatrix;
			var viewRect = canvasView.TransformRect (rectangle);

			canvas.Context.AddEllipseInRect (viewRect);
			canvas.Context.StrokePath ();
		}

        public void DrawBorder (FieldCanvas canvas)
		{
			var canvasView = canvas.CanvasViewMatrix;
			var viewRect = canvasView.TransformRect (rectangle);

			canvas.Context.AddRect (viewRect);
			canvas.Context.StrokePath ();
		}

        public void DrawBasePoint (FieldCanvas canvas)
		{
            canvas.DrawBasePoint (canvas.CanvasViewMatrix, baseX, baseY);
		}


		private CGRect CreateRectangle () {
			decimal x, y, width, height;

			width = height = p.Radius * 2;


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
				x = -p.Radius;
				break;

			case Consts.FieldBasePointRB:
			case Consts.FieldBasePointRM:
			case Consts.FieldBasePointRT:
				x = -p.Radius * 2;
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
				y = -p.Radius;
				break;

			case Consts.FieldBasePointLB:
			case Consts.FieldBasePointCB:
			case Consts.FieldBasePointRB:
				y = -p.Radius * 2;
				break;
			}

			return new CGRect (
				(nfloat)x, (nfloat)y, (nfloat)width, (nfloat)height);
		}

	}
}

