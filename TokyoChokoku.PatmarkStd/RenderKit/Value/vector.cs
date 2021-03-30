using System;
using static System.Math;
using TokyoChokoku.Patmark.UnitKit;
using MathNet.Numerics.LinearAlgebra.Double;
using TokyoChokoku.Patmark.RenderKit.Transform;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
	/// <summary>
	/// 位置
	/// </summary>
    public struct Pos2D
	{
        public double X;
		public double Y;

        public double Length 
        {
            get
            {
                return Sqrt(X * X + Y * Y);
            }
        }

		public static Pos2D Zero()
		{
			return new Pos2D();
		}

        public static Pos2D InitX(double x = 1)
		{
            return Init(x, 0);
		}

        public static Pos2D InitY(double y = 1)
		{
            return Init(0, y);
		}

		public static Pos2D InitDistance(double r, double theta, AngleUnit unit)
		{
			var rad = unit.ToRad(theta);
			return Init(r * Cos(rad), r * Sin(rad));
		}

        public static Pos2D Init(double x, double y)
        {
            Pos2D p;
            p.X = x;
            p.Y = y;
            return p;
        }

        public static Pos2D Fill(double v)
        {
            return Init(v, v);
        }

        public void Set(double x, double y)
        {
			X = x;
			Y = y;
		}

		public Vector ToHomogeneous()
		{
			return new DenseVector(new double[] { X, Y, 1 });
		}

		public Vector ToCartesian()
		{
			return new DenseVector(new double[] { X, Y });
		}

        public Affine2D ToTransform() {
            return Affine2D.Translate(this);
        }

        public static Pos2D operator* (Affine2D a, Pos2D b)
        {
            var m = a.ToMatrix();
            var v = b.ToHomogeneous();
            var n = m * v;
            return Init(n[0], n[1]);
		}

		public static Pos2D operator +(Pos2D a, Pos2D b)
		{
			return a.Plus(b);
		}

		public static Pos2D operator -(Pos2D a, Pos2D b)
		{
			return a.Minus(b);
		}

		public static Pos2D operator *(Pos2D a, Pos2D b)
		{
            return a.Times(b);
		}

        public static Pos2D operator /(Pos2D a, Pos2D b)
        {
            return a.Div(b);
        }

		public Pos2D Plus(Pos2D r)
		{
			var x = X + r.X;
			var y = Y + r.Y;
			return Init(x, y);
		}

		public Pos2D Minus(Pos2D r)
		{
			var x = X - r.X;
			var y = Y - r.Y;
			return Init(x, y);
		}

		public Pos2D Times(Pos2D r)
		{
			var x = X * r.X;
			var y = Y * r.Y;
			return Init(x, y);
		}

        public Pos2D Div(Pos2D r)
        {
            var x = X / r.X;
            var y = Y / r.Y;
            return Init(x, y);
        }

        public double Dot(Pos2D r) {
            var t = Times(r);
            return t.X + t.Y;
        }

        public override string ToString()
        {
            return string.Format("[Pos2D: X={0}, Y={1}]", X, Y);
        }
    }


    /// <summary>
    /// 方向
    /// </summary>
    public struct Dir2D
    {
        public double X;
        public double Y;

		public double Length
		{
			get
			{
				return Sqrt(X * X + Y * Y);
			}
		}

		public static Dir2D Zero()
		{
			return new Dir2D();
		}

		public static Dir2D InitX(double x = 1)
		{
			return Init(x, 0);
		}

		public static Dir2D InitY(double y = 1)
		{
			return Init(0, y);
		}

        public static Dir2D InitDistance(double r, double theta, AngleUnit unit)
        {
            var rad = unit.ToRad(theta);
            return Init(r * Cos(rad), r * Sin(rad));
        }

        public static Dir2D Init(double dx, double dy)
        {
            Dir2D d;
            d.X = dx;
            d.Y = dy;
            return d;
		}

        public static Dir2D Fill(double v)
        {
            return Init(v, v);
        }

		public void Set(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector ToHomogeneous()
		{
			return new DenseVector(new double[] { X, Y, 0 });
		}

		public Vector ToCartesian()
		{
			return new DenseVector(new double[] { X, Y });
		}

        public static Dir2D operator* (Affine2D a, Dir2D b)
		{
			var m = a.ToMatrix();
			var v = b.ToHomogeneous();
			var n = m * v;
            return Init(n[0], n[1]);
		}

		public static Dir2D operator +(Dir2D a, Dir2D b)
		{
			return a.Plus(b);
		}

		public static Dir2D operator -(Dir2D a, Dir2D b)
		{
			return a.Minus(b);
		}

		public static Dir2D operator *(Dir2D a, Dir2D b)
		{
			return a.Times(b);
		}

		public static Dir2D operator /(Dir2D a, Dir2D b)
		{
			return a.Div(b);
		}

		public Dir2D Plus(Dir2D r)
		{
			var x = X + r.X;
			var y = Y + r.Y;
			return Init(x, y);
		}

		public Dir2D Minus(Dir2D r)
		{
			var x = X - r.X;
			var y = Y - r.Y;
			return Init(x, y);
		}

		public Dir2D Times(Dir2D r)
		{
			var x = X * r.X;
			var y = Y * r.Y;
			return Init(x, y);
		}

		public Dir2D Div(Dir2D r)
		{
			var x = X / r.X;
			var y = Y / r.Y;
			return Init(x, y);
		}

		public double Dot(Dir2D r)
		{
			var t = Times(r);
			return t.X + t.Y;
		}

		public override string ToString()
		{
			return string.Format("[Dir2D: X={0}, Y={1}]", X, Y);
		}
    }

    /// <summary>
    /// サイズ
    /// </summary>
    public struct Size2D
    {
        public double W;
        public double H;

        public double AspectW
        {
            get
            {
                return W / H;
            }
        }

        public double AspectH
        {
            get
            {
                return H / W;
            }
        }

		public double Length
		{
			get
			{
				return Sqrt(W * W + H * H);
			}
		}

        public double SX {
            get { return W; }
            set { W = value; }
        }

        public double SY {
            get { return H;  }
            set { H = value; }
        }

		public Dir2D DirX { get { return Dir2D.InitX(SX); } }
		public Dir2D DirY { get { return Dir2D.InitY(SY); } }

		public static Size2D Zero()
		{
            return new Size2D();
		}

        public static Size2D One() {
            return Init(1, 1);
        }

        public static Size2D Init(double w, double h)
        {
            Size2D s;
            s.W = w;
            s.H = h;
            return s;
        }

        public static Size2D Fill(double v)
        {
            return Init(v, v);
        }

        public void Set(double w, double h)
		{
			W = w;
			H = h;
        }

        public Affine2D ToTransform()
        {
            return Affine2D.Scale(this);
		}

		public override string ToString()
		{
			return string.Format("[Size2D: W={0}, H={1}]", W, H);
		}


        public static Size2D operator +(Size2D a, Size2D b)
        {
            return a.Plus(b);
        }

        public static Size2D operator -(Size2D a, Size2D b)
        {
            return a.Minus(b);
        }

        public static Size2D operator *(Size2D a, Size2D b)
        {
            return a.Times(b);
        }

        public static Size2D operator /(Size2D a, Size2D b)
        {
            return a.Div(b);
        }

        public Size2D Plus(Size2D r)
        {
            var x = SX + r.SX;
            var y = SY + r.SY;
            return Init(x, y);
        }

        public Size2D Minus(Size2D r)
        {
            var x = SX - r.SX;
            var y = SY - r.SY;
            return Init(x, y);
        }

        public Size2D Times(Size2D r)
        {
            var x = SX * r.SX;
            var y = SY * r.SY;
            return Init(x, y);
        }

        public Size2D Div(Size2D r)
        {
            var x = SX / r.SX;
            var y = SY / r.SY;
            return Init(x, y);
        }

        public double Dot(Size2D r)
        {
            var t = Times(r);
            return t.SX + t.SY;
        }
    }

    /// <summary>
    /// 矩形
    /// </summary>
    public class Frame2D
    {
        public Pos2D  Pos;
        public Size2D Size;

        public double X { get { return Pos.X;  } set { Pos.X = value;  } }
        public double Y { get { return Pos.Y;  } set { Pos.Y = value;  } }

        public double W { get { return Size.W; } set { Size.W = value; } }
        public double H { get { return Size.H; } set { Size.H = value; } }

        public double Top
        {
            get { return Y; }
            set {
                var y = value;
                var h = Bottom - value;
                Y = y;
                H = h;
            }
        }

        public double Bottom
        {
            get { return Y + H; }
            set {
                var h = value - Top;
                H = h;
            }
        }

        public double Left
		{
			get { return X; }
			set {
				var x = value;
				var w = Right - value;
				X = x;
				W = w;
			}
		}

        public double Right
        {
            get { return X + W; }
            set {
                var w = value - Left;
				W = w;
            }
        }

        public Pos2D Center
        {
            get
            {
                var cx = (Left + Right) / 2;
                var cy = (Top + Bottom) / 2;
                return Pos2D.Init(cx, cy);
            }
        }

        public RectStrip2D ToStrip()
        {
            return RectStrip2D.Frame(this);
        }

        public Frame2D Copy()
        {
            var data = new Frame2D();
            data.Pos = Pos;
            data.Size = Size;
            return data;
        }

        public static Frame2D Zero()
        {
            var f = new Frame2D();
            return f;
        }

        public static Frame2D One(Pos2D pos)
		{
            return Create(pos, Size2D.Init(1, 1));
        }

        public static Frame2D Bounds(Size2D size)
        {
            return Create(Pos2D.Zero(), size);
		}

		public static Frame2D Bounds(double w, double h)
		{
            return Create(Pos2D.Zero(), Size2D.Init(w, h));
		}

        public static Frame2D Create(Pos2D pos, Size2D size)
        {
            var f = new Frame2D();
            f.Pos = pos;
            f.Size = size;
            return f;
        }

        public static Frame2D Create(double x, double y, double w, double h)
        {
            var f = new Frame2D();
            f.X = x;
            f.Y = y;
            f.W = w;
            f.H = h;
            return f;
        }



        public override string ToString()
        {
            return string.Format("[Frame2D: X={0}, Y={1}, W={2}, H={3}]", X, Y, W, H);
        }
    }

    /// <summary>
    /// 矩形(パス形式)
    /// </summary>
    public class RectStrip2D: BaseStrip
	{
        public override bool IsLoop{
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        public Pos2D[] Path { get{ return path; } }
        public Pos2D[] CopyPath { get { return (Pos2D[]) Path.Clone(); } }

        public override Pos2D this[int index]
        {
            get
            {
                return path[index];
            }
            set
            {
                path[index] = value;
            }
        }

        public RectStrip2D(Pos2D p0, Pos2D p1, Pos2D p2, Pos2D p3): base(new Pos2D[] { p0, p1, p2, p3 })
        {
        }

        public static RectStrip2D Create(Pos2D p0, Pos2D p1, Pos2D p2, Pos2D p3) {
            return new RectStrip2D(p0, p1, p2, p3);
        }

		public static RectStrip2D Create(
			double w0, double h0,
			double w1, double h1,
			double w2, double h2,
			double w3, double h3)
		{
			var p = new RectStrip2D(
	            Pos2D.Init(w0, h0),
	            Pos2D.Init(w1, h1),
				Pos2D.Init(w2, h2),
                Pos2D.Init(w3, h3));
			return p;
		}

        public static RectStrip2D Frame(Frame2D frame) {
            var t = frame.Top;
            var b = frame.Bottom;
            var l = frame.Left;
            var r = frame.Right;

			return Create(
				Pos2D.Init(l, t),
				Pos2D.Init(l, b),
				Pos2D.Init(r, b),
				Pos2D.Init(r, t)
            );
        }
    }


    public abstract class CommonColor // グレースケールの対応は必要？
	{
        #region Init
        public static CommonColor FromByteRGBA(byte r, byte g, byte b, byte a = 255)
		{
            return new RGBAColor(ByteRGBA.Init(r, g, b, a));
		}

        public static CommonColor FromFloatRGBA(float r, float g, float b, float a = 255)
		{
            return new RGBAColor(FloatRGBA.Init(r, g, b, a));
		}

        public static CommonColor CreateRGBA(ByteRGBA v)
		{
			return new RGBAColor(v);
		}

		public static CommonColor CreateRGBA(FloatRGBA v)
		{
			return new RGBAColor(v);
		}

		CommonColor(FloatRGBA frgba)
        {
            FloatRGBA = frgba;
        }
        #endregion

        public FloatRGBA FloatRGBA { get; }
		public ByteRGBA  ByteRGBA  { get { return FloatRGBA.ToByte(); } }

        public float R { get { return FloatRGBA.R; } }
		public float G { get { return FloatRGBA.G; } }
		public float B { get { return FloatRGBA.B; } }
		public float A { get { return FloatRGBA.A; } }


        class RGBAColor: CommonColor
        {
            internal RGBAColor(FloatRGBA v): base(v) {}
            internal RGBAColor(ByteRGBA  v): base(v.ToFloat()) {}
        }
    }

    #region Color structure
    public struct FloatRGBA
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public static FloatRGBA Init(float r, float g, float b, float a = 1) {
            FloatRGBA c;
            c.R = r;
            c.G = g;
            c.B = b;
            c.A = a;
            c.normalize();
            return c;
        }

        public ByteRGBA ToByte()
        {
            float s = 255;
            var c = this;
            c.normalize();
            return ByteRGBA.Init(
                (byte)(c.R * s), 
                (byte)(c.G * s), 
                (byte)(c.B * s),
                (byte)(c.A * s)
            );
        }

		public void normalize()
		{
            float min = 0.0f;
            float max = 1.0f;
			R = Max(min, Min(max, R));
			G = Max(min, Min(max, G));
			B = Max(min, Min(max, B));
			A = Max(min, Min(max, A));
        }
    }

    public struct ByteRGBA
	{
		public byte R;
		public byte G;
		public byte B;
        public byte A;


        public static ByteRGBA Init(byte r, byte g, byte b, byte a = 255) {
            ByteRGBA c;
            c.R = r;
            c.G = g;
            c.B = b;
            c.A = a;
            return c;
        }

        public FloatRGBA ToFloat() {
            float s = 1.0f / 255.0f;
            var c = this;
            c.normalize();
            return FloatRGBA.Init(
                (c.R * s), 
                (c.G * s), 
                (c.B * s),
                (c.A * s)
            );
        }

        public void normalize() {
            byte min = 0;
            byte max = 255;
            R = Max(min, Min(max, R));
            G = Max(min, Min(max, G));
            B = Max(min, Min(max, B));
            A = Max(min, Min(max, A));
        }
    }
    #endregion
}
