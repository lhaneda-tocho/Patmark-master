using System;

using CoreGraphics;
using UIKit;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public sealed class XVerticalTextDrawable : FieldDrawable
	{
		readonly XVerticalTextParameter p;
        readonly RootFieldTextNode parsed;


        readonly float  baseX;
        readonly float  baseY;
        readonly float  angle;

        readonly float  oneCharWidth;
        readonly float  oneCharHeight;
        readonly float  pitchBetweenChars;

        readonly float  boxWidth ;
        readonly float  boxHeight;

        readonly Cartesian2D drawStartFromBasePoint;

        readonly ICollisionDrawable CollisionDrawable;


        public XVerticalTextDrawable (XVerticalText.Constant mbobject)
		{
			p = mbobject.Parameter;
            parsed = p.ParseText ();

            baseX = (float)p.X;
            baseY = (float)p.Y;
            angle = (float)p.Angle;

            boxWidth  = (float) p.BoxWidth;
            boxHeight = (float) p.BoxHeight;

            oneCharWidth = (float)p.Width;
            oneCharHeight = (float)p.Height;
            pitchBetweenChars = (float)p.Pitch;

            drawStartFromBasePoint = DrawStartFromBasePoint;

            CollisionDrawable = CollisionDrawableFactory.DefaultCreateFrom(mbobject.CreatePreciseCollision());

        }


        public void Draw(FieldCanvas canvas)
        {
            canvas.StateScope(() =>
            {
                var context = canvas.Context;
                CGAffineTransform m = FieldCanvas.MakeIdentity();

                m = FieldCanvas.Concat(m, canvas.CanvasViewMatrix);

                // ベースポイントの位置と回転を設定する．
                m = FieldCanvas.Translate(m, baseX, baseY);
                m = FieldCanvas.Rotate(m, -angle);


                // 描画開始位置まで移動する
                m = FieldCanvas.Translate(m, drawStartFromBasePoint.X, drawStartFromBasePoint.Y);


                // 描画
                DrawXVerticalText(
                    canvas,
                    m,
                    parsed,
                    oneCharWidth,
                    oneCharHeight,
                    pitchBetweenChars,
                    p.Mirrored
                );
            });
            canvas.StateScope(() =>
            {
                var border     = new CGColor(1.0f, 0.0f, 1.0f, 1.0f);
                var background = new CGColor(1.0f, 0.3f, 0.0f, 0.2f);

                var context = canvas.Context;
                context.SetStrokeColor(border);
                context.SetFillColor(background);
                CGAffineTransform m = FieldCanvas.MakeIdentity();
                CollisionDrawable.Draw(canvas);
            });
        }


        private void DrawXVerticalText(
            FieldCanvas canvas,
            CGAffineTransform transform,
            RootFieldTextNode root,
            float charWidth,
            float charHeight,
            float pitch,
            bool mirrored)
        {
            if (mirrored) {
                transform = FieldCanvas.YAxisMirror (transform, boxHeight/2);
            }

            canvas.DrawFieldTextNode (root, (index) => {
                var m = transform;

                m = FieldCanvas.Translate (m, pitch * index, 0);

                // 文字の向きと大きさを合わせる
                m = FieldCanvas.Rotate(m, -90);
                m = FieldCanvas.Scale(m, charWidth, charHeight);
                m = FieldCanvas.Translate(m, -1, 0);

                return m;
            });
        }


        public void DrawBorder (FieldCanvas canvas)
		{
            var m = canvas.CanvasViewMatrix;

            CGPoint[] vertices = {
                new CGPoint (      0 ,         0),
                new CGPoint (boxWidth,         0),
                new CGPoint (boxWidth, boxHeight),
                new CGPoint (       0, boxHeight),
            };


            // ベースポイントの位置と回転を設定する．
            m = FieldCanvas.Translate (m, baseX, baseY);
            m = FieldCanvas.Rotate    (m, -angle);


            // 描画開始位置まで 移動する．
            var start = drawStartFromBasePoint;
            m = FieldCanvas.Translate (m, start.X, start.Y);


            // 描画
            canvas.DrawLoopedLines (m, vertices);
		}

        public void DrawBasePoint (FieldCanvas canvas)
		{
            canvas.DrawBasePoint (canvas.CanvasViewMatrix, baseX, baseY);
		}

			

		private Cartesian2D DrawStartFromBasePoint
		{
			get {
				double startX, startY;

				switch (p.BasePoint) {
				default:
				case Consts.FieldBasePointLB:
					startX = 0f;
					startY = -p.BoxHeight;
					break;

				case Consts.FieldBasePointLM:
					startX = 0f;
					startY = -p.BoxHeight/2;
					break;

				case Consts.FieldBasePointLT:
					startX = 0f;
					startY = 0f;
					break;

				case Consts.FieldBasePointCB:
					startX = -p.BoxWidth/2;
					startY = -p.BoxHeight;
					break;

				case Consts.FieldBasePointCM:
					startX = -p.BoxWidth/2;
					startY = -p.BoxHeight/2;
					break;

				case Consts.FieldBasePointCT:
					startX = -p.BoxWidth/2;
					startY = 0f;
					break;

				case Consts.FieldBasePointRB:
					startX = -p.BoxWidth;
					startY = -p.BoxHeight;
					break;

				case Consts.FieldBasePointRM:
					startX = -p.BoxWidth;
					startY = -p.BoxHeight/2;
					break;

				case Consts.FieldBasePointRT:

					startX = -p.BoxWidth;
					startY = 0f;
					break;
				}

				return new Cartesian2D (startX, startY);
			}
		}
	}
}

