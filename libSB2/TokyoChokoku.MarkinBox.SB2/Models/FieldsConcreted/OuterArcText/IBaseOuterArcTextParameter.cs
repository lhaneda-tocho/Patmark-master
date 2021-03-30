using System;
using Unit = TokyoChokoku.MarkinBox.Sketchbook.UnitConverter;
using static System.Math;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public partial class IBaseOuterArcTextParameter
    {
        public RectangleArea Bounds {
            get {
                return CalcBoundingBox ();
            }
        }

        public RootFieldTextNode ParseText () {
            return FieldTextParser.ParseText (Text);
        }


        /// <summary>
        /// ロゴ，シリアル，カレンダーを含めた文字数です．
        /// </summary>
        private int Count ()
        {
            var root = ParseText ();
            return root.ElementCount ();
        }


        /// <summary>
        /// 文字幅
        /// HeightとAspectを用いて表現します．
        /// </summary>
        public decimal Width
        {
            get{
                return Height * (Aspect / 100);
            }
        }


        // Utility

        /// <summary>
        /// テキスト上部を通る円の半径
        /// </summary>
        public double   OuterRadius
        {
            get{
                return (double) Radius + (double) Height;
            }
        }

        /// <summary>
        /// テキスト下部を通る円の半径
        /// </summary>
        public double InnerRadius
        {
            get{
                return (double) Radius;
            }
        }

        public double InnerTextLength
        {
            get{
                if (Count() == 0)
                    return (double)Pitch;
                return (double) Pitch * (Count() - 1.0) + (double) Width;
            }
        }




        /// <summary>
        /// 基準点を通る円の半径
        /// </summary>
        public float DistanceBetweenBasePointAndCircleCenter
        {
            get{
                switch (BasePoint) {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointRB:
                    return (float) Radius;

                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointRM:
                    return (float) Radius + (float) Height/2;

                case Consts.FieldBasePointLT:
                case Consts.FieldBasePointCT:
                case Consts.FieldBasePointRT:
                    return (float) Radius + (float) Height;
                }
            }
        }



    public RectangleArea CalcBoundingBox ()
        {
            var startRad = -Unit.Degrees.ToRadians ((double)Angle);
            double phase = InnerTextLength / InnerRadius;

            RectangleArea area;


            // 円を原点にして BoundingBoxを求める
            if (Abs (phase) <= 2 * PI)
                // 部分ドーナツの 円弧中心 BoundingBoxを求める．
                // ドーナツの中心を原点に求めてくれます．
                area = PieceOfTorusBoundingBox (startRad, phase);
            else
                // 円になる場合．円の中心を原点に求めてくれます．
                area = CircleBoundingBox ();


            // 先ほど求めたBoundingBoxを フィールドの位置に合わせて移動

            var basePointToCircleOrigin = new Position2D (
                new Homogeneous2D (
                    0, -DistanceBetweenBasePointAndCircleCenter
                ).Rotate (-(double)Angle)
            );


            return new RectangleArea (
                area.X - basePointToCircleOrigin.X + (double)X,
                area.Y - basePointToCircleOrigin.Y + (double)Y,
                area.Width,
                area.Height
            );
        }

        RectangleArea PieceOfTorusBoundingBox (double startRad, double phaseRad)
        {
            double maxX, maxY, minX, minY;
            maxX = maxY = double.MinValue;
            minX = minY = double.MaxValue;


            // 頂点4つ以外で max, min になりうる箇所を求める
            // 外円弧が原因
            // 円弧オブジェクトは (angle == 0) の時 x軸から -90 [deg] 回転したところから 描画される
            if (Contains (startRad, phaseRad, PI / 2))
                maxX = +(double)Radius;
            if (Contains (startRad, phaseRad, PI))
                maxY = +(double)Radius;
            if (Contains (startRad, phaseRad, 3 * PI / 2))
                minX = -(double)Radius;
            if (Contains (startRad, phaseRad, 0))
                minY = -(double)Radius;

            // フィールドを構成する 4点を求める
            Cartesian2D [] points;
            {
                var innerRadius = (double)InnerRadius;
                var outerRadius = (double)OuterRadius;

                var xEntity = FamousVectors.XEntity.OfHomogeneous2D;

                var xInner = xEntity.Scale (innerRadius, innerRadius);
                var xOuter = xEntity.Scale (outerRadius, outerRadius);

                points = new Cartesian2D [] {
                    xInner.Rotate ( Unit.Radians.ToDegrees (startRad            - PI / 2) ).ToCartesian (),
                    xInner.Rotate ( Unit.Radians.ToDegrees (startRad + phaseRad - PI / 2) ).ToCartesian (),
                    xOuter.Rotate ( Unit.Radians.ToDegrees (startRad            - PI / 2) ).ToCartesian (),
                    xOuter.Rotate ( Unit.Radians.ToDegrees (startRad + phaseRad - PI / 2) ).ToCartesian ()
                };
            }

            // 4点から 最小値を求めていく
            foreach (Cartesian2D p in points) {
                maxX = Max (p.X, maxX);
                maxY = Max (p.Y, maxY);
                minX = Min (p.X, minX);
                minY = Min (p.Y, minY);
            }

            // 求められた 4つの 値を 返す
            return new RectangleArea (
                minX,
                minY,
                maxX - minX,
                maxY - minY
            );
        }

        RectangleArea CircleBoundingBox ()
        {
            double maxX, maxY, minX, minY;

            // 初めから決まっています
            maxX = (double)Radius;
            maxY = (double)Radius;
            minX = -(double)Radius;
            minY = -(double)Radius;

            // 求められた 4つの 値を 返す
            return new RectangleArea (
                minX,
                minY,
                maxX - minX,
                maxY - minY
            );
        }

        /// <summary>
        /// 指定された ラジアン値を 0 ~ 2π の範囲に写像します．
        /// 指定された値の大きさによっては失敗する可能性があります
        /// </summary>
        /// <returns>The angles.</returns>
        /// <param name="rad">RAD.</param>
        static double NormalizeAngles (double rad)
        {
            return
                rad - Floor (rad / (2 * PI)) * (2 * PI);
        }

        /// <summary>
        /// targetAngleRadが startRadから startRad + phaseの 範囲に存在するか調べます．
        /// </summary>
        /// <param name="pstartRad">Pstart RAD.</param>
        /// <param name="phase">Phase.</param>
        /// <param name="targetAngleRad">Target angle RAD.</param>
        static bool Contains (double pstartRad, double phase, double targetAngleRad)
        {
            // pstartRad を原点にする時の， targetAngleRadの値を 求めます
            // さらに 0 ~ 2π の範囲に 正規化します．
            var targetFromStartRad = NormalizeAngles (targetAngleRad - pstartRad);
            return targetFromStartRad <= phase;
        }
            
    }
}
