using System;
using static System.Math;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using MathNet.Numerics.LinearAlgebra.Double;

using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.RenderKit.Context;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
    public interface IShape
    {
        void StrokeShape(Canvas canvas);
        //void FillShape(Canvas canvas);
    }


    public class Fan: IShape
    {
        public struct P
        {
            public Pos2D  O;

            public double Thickness;
            public double OuterRadius;
            public double StartAngle; // [rad]
            public double OpenAngle;  // [rad]

            public double InnerRadius
            {
                get { return OuterRadius - Thickness; }
                set {
                    Thickness = OuterRadius - value;
                }
            }

            public double ThicknessFromInner
            {
                get { return Thickness; }
                set {
					OuterRadius = InnerRadius + value;
					Thickness = value;
                }
            }

            public double EndAngle // [rad]
            {
                get { return StartAngle + OpenAngle; }
                set { OpenAngle = value - StartAngle; }
            }

            public static P Init() {
                P p;
                p.O = Pos2D.Zero();
                p.Thickness    = 1;
                p.OuterRadius  = 2;
                p.OpenAngle    = PI / 2;
                p.StartAngle   = 0;
                return p;
            }
        }


        public Fan(P para) {
            Para = para;
        }

        public Fan(Func<P, P> init): this(init(P.Init())) {
        }


        private P Para { get; }


        public Pos2D  O { get{ return Para.O; } }

		public double Thickness    { get { return Para.Thickness;    } }
        public double OuterRadius  { get { return Para.OuterRadius;  } }
        public double InnterRadius { get { return Para.InnerRadius;  } }

        public double StartAngle   { get { return Para.StartAngle;   } }
        public double EndAngle     { get { return Para.EndAngle;     } }
        public double OpenAngle    { get { return Para.OpenAngle;    } }


        public void StrokeShape(Canvas canvas)
        {
            canvas.Use(()=>{
                canvas.Translate(O);
				canvas.StrokeLines(ToStrip());
            });
        }

        IStrip ToStrip()
        {
            var c = SplitCount();
            if (c < 12) c = 12;

            Pos2D[] points = new Pos2D[c * 2];

			for (int i = 0; i < c; i++)
			{
				var ca = StartAngle + i * OpenAngle / (c - 1);
				var cr = InnterRadius;
				var p = Pos2D.InitDistance(cr, ca, Unit.rad);
				points[0 + i] = p;
			}
			for (int j = 0; j < c; j++)
			{
                var ca = StartAngle + (c - 1 - j) * OpenAngle / (c - 1);
                var cr = OuterRadius;
				var p = Pos2D.InitDistance(cr, ca, Unit.rad);
				points[c + j] = p;
			}

            return new Strip(true, points);
        }

        int SplitCount() {
            var l = 4; // pt
            var r = Max(Abs(InnterRadius), Abs(OuterRadius));
            if (r < 0.00001) // TODO: 定数切り出し
                return 0;
            return (int) Ceiling(OpenAngle / (l / r));
        } 



    }
}