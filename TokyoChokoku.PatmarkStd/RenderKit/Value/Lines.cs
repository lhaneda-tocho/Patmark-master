using System;
using static System.Math;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using TokyoChokoku.Patmark.UnitKit;

using TokyoChokoku.Patmark.RenderKit.Transform;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
    public class Line
    {
        public Pos2D Start { get; }
        public Pos2D End   { get; }

        public Line(Pos2D start, Pos2D end)
        {
            Start = start;
            End = end;
        }

        public Line(double sx, double sy, double ex, double ey): this(Pos2D.Init(sx, sy), Pos2D.Init(ex, ey))
        {}

        public static Line operator* (Affine2D t, Line r)
        {
            var ns = t * r.Start;
            var ne = t * r.End;
            return new Line(ns, ne);
        }
    }

    public class Lines: IEnumerable<Line>
	{
        readonly Line[] data;
        public int LineCount { get { return data.Length; } }
        public Line this[int index] { get{ return data[index]; } }

        public Lines(Line[] data)
        {
            this.data = (Line[]) (data ?? throw new NullReferenceException()).Clone();
        }

        public static Lines operator* (Affine2D t, Lines r)
        {
            var na = new Line[r.LineCount];
            for (int i = 0; i < r.LineCount; i++) {
                na[i] = t * r[i];
            }
            return new Lines(na);
        }



        public IEnumerator<Line> GetEnumerator() {
            return data.Cast<Line>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class GridSource {
        public UnitPair<Double , LengthUnit> Stride { get; set; } = 1.0.LengthUnit(Unit.mm);
        public UnitPair<Pos2D  , LengthUnit> O      { get; set; } = Pos2D.Zero().LengthUnit(Unit.mm);
        public UnitPair<Frame2D, LengthUnit> Range  { get; set; } = Frame2D.Zero().LengthUnit(Unit.mm);

        public Lines Bake(LengthUnit unit)
        {
            var s = Stride.Fold((v, u) => { return u.To(unit, v); });
            var o = O.Fold((v, u) => { return Pos2D.Init(u.To(unit, v.X), u.To(unit, v.Y)); });  // FIXME: Pos2Dに map 関数を与える
			var r = Range.Fold((v, u) =>
            {
                return Frame2D.Create( // FIXME: Frame2Dに map 関数を与える
					u.To(unit, v.X),
                    u.To(unit, v.Y),
                    u.To(unit, v.W),
                    u.To(unit, v.H)
                );
            });


            var start  = GetGridStart(s, o, r);
            var counts = GetXYLineCounts(s, o, r);
            var xcounts = counts[0];
            var ycounts = counts[1];

            if (xcounts + (long)ycounts > int.MaxValue)
                throw new ArithmeticException(); // TODO: 例外処理が必要かどうかを検討する．

            var lines = new Line[xcounts + ycounts];
            for (int i = 0; i < xcounts; i++)
			{
                var x  = i * s + start.X;
                var sy = r.Top;
                var ey = r.Bottom;
                lines[i] = new Line(x, sy, x, ey);
            }
            for (int j = 0; j < ycounts; j++)
            {
                var y = j * s + start.Y;
                var sx = r.Left;
                var ex = r.Right;
                lines[j + xcounts] = new Line(sx, y, ex, y);
            }

			//Console.WriteLine("sx = {0}, sy = {1}", s, s);
			//Console.WriteLine("cx = {0}, cy = {1}", xcounts, ycounts);

            return new Lines(lines);
        }

		// グリッドの描画開始位置(上端，左端)
		// 引数は全て同じ単位の数値
		// strideは 0より大きい必要がある
		static Pos2D GetGridStart(double stride, Pos2D o, Frame2D range)
		{
			if (Abs(stride) < 0.000001)
				throw new ArithmeticException();
            
            var ox = range.X - o.X;
            var oy = range.Y - o.Y;
            var s = stride;

            var sx = Ceiling(ox / s) * s + o.X;
            var sy = Ceiling(oy / s) * s + o.Y;

            return Pos2D.Init(sx, sy);
        }

		// 描画する線の本数
		// 引数は全て同じ単位の数値
		// strideは 0.000001 以上必要．
		static int[] GetXYLineCounts(double stride, Pos2D o, Frame2D range)
        {
            if (Abs(stride) < 0.000001)
                throw new ArithmeticException();

			var start = GetGridStart(stride, o, range);

            var sxToRight  = range.Right  - start.X;
            var syToBottom = range.Bottom - start.Y;

            var cx = (int) Floor(sxToRight  / stride) + 1; // 初期位置にも描画できるので +1 
            var cy = (int) Floor(syToBottom / stride) + 1; // 初期位置にも描画できるので +1

			return new int[] {cx, cy};
        }
    }
}
