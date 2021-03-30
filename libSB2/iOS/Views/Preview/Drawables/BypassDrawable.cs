using System;

using CoreGraphics;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class BypassDrawable : FieldDrawable
	{
		private readonly BypassParameter p;

        


		private readonly CGPoint[][]  cross = new CGPoint[][] {
			new CGPoint[2], new CGPoint[2]
		};

        private CGPoint[][] CopiedCross {
            get {
                var copied = (CGPoint [][]) cross.Clone ();
                for (int i = 0; i < copied.Length; i++) {
                    copied [i] = (CGPoint []) copied [i].Clone ();
                }
                return copied;
            }
        }

		private readonly CGRect      rectangle;


        public BypassDrawable (Bypass.Constant mbobject)
		{
			this.p = mbobject.Parameter;

			nfloat x = (nfloat)p.X;
			nfloat y = (nfloat)p.Y;
            nfloat size = (nfloat)BypassConstant.MarkerSize;

			CGPoint[][] rectangleVertices = new CGPoint[2][] {
				new CGPoint[2], new CGPoint[2]
			};

			rectangleVertices [0][0] = new CGPoint (x - size / 2, y - size / 2);
			rectangleVertices [0][1] = new CGPoint (x - size / 2, y + size / 2);

			rectangleVertices [1][0] = new CGPoint (x + size / 2, y - size / 2);
			rectangleVertices [1][1] = new CGPoint (x + size / 2, y + size / 2);

			cross  =  new CGPoint[2][] {
				new CGPoint[2], new CGPoint[2]
			};

			cross [0][0] = rectangleVertices [0][0];
			cross [0][1] = rectangleVertices [1][1];

			cross [1][0] = rectangleVertices [0][1];
			cross [1][1] = rectangleVertices [1][0];
	
			rectangle = new CGRect (rectangleVertices [0][0], new CGSize (size, size));
		}



        public void Draw (FieldCanvas canvas)
		{
			var canvasView = canvas.CanvasViewMatrix;
			
            var viewCross = CopiedCross;

			for (int i = 0; i < viewCross.Length; i++) {
				for (int j = 0; j < viewCross[i].Length; j++) {
					viewCross [i][j] = canvasView.TransformPoint (viewCross [i][j]);
				}
			}

            canvas.Context.SaveState ();
    			canvas.Context.AddLines (viewCross[0]);
    			canvas.Context.StrokePath ();

    			canvas.Context.AddLines (viewCross[1]);
    			canvas.Context.StrokePath ();
            canvas.Context.RestoreState ();

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
			
		}



	}
}

