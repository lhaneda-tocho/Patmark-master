using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public class LineStrip : ICollision
    {
        private readonly Position2D[] line;


        public LineStrip (Position2D start, Position2D end)
        {
            if (start == null || end == null) {
                throw new NullReferenceException ();
            }

            this.line = new Position2D[] {start, end};
        }


        public LineStrip (QuadBezierCurve curve, int divisionNumber)
        {
            if (curve == null) {
                throw new NullReferenceException ();
            }

            if (divisionNumber <= 0)
                throw new ArgumentOutOfRangeException (
                    "divisionNumber (= " + divisionNumber + ") is must be > 0.");


            line = new Position2D[divisionNumber];

            for (int i = 0; i < divisionNumber; i++) {
                
                double ratio = (double)i / (divisionNumber-1);
                line [i] = curve.CalculatePoint (ratio);

            }
        }


        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }




        public bool At (Homogeneous2D point)
        {
            var cartesianOrigin = point.ToCartesian ();

            for (int i = 0; i < line.Length - 1; i++) {

                bool isCollided =
                    CircleOnLineNotOnSegment (line[i].Cartesian, line[i+1].Cartesian, cartesianOrigin, 1);

                if (isCollided)
                    return true;
            }

            for (int i = 0; i < line.Length; i++) {

                bool isCollided =
                    CircleOnSegment (line[i].Cartesian, cartesianOrigin, 1);

                if (isCollided)
                    return true;
            }

            return false;
        }




        public bool OnCircle (Homogeneous2D origin, double radius)
        {
            var cartesianOrigin = origin.ToCartesian ();

            for (int i = 0; i < line.Length - 1; i++) {

                bool isCollided =
                    CircleOnLineNotOnSegment (line[i].Cartesian, line[i+1].Cartesian, cartesianOrigin, radius);

                if (isCollided)
                    return true;
            }

            for (int i = 0; i < line.Length; i++) {

                bool isCollided =
                    CircleOnSegment (line[i].Cartesian, cartesianOrigin, radius);

                if (isCollided)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 箱との衝突判定
        /// </summary>
        /// <returns><c>true</c>, if box was oned, <c>false</c> otherwise.</returns>
        public bool InBox (RectangleArea rect)
        {
            var sx = rect.X;
            var sy = rect.Y;
            var ex = sx + rect.Width;
            var ey = sy + rect.Height;

            bool collided = true;

            // 各点が箱の中にいるかを計算
            foreach (var point in line) {
                var m = sx <= point.X && point.X <= ex &&
                        sy <= point.Y && point.Y <= ey;
                collided &= m;
            }
            return collided;
        }



        private bool CircleOnLineNotOnSegment (Cartesian2D start, Cartesian2D end, Cartesian2D origin, double radius) {

            var startToEnd    = end    - start;
            var startToOrigin = origin - start;

            var endToOrigin   = origin - end;


            var startDot = startToEnd * startToOrigin;
            var endDot   = startToEnd * endToOrigin;


            if (startDot >= 0 && endDot <= 0) {
                var distance = 
                    Math.Abs (startToOrigin.OuterProduct (startToEnd.Normalize (2)));

                return 
                    distance <= radius;
            } else {
                return false;
            }
        }


        private bool CircleOnSegment (Cartesian2D segment, Cartesian2D origin, double radius) {
            
            var segmentToOrigin = origin - segment;

            var distance = Math.Abs (segmentToOrigin.Norm (2));

            return distance <= radius;
        }

    }
}

