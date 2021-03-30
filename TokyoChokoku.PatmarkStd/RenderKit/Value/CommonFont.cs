using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.UnitKit;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
	public static class FontFactory
	{
		private static readonly Object theLock = new object();
        public static IFontFactory CoreFactory { get; private set; }

		/// <summary>
		/// 指定したファクトリをInjectionします．
		/// 2回目は InvalidOperationException となります．
		/// </summary>
		/// <returns>The inject.</returns>
		/// <param name="factory">Factory.</param>
		public static void Inject(IFontFactory factory)
		{
			if (factory == null)
				throw new NullReferenceException("Not allowed null.");
			lock (theLock)
			{
                if (CoreFactory == null)
                    CoreFactory = factory;
				else
					throw new InvalidOperationException("Not allowed re-inject.");
			}
		}

		/// <summary>
		/// 指定したファクトリをInjectionします．
		/// 2回目を呼び出しても何も起きません．
		/// </summary>
		/// <param name="factory">Factory.</param>
		public static void InjectNeeded(IFontFactory factory)
		{
			if (factory == null)
				throw new NullReferenceException("Not allowed null.");
            if (CoreFactory == null)
                Inject(factory);
        }


        public static CommonFont Create(string family, UnitPair<double, LengthUnit> size)
        {
            return CoreFactory.Create(family, size);
        }

		public static CommonFont Create(string family, double size, LengthUnit unit)
		{
            return Create(family, size.LengthUnit(unit));
		}

		public static CommonFont CreateWithPoint(string family, double pt)
		{
            return Create(family, pt, Unit.adobePt);
		}



        public static CommonFont CreateArial(double size, LengthUnit unit)
        {
            return Create("Arial", size, unit);
        }

		public static CommonFont CreateArialWithPoint(double pt)
		{
            return Create("Arial", pt, Unit.adobePt);
		}
    }

    public interface IFontFactory
    {
        CommonFont Create(string family, UnitPair<double, LengthUnit> size);
    }

    public abstract class CommonFont
    {
        public String Family { get; }
        public UnitPair<double, LengthUnit> Size { get; }


        protected CommonFont (string family, UnitPair<double, LengthUnit> size)
        {
            Family = family;
            Size = size;
        }

        public double SizePt {
            get {
                var u = Size.Unit;
                var v = Size.Value;
                return u.ToAdobePt(v);
            }
        }

		public CommonFont ReplaceSize(double size, LengthUnit unit)
		{
			return FontFactory.Create(Family, size, unit);
		}
		public CommonFont ReplaceSizePt(double size)
		{
            return FontFactory.CreateWithPoint(Family, size);
		}

		public abstract Size2D TextSize(string text);
        public Frame2D TextBounds(string text)
        {
            var s = TextSize(text);
            return Frame2D.Create(Pos2D.Zero(), s);
        }
    }
}
