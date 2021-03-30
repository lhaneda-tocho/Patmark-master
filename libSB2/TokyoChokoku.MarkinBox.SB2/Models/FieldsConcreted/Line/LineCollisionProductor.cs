using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// Line フィールドの衝突判定ファクトリ
    /// </summary>
    public class LineCollisionProductor
    {
        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (Line.Constant target)
        {
            var p = target.Parameter;

            if (p.IsBezierCurve) {
                var start = new Position2D ((double)p.X, (double)p.Y);
                var center = new Position2D ((double)p.CenterX, (double)p.CenterY);
                var end = new Position2D ((double)p.EndX, (double)p.EndY);

                var bezierCollision = CreateBezier (start, center, end);
                var startCollision = CreatePoint (start);
                var centerCollision = CreatePoint (center);
                var endCollision = CreatePoint (end);

                var builder = CombinedCollision.CreateBuilder ();
                builder.AddCollision (LineCollisionKeys.StartPoint, startCollision);
                builder.AddCollision (LineCollisionKeys.CenterPoint, centerCollision);
                builder.AddCollision (LineCollisionKeys.EndPoint, endCollision);
                builder.AddCollision (LineCollisionKeys.Curve, bezierCollision);

                return builder.Build ();
            } else {
                var start = new Position2D ((double)p.X, (double)p.Y);
                var end = new Position2D ((double)p.EndX, (double)p.EndY);
                var center = (start + end) / 2;

                var bezierCollision = CreateBezier (start, center, end);
                var startCollision = CreatePoint (start);
                var centerCollision = CreatePoint (center);
                var endCollision = CreatePoint (end);

                var builder = CombinedCollision.CreateBuilder ();
                builder.AddCollision (LineCollisionKeys.StartPoint, startCollision);
                builder.AddCollision (LineCollisionKeys.CenterPoint, centerCollision);
                builder.AddCollision (LineCollisionKeys.EndPoint, endCollision);
                builder.AddCollision (LineCollisionKeys.Curve, bezierCollision);

                return builder.Build ();
            }
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (Line.Constant target)
        {
            var p = target.Parameter;

            if (p.IsBezierCurve) {
                var start  = new Position2D ((double)p.X      , (double)p.Y);
                var center = new Position2D ((double)p.CenterX, (double)p.CenterY);
                var end    = new Position2D ((double)p.EndX   , (double)p.EndY);

                var bezierCollision = CreateBezier (start, center, end);

                var builder = CombinedCollision.CreateBuilder ();
                builder.AddCollision (LineCollisionKeys.Curve      , bezierCollision);

                return builder.Build ();
            } else {
                var start  = new Position2D ((double)p.X      , (double)p.Y);
                var end    = new Position2D ((double)p.EndX   , (double)p.EndY);
                var center = (start + end) / 2;

                var bezierCollision = CreateBezier (start, center, end);

                var builder = CombinedCollision.CreateBuilder ();
                builder.AddCollision (LineCollisionKeys.Curve      , bezierCollision);

                return builder.Build ();
            }
        }

        static ICollision CreateBezier (Position2D start, Position2D center, Position2D end) {

            QuadBezierCurve curve = new QuadBezierCurve (
                start, center, end
            );

            return new LineStrip (curve, 30);
        }


        static ICollision CreateLine (LineParameter p) {
            return new LineStrip (
                new Position2D ((double)p.X      , (double)p.Y),
                new Position2D ((double)p.EndX   , (double)p.EndY)
            );
        }


        static ICollision CreatePoint (Position2D point) {
            return new PointCollision (point);
        }

    }
}

