using System;
using System.Collections.Generic;
using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.TextData;

using TokyoChokoku.FieldModel;

namespace TokyoChokoku.Patmark.RenderKit.Context
{
    public abstract class Canvas
    {
        #region Properties
        private Stack<Affine2D> TransformStack { get; } = None.Run(()=>{
            var stack = new Stack<Affine2D>();
            var i = Affine2D.Entity();
            stack.Push(i);
            return stack;
        });

        public Affine2D Transform { 
            get {
                return TransformStack.Peek();
            }
            set {
                TransformStack.Pop();
                TransformStack.Push(value);
            }
		}
		#endregion

		public void ShowCharAt(Pos2D pos, char c, Size2D size)
		{
			ShowStringAt(pos, c.ToString(), size);
		}

        public void ShowStringAt(Pos2D pos, string text, Size2D size, double stride)
		{
            var move = Pos2D.InitX(stride);
			var p = pos;
			var len = text.Length;

			for (int i = 0; i < len; i++)
			{
				var c = text[i];
				ShowCharAt(p, c, size);
				p += move;
			}
        }

		public void StrokeLines(Lines lines)
		{
            foreach(var l in lines) {
                StrokeLine(l);
            }
		}


        #region control
        public void PushState()
        {
            PushStateNative();
            TransformStack.Push(Transform.Copy());
        }

        public void PopState()
		{
			PopStateNative();
            TransformStack.Pop();
		}

		public R Use<R>(Func<R> f)
		{
			PushState();
			R v = f();
			PopState();
			return v;
		}


		public R UseNative<R>(Func<R> f)
		{
			PushStateNative();
			R v = f();
			PopStateNative();
			return v;
		}


        public void Use(Action f)
        {
            PushState();
            f();
            PopState();
        }


        public void UseNative(Action f)
        {
            PushStateNative();
            f();
            PopStateNative();
        }
        #endregion

        public Canvas SetTransform(Affine2D newTransform)
        {
            Transform = newTransform;
            return this;
        }

        public Canvas Translate(double x, double y)
        {
            Translate(Pos2D.Init(x, y));
            return this;
        }
        public Canvas Translate(Pos2D p)
        {
            Transform *= Affine2D.Translate(p);
            return this;
        }

        public Canvas Scale(double x, double y)
        {
            Scale(Size2D.Init(x, y));
            return this;
        }
        public Canvas Scale(Size2D s)
        {
            Transform *= Affine2D.Scale(s);
            return this;
        }

        public Canvas Rotate(double angle, AngleUnit unit)
        {
            Transform *= Affine2D.Rotate(angle, unit);
            return this;
        }

        public void Stroke(IShape shape)
        {
            shape.StrokeShape(this);
		}

        public abstract void ShowStringAt(Pos2D pos, string text, Size2D size);
        public abstract void ShowStringMonoSpaceAt(Pos2D pos, string text, Size2D capsize, double stride);
        public abstract void ShowFieldTextMonoSpaceAt(Pos2D pos, FieldText text, Size2D capsize, double stride);

		public abstract void StrokeLines(IStrip path);
		public abstract void FillLines(IStrip path);
		public abstract void StrokeLine(Line line);

        public abstract void PushStateNative();
        public abstract void PopStateNative();

		public abstract void SetStrokeColor(CommonColor color);
		public abstract void SetFillColor  (CommonColor color);
	}

    /// <summary>
    /// Cavasの座標系 と View座標系との対応関係を管理します．
    /// </summary>
    public class CanvasViewTransform
    {
		#region Field
		private Lazy<Affine2D> CanvasViewMatrixLazy;

        /// <summary>
        /// Scale, Rot, Translate 全て掛け合わせたもの．
        /// </summary>
        /// <value>The canvas view.</value>
        public Affine2D CanvasView
        {
            get
            {
                return CanvasViewMatrixLazy.Value;
            }
        }

        public Affine2D Translate => Position.ToTransform();
        public Affine2D Scale => ScaleFactor.ToTransform();
        public Affine2D Rotate => Affine2D.Rotate(angle, Unit.rad);

        Pos2D position;
        double angle; // Rad 固定
        Size2D scalefactor;

        public Pos2D Position
		{
			get { return position; }
            set {
                position = value;
                Update();
            }
        }

        public double Angle
		{
			get { return angle; }
            set {
                angle = value;
                Update();
            }
		}

        public Size2D ScaleFactor
        {
            get { return scalefactor; }
            set {
                scalefactor = value;
                Update();
            }
        }

        #endregion

        public static CanvasViewTransform Init()
        {
            var p = Pos2D.Init(0, 0);
            var s = Size2D.Init(1, 1);
            var a = 0d;
            return new CanvasViewTransform(p, s, a);
        }

        public static CanvasViewTransform Init(Pos2D pos, Size2D scale, double rot)
        {
            return new CanvasViewTransform(pos, scale, rot);
        }

        CanvasViewTransform(Pos2D pos, Size2D scale, double rot) {
            position    = pos;
            scalefactor = scale;
            angle       = rot;
            Update();
        }

        void Update()
        {
            CanvasViewMatrixLazy = new Lazy<Affine2D>(() =>
            {
                var t = position.ToTransform();
                var s = scalefactor.ToTransform();
                var r = Affine2D.Rotate(angle, Unit.rad);
                return t * s * r;
            });
        }

        public CanvasViewTransform Copy()
        {
            return Init(position, scalefactor, angle);
        }
    }
}
