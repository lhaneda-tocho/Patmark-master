using System;

using CoreGraphics;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class InnerArcTextDrawable : FieldDrawable
	{
		private readonly InnerArcTextParameter p;
        private readonly RootFieldTextNode     parsed;

        private readonly float  baseX;
        private readonly float  baseY;
        private readonly float  angle;

        private readonly Cartesian2D     circleOrigin;
        private readonly float           drawStartPhase;
        private readonly float           centerPhaseFromDrawStartPhase;


        public InnerArcTextDrawable (InnerArcText.Constant mbobject)
		{
			p = mbobject.Parameter;
            parsed = p.ParseText ();

            baseX = (float)p.X;
            baseY = (float)p.Y;
            angle = (float)p.Angle;


            drawStartPhase
                = (float) DrawStartPhase;

            centerPhaseFromDrawStartPhase
                = (float) UnitConverter.Radians.ToDegrees (0.5 * p.OuterTextLength / p.OuterRadius);

            circleOrigin
                = CircleOriginFromBasePoint;
		}



        public void Draw (FieldCanvas canvas)
		{
            var context = canvas.Context;
            var m = FieldCanvas.MakeIdentity ();
            m = FieldCanvas.Concat (m, canvas.CanvasViewMatrix);


			// ベースポイントの位置を合わせる
            m = FieldCanvas.Translate (m, baseX, baseY);
            m = FieldCanvas.Rotate    (m, -angle);


			// 円の中心が描画基準位置
			// 円の中心に座標系を移動．
            m = FieldCanvas.Translate (m, circleOrigin.X, circleOrigin.Y);


			// 描画の開始角度を求める
			// 開始角度から 時計回りに伸ばして行く．
            m = FieldCanvas.Rotate (m, drawStartPhase);


			// 描画
			DrawInnerArcText(
                canvas,
                m,
                parsed,
                p.OuterRadius,
                (float) p.Width,
                (float) p.Height,
                (float) p.Pitch,
                p.Mirrored);

		}


        private void DrawInnerArcText(
            FieldCanvas canvas,
            CGAffineTransform transform,
            RootFieldTextNode node,
            float outerRadius,
            float charWidth,
            float charHeight,
            float pitch,
            bool  mirrored)
        {
            var degPitch = (nfloat) UnitConverter.Radians.ToDegrees (pitch / outerRadius);
            // var degWitdh = (nfloat) UnitConverter.Radians.ToDegrees (charWidth/outerRadius);

            float distanceFromCenterToText = (-outerRadius + charHeight);

            transform = FieldCanvas.Rotate (transform, 90);

            if (mirrored) {
                transform = FieldCanvas.PhaseAxisMirror (transform, centerPhaseFromDrawStartPhase);
            }

            int elementCount = node.ElementCount ();

            canvas.DrawFieldTextNode (node, (index) => {
                var m = transform;
                m = FieldCanvas.Rotate     (m, degPitch * (elementCount - index - 1));
                m = FieldCanvas.Translate  (m, 0, distanceFromCenterToText);
                m = FieldCanvas.Rotate     (m, 180);
                m = FieldCanvas.Scale      (m,  charWidth, charHeight);
                m = FieldCanvas.Translate  (m, -1, 0);
                return m;
            });
        }


        public void DrawBorder (FieldCanvas canvas)
		{
            var m = canvas.CanvasViewMatrix;


            // ベースポイントの位置を合わせる
            m = FieldCanvas.Translate (m, baseX, baseY);
            m = FieldCanvas.Rotate    (m, -angle);


            // 円弧の中心位置まで移動
            m = FieldCanvas.Translate (m, circleOrigin.X, circleOrigin.Y);


			// 描画
			canvas.DrawArcBeam (
				m,
                DrawStartPhase,
                DrawEndPhase,
				p.InnerRadius, 
				p.OuterRadius - p.InnerRadius);
		}


        public void DrawBasePoint (FieldCanvas canvas)
        {
            canvas.DrawBasePoint (canvas.CanvasViewMatrix, baseX, baseY);
		}





		private Cartesian2D CircleOriginFromBasePoint {
			get {
				return new Cartesian2D (0, p.DistanceBetweenBasePointAndCircleCenter);
			}
		}


		private double DrawStartPhase 
		{
			get {
				switch (p.BasePoint) {
				default:
				case Consts.FieldBasePointLB:
				case Consts.FieldBasePointLM:
				case Consts.FieldBasePointLT:
					return -90;

				case Consts.FieldBasePointCB:
				case Consts.FieldBasePointCM:
				case Consts.FieldBasePointCT:
					return -90 + UnitConverter.Radians.ToDegrees(
						-p.OuterTextLength * 0.5 / p.OuterRadius
					);

				case Consts.FieldBasePointRB:
				case Consts.FieldBasePointRM:
				case Consts.FieldBasePointRT:
					return -90 + UnitConverter.Radians.ToDegrees(
						-p.OuterTextLength / p.OuterRadius
					);

				}
			}
		}


		private double DrawEndPhase
		{
			get {
				return DrawStartPhase
					+ UnitConverter.Radians.ToDegrees (p.OuterTextLength / p.OuterRadius);
			}
		}
	}
}

