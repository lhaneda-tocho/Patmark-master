using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class QuadBezierCurve
    {
        private readonly Cartesian2D controlPoint0;
        private readonly Cartesian2D controlPoint1;
        private readonly Cartesian2D controlPoint2;


        public QuadBezierCurve (
            Position2D controlPoint0, Position2D controlPoint1, Position2D controlPoint2) {
            this.controlPoint0 = controlPoint0.Cartesian;
            this.controlPoint1 = controlPoint1.Cartesian;
            this.controlPoint2 = controlPoint2.Cartesian;
        }


        /// <summary>
        /// ベジエ曲線上の1点を求めて返します．
        /// </summary>
        /// <param name="ratio"> 0以上 1以下の実数．0を下回る場合は0として， 1を上回る場合は1として扱います． </param>
        public Position2D CalculatePoint (double ratio) {
            if (ratio < 0) {
                ratio = 0;
            }

            if (ratio > 1) {
                ratio = 1;
            }

            double base0 =     (1 - ratio) * (1 - ratio);
            double base1 = 2 * (1 - ratio) * ratio;
            double base2 =     ratio       * ratio;

            var current =
                controlPoint0 * base0 +
                controlPoint1 * base1 +
                controlPoint2 * base2;

            return new Position2D (current);
        }

        public double CalculateX (double ratio) {
            if (ratio < 0) {
                ratio = 0;
            }

            if (ratio > 1) {
                ratio = 1;
            }

            double base0 =     (1 - ratio) * (1 - ratio);
            double base1 = 2 * (1 - ratio) * ratio;
            double base2 =     ratio       * ratio;

            var current =
                controlPoint0.X * base0 +
                controlPoint1.X * base1 +
                controlPoint2.X * base2;

            return current;
        }

        public double CalculateY (double ratio) {
            if (ratio < 0) {
                ratio = 0;
            }

            if (ratio > 1) {
                ratio = 1;
            }

            double base0 =     (1 - ratio) * (1 - ratio);
            double base1 = 2 * (1 - ratio) * ratio;
            double base2 =     ratio       * ratio;

            var current =
                controlPoint0.Y * base0 +
                controlPoint1.Y * base1 +
                controlPoint2.Y * base2;

            return current;
        }


        public double? XExtremePoint {
            get {
                double extreme =
                    (controlPoint0.X - controlPoint1.X) / 
                    (controlPoint0.X - 2 * controlPoint1.X + controlPoint2.X);

                if (0 <= extreme && extreme <= 1)
                    return extreme;
                else
                    return null;
            }
        }

        public double? YExtremePoint {
            get {
                double extreme =
                    (controlPoint0.Y - controlPoint1.Y) / 
                    (controlPoint0.Y - 2 * controlPoint1.Y + controlPoint2.Y);

                if (0 <= extreme && extreme <= 1)
                    return extreme;
                else
                    return null;
            }
        }

        public double MinX {
            get {
                double? extreme = XExtremePoint;

                if (extreme != null) {
                    return Math.Min (CalculateX ((double)extreme), Math.Min (controlPoint0.X, controlPoint2.X));
                } else {
                    return Math.Min (controlPoint0.X, controlPoint2.X);
                }
            }
        }

        public double MinY {
            get {
                double? extreme = YExtremePoint;

                if (extreme != null) {
                    return Math.Min (CalculateY ((double)extreme), Math.Min (controlPoint0.Y, controlPoint2.Y));
                } else {
                    return Math.Min (controlPoint0.Y, controlPoint2.Y);
                }
            }
        }


        public double MaxX {
            get {
                double? extreme = XExtremePoint;

                if (extreme != null) {
                    return Math.Max (CalculateX ((double)extreme), Math.Max (controlPoint0.X, controlPoint2.X));
                } else {
                    return Math.Max (controlPoint0.X, controlPoint2.X);
                }
            }
        }

        public double MaxY {
            get {
                double? extreme = YExtremePoint;

                if (extreme != null) {
                    return Math.Max (CalculateY ((double)extreme), Math.Max (controlPoint0.Y, controlPoint2.Y));
                } else {
                    return Math.Max (controlPoint0.Y, controlPoint2.Y);
                }
            }
        }


    }
}

