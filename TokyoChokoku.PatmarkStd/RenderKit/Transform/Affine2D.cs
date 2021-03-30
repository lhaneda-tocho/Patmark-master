using System;
using MathNet.Numerics.LinearAlgebra.Double;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.UnitKit;
namespace TokyoChokoku.Patmark.RenderKit.Transform
{
    public struct Affine2D
    {
        // 1列目
        public Dir2D RX;
        public double RXX { get { return RX.X; } set { RX.X = value; } }
        public double RYX { get { return RX.Y; } set { RX.Y = value; } }

        // 2列目
        public Dir2D RY;
        public double RXY { get { return RY.X; } set { RY.X = value; } }
        public double RYY { get { return RY.Y; } set { RY.Y = value; } }

        // 3列目
        public Pos2D Pos;
        public double PosX { get { return Pos.X; } set { Pos.X = value; } }
        public double PosY { get { return Pos.Y; } set { Pos.Y = value; } }

        #region Factory
        public static Affine2D InitColumns(Dir2D rx, Dir2D ry, Pos2D pos)
        {
            Affine2D t;
            t.RX = rx;
            t.RY = ry;
            t.Pos = pos;

            return t;
        }

        public static Affine2D InitColumns(double rxx, double ryx, double rxy, double ryy, double px, double py)
        {
            Affine2D t;

            t.RX.X = rxx;
            t.RX.Y = ryx;

            t.RY.X = rxy;
            t.RY.Y = ryy;

            t.Pos.X = px;
            t.Pos.Y = py;

            return t;
        }

        public static Affine2D Entity()
        {
            return InitColumns(Dir2D.InitX(), Dir2D.InitY(), Pos2D.Zero());
        }

        public static Affine2D Scale(Size2D s)
        {
            return InitColumns(Dir2D.InitX(s.W), Dir2D.InitY(s.H), Pos2D.Zero());
        }

        public static Affine2D Scale(double x, double y)
        {
            return InitColumns(Dir2D.InitX(x), Dir2D.InitY(y), Pos2D.Zero());
        }

        public static Affine2D Translate(Pos2D p)
        {
            return InitColumns(Dir2D.InitX(), Dir2D.InitY(), Pos2D.Init(p.X, p.Y));
        }

        public static Affine2D Translate(double x, double y)
        {
            return InitColumns(Dir2D.InitX(), Dir2D.InitY(), Pos2D.Init(x, y));
        }

        public static Affine2D Rotate(double theta, AngleUnit unit) {
            var t1 = theta;
            var t2 = theta + Unit.deg.To(unit, 90.0);

            return InitColumns(
                Dir2D.InitDistance(1, t1, unit),
                Dir2D.InitDistance(1, t2, unit),
                Pos2D.Zero()
            );
        }
		#endregion

		public Affine2D DeleteTranslation()
		{
            return InitColumns(
                RX,
                RY,
                Pos2D.Zero()
			);
		}

        public Tuple<Pos2D, Affine2D> FactorGlobalPos()
        {
            return new Tuple<Pos2D, Affine2D>(
                Pos,
                InitColumns(RX, RY, Pos2D.Zero())
            );
        }

        public Tuple<Affine2D, Size2D> FactorLocalScale()
        {
            var sx = RX.Length;
            var sy = RY.Length;
            return new Tuple<Affine2D, Size2D>(
                InitColumns(
                    RX / Dir2D.Fill(sx),
                    RY / Dir2D.Fill(sy),
                    Pos
                ),
                Size2D.Init(sx, sy)
            );
        }

        public void CopyTo(Affine2D dest)
		{
            dest.RX = RX;
            dest.RY = RY;
            dest.Pos= Pos;
		}

        public Affine2D Copy() {
            return InitColumns(RX, RY, Pos);
        }

        public double[] ToColumnOrderArray() {
            return new double[] {
                RX.X, RX.Y, RY.X, RY.Y, Pos.X, Pos.Y
            };
        }

        public float[] ToRowOrderArrayAsFloat9() {
            return new float[] {
                (float)RX.X, (float)RY.X, (float)Pos.X, 
                (float)RX.Y, (float)RY.Y, (float)Pos.Y,
                0, 0, 1
            };
        }

        public double Determine {
            get {
                var a = RXX; var b = RXY;
                var c = RYX; var d = RYY;
                return a*d - b*c;
            }
        }

        public Affine2D Inverse {
            get
            {
                var a = RXX; var b = RXY; var x = PosX;
                var c = RYX; var d = RYY; var y = PosY;
                var det = Determine;
                return InitColumns(
                    +d / det, -c / det, // 1列目
                    -b / det, +a / det, // 2列目
                    (-d * x + b * y) / det, (c * x - a * y) / det // 3列目
                );
            }
        }

        public Matrix ToMatrix()
		{
			var storage = new double[] {
				RX.X, RX.Y, 0.0, RY.X, RY.Y, 0.0, Pos.X, Pos.Y, 1.0
			};
            return new DenseMatrix(3, 3, storage);
        }

		public static Affine2D operator* (Affine2D a, Affine2D b)
		{
            var m1 = a.ToMatrix();
            var m2 = b.ToMatrix();
            var n = m1 * m2;
            return InitColumns(
                n[0,0], n[1,0],
                n[0,1], n[1,1],
                n[0,2], n[1,2]
            );
        }


		public static IStrip operator *(Affine2D t, IStrip r)
		{
			Pos2D[] buf = new Pos2D[r.Count];
			for (int i = 0; i < r.Count; i++)
			{
				buf[i] = t * r[i];
			}
			return new Strip(r.IsLoop, buf);
		}

        public override string ToString()
        {
            return string.Format("[Affine2D: RX={0}, RY={1}, Pos={2}]", RX, RY, Pos);
        }

    }
}
