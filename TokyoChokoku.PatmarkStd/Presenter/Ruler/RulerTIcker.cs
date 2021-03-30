using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Value;

namespace TokyoChokoku.Patmark.Presenter.Ruler
{
    public class RulerTicker
	{
        public readonly double X;
        public readonly double W;

		public readonly double MinPos;
		public readonly double MaxPos;

		public readonly double MinTick;
		public readonly double MaxTick;

		public readonly int    BlockCount;
        public readonly bool   IsReverse;

        public int Direction
        {
            get {
                return (IsReverse) ? -1 : +1;
            }
        }

        public double StartPos
        {
            get {
                if (IsReverse)
                    return X + (W - MaxPos);
                else
                    return X + MinPos;
            }
        }

        public double StartTick
        {
            get {
                if (IsReverse)
                    return MaxTick;
                else
                    return MinTick;
            }
        }

        public RulerTicker(TickInfo info)
        {
			info.Validate();
            X = info.X;
            W = info.W;
            IsReverse = (info.Direction < 0);
			var interval = info.Interval;
            {
                var min = info.MinTick;
                var max = info.MaxTick;
                var w = W;
                var wt = info.WTick;

                // 目盛り計算
                MinTick = GetMinTick(min, interval); // 最初のTickの値
                MaxTick = GetMaxTick(max, interval); // 最後のTickの値

                // 位置計算
                MinPos = GetMinPos(w, wt, min, MinTick);
                MaxPos = GetMaxPos(w, wt, max, MaxTick);
            }
			BlockCount = GetBlockCount(MinTick, MaxTick, interval);
        }

		public RulerTicks Tick()
		{

            var c  = BlockCount;
            var st = StartTick;
            var wt = MaxTick - MinTick;
            var sp = StartPos;
            var wp = (MaxPos - MinPos) * Direction;

            return new RulerTicks(TickFrom(c, st, sp, wt, wp));
        }

        static RulerTick[] TickFrom(int blockCount, double lt, double lp, double wt, double wp)
		{

			if (blockCount == 0)
				return new RulerTick[0];

			var ticks = new RulerTick[blockCount+1];
			for (int i = 0; i < blockCount+1; ++i)
			{
                var t = lt + (i * wt) / blockCount;
                var c = lp + (i * wp) / blockCount;
                ticks[i] = RulerTick.Init(t, c);
			}

			return ticks;
		}

        #region Util
        static double GetMinTick(double min, double interval)
		{
            double ans = Ceiling(min / interval) * interval;
            return ans;
		}

		static double GetMaxTick(double max, double interval)
		{
            double ans = Floor(max / interval) * interval;
            return ans;
		}

		static double GetMinPos(double w, double wt, double min, double minTick)
		{
			return w * ((minTick - min) / wt);
		}

		static double GetMaxPos(double w, double wt, double max, double maxTick)
		{
			return w + w * ((maxTick - max) / wt);
		}

        static int GetBlockCount(double minT, double maxT, double interval)
		{
			var dcount = (maxT - minT) / interval;
            if (double.IsInfinity(dcount) || double.IsNaN(dcount))
                throw new ArithmeticException("interval too small.");
			if (dcount > int.MaxValue)
				throw new ArithmeticException("tick count overflow.");
            Console.WriteLine(dcount);
			return (int)dcount;
		}
        #endregion
    }

    public struct TickInfo
    {
        /// <summary>
        /// 表示開始位置
        /// </summary>
        public double X;

		/// <summary>
		/// 表示サイズ．非負．
		/// </summary>
		public double W;

        /// <summary>
        /// 表示開始位置の目盛り値
        /// </summary>
        public double XTick;

        /// <summary>
        /// 表示サイズに対応するティック幅．非負．
        /// </summary>
        public double WTick;

        /// <summary>
        /// 目盛り間隔．負にすると逆向きになる．
        /// </summary>
        /// <value>The interval.</value>
        public double Interval;

        public double MinTick {
            get {
                return XTick;
            }
        }

        public double MaxTick {
            get {
                return XTick + WTick;
            }
        }

		/// <summary>
		/// +の時 右向き，
		/// -の時 左向きを表します
		/// </summary>
		/// <value>The direction.</value>
		public int Direction {
            get {
                var dd = Abs(Interval) / Interval;
                if (double.IsInfinity(dd) || double.IsNaN(dd))
                    throw new ArithmeticException("interval too small.");
                return (int) dd;
            }
        }

        public void Validate()
        {
            if (W <= 0)
                throw new ArgumentOutOfRangeException("W is must be > 0");
            if (WTick <= 0)
                throw new ArgumentOutOfRangeException("WTick must be > 0");
        }

        public RulerTicks Tick()
        {
            return new RulerTicker(this).Tick();
        }

        public static TickInfo Init(double x, double w)
        {
            TickInfo info;
            info.X = x;
            info.W = w;

            info.XTick    = 0.0;
            info.WTick    = 1.0;
            info.Interval = 0.2;
            return info;
        }
    }

    public struct RulerTick
    {
        /// <summary>
        /// 目盛り値
        /// </summary>
        /// <value>The graduation.</value>
        public double Graduation;

        /// <summary>
        /// 表示位置
        /// </summary>
        /// <value>The position.</value>
        public double Position;

        public static RulerTick Init(double t, double p)
        {
            RulerTick res;
            res.Graduation = t;
            res.Position   = p;
            return res;
        }

        public override string ToString()
        {
            return string.Format("[RulerTick(Grad={0}, Pos={1})]", Graduation, Position);
        }
    }

    public class RulerTicks : IEnumerable<RulerTick>
    {
        readonly RulerTick[] ticks;

        public int Count
        {
            get
            {
                return ticks.Length;
            }
        }
        public RulerTick this[int i]
        {
            get
            {
                return ticks[i];
            }
        }

        public RulerTicks(RulerTick[] ticks)
        {
            this.ticks = (RulerTick[])ticks.Clone();
        }

        public IEnumerator<RulerTick> GetEnumerator()
        {
            return ticks.Cast<RulerTick>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
