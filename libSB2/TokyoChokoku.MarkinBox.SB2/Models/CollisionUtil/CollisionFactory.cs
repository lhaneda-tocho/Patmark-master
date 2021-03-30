using System;
using MathNet.Numerics.LinearAlgebra;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// 三角や四角形などの衝突判定を作成できるファクトリクラスです．
    /// </summary>
	public class CollisionFactory
	{
        /// <summary>
        /// 座標変換行列
        /// </summary>
        /// <value>The transform.</value>
		public MatrixContext Transform {get;} = new MatrixContext();

		public ConvexHull NewTriangle(Homogeneous2D p0, Homogeneous2D p1, Homogeneous2D p2)
		{
			p0 = Transform.Multiply (p0);
			p1 = Transform.Multiply (p1);
			p2 = Transform.Multiply (p2);

			return ConvexHull.NewTriangle (p0, p1, p2);
		}

		public ConvexHull NewConvexRectangle(Homogeneous2D p0, Homogeneous2D p1, Homogeneous2D p2, Homogeneous2D p3)
		{
			p0 = Transform.Multiply (p0);
			p1 = Transform.Multiply (p1);
			p2 = Transform.Multiply (p2);
			p3 = Transform.Multiply (p3);

			return ConvexHull.NewRectangle (p0, p1, p2, p3);
		}

		public ConvexHull NewConvexHull(Homogeneous2D[] points)
		{
			points = Transform.Multiply (points);
			return new ConvexHull (points);
		}

		public TriangulateStrip NewFan(
			Homogeneous2D origin,
			double innerRadius, double startPhase,
			double outerRadius, double endPhase)
		{
            double BaseDegreesPerStep = CircleResolution.DegreesPerStep;

			// 内径・外径が逆の時は 例外．
			if (innerRadius > outerRadius) {
				throw new ArgumentException ("Inner radius must be less than Outer radius : inner radius=" + innerRadius + "outer radius=" + outerRadius);
			}

			// startPhaseと endPhaseについては startPhaseの方が小さくなるように修正．
			if (endPhase < startPhase) {
				var tmp = startPhase;
				startPhase = endPhase;
				endPhase = tmp;
			}

			var xEntity    = new Homogeneous2D (1, 0);
			var startInner = xEntity.Scale (innerRadius, innerRadius);
			var startOuter = xEntity.Scale (outerRadius, outerRadius);

			var openAngle  = endPhase - startPhase;
            openAngle      = (openAngle < 360d) ? openAngle
                                                : 360d;

            int needsStep  = CalculateNeedsStep (openAngle, BaseDegreesPerStep);
            double dps     = openAngle / needsStep;


            Homogeneous2D[] vertices = new Homogeneous2D[ (needsStep + 1) * 2 ];

			// 円弧を 短冊状に分割．
			for (int step = 0; step < needsStep + 1; step++) {
				double phase = step * dps;
				vertices [step * 2] = startInner.Rotate (phase);
				vertices [step * 2 + 1] = startOuter.Rotate (phase);
			}


			// 変換行列を適用．		
			Transform.PushMatrix ();
			Transform.Translate  (origin.CartesianX, origin.CartesianY);
			Transform.Rotate     (startPhase);
			vertices = Transform.Multiply(vertices);
			Transform.PopMatrix  ();

			return new TriangulateStrip (vertices);
		}


		public TriangulateStrip NewArcText(
			Homogeneous2D origin,
			double startRadius, double startPhase,
			double endRadius, double endPhase)
		{
			return NewFan (origin, startRadius, startPhase - 90, endRadius, endPhase - 90);
		}


        public ConvexHull NewCircle (Homogeneous2D origin, double radius) {
            if (radius < 0.0001) {
                radius = 0.0001;
            }

            int needsStep = CalculateNeedsStep (360, CircleResolution.DegreesPerStep);
            double rps = 2*Math.PI / needsStep;

            Homogeneous2D[] vertices = new Homogeneous2D[ needsStep ];

            for (int step = 0; step < vertices.Length; step++) {
                var currentPhase = rps * step;
                var x = radius * Math.Cos (currentPhase);
                var y = radius * Math.Sin (currentPhase);

                vertices [step] = new Homogeneous2D (x, y);
            }

            // 変換行列を適用．     
            Transform.PushMatrix ();
            Transform.Translate  (origin.CartesianX, origin.CartesianY);
            vertices = Transform.Multiply (vertices);
            Transform.PopMatrix  ();

            return new ConvexHull (vertices);
        }

        public PointCollision NewPoint (double x, double y)
        {
            var hgPos = Transform.Multiply (new Homogeneous2D(x, y));
            return new PointCollision (new Position2D (hgPos));
        }



        /// <summary>
        /// 基準分割数に近い大きさになるよう ステップ数を求める
        /// </summary>
        /// <returns>The needs step.</returns>
        /// <param name="dividee">Dividee.</param>
        /// <param name="perStep">Per step.</param>
        private int CalculateNeedsStep (double dividee, double perStep) {
            return 1 + (int)(dividee / perStep);
        }
	}
}

