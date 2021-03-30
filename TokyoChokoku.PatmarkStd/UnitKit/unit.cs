using System;
namespace TokyoChokoku.Patmark.UnitKit
{

    public static class UnitPair 
    {
        public static UnitPair<T, U> WithUnit<T, U>(this T it, U unit) where U: IUnitType
        {
            return new UnitPair<T, U>(it, unit);
        }

        public static UnitPair<T, LengthUnit> LengthUnit<T>(this T it, LengthUnit unit)
        {
            return new UnitPair<T, LengthUnit>(it, unit);
        }
    }

    public struct UnitPair<T, U> where U: IUnitType
    {
        public UnitPair(T value, U unit)
		{
			Value = value;
            Unit  = unit;
        }

        public T Value { get; }
        public U Unit  { get; }


        public UnitPair<R, S> FlatMap<R, S>(Func<T, U, UnitPair<R, S>> f) where S: IUnitType
        {
            return f(Value, Unit);
        }

        public UnitPair<S, U> Map<S>(Func<T, U, S> f)
        {
            return FlatMap((v, u) =>
            {
                return new UnitPair<S, U>(f(v, u), u);
            });
        }

        public S Fold<S>(Func<T, U, S> f) 
        {
            return f(Value, Unit);
        }
    }


    public interface IUnitType {}

    public static class Unit
    {
        public static readonly Rad rad = new Rad();
        public static readonly Deg deg = new Deg();

        public static readonly MM      mm      = new MM();
        public static readonly Inch    inch    = new Inch();
        public static readonly AdobePt adobePt = new AdobePt();  
    }

    public abstract class AngleUnit: IUnitType
    {
        public abstract double ToDeg(double v);
        public abstract double ToRad(double v);
        public double To(AngleUnit unit, double v)
        {
            if (unit is Deg)
                return ToDeg(v);
            if (unit is Rad)
                return ToRad(v);
            throw new NotImplementedException(unit.ToString());
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }

    public class Rad : AngleUnit
    {
        public override double ToDeg(double v)
        {
            var rad = v;
            return rad * 180.0 / Math.PI;
        }

        public override double ToRad(double v)
        {
            var rad = v;
            return rad;
        }
    }

    public class Deg : AngleUnit
    {
        public override double ToDeg(double v)
        {
            var deg = v;
            return deg * 180.0 / Math.PI;
        }

        public override double ToRad(double v)
        {
            var deg = v;
            return deg * Math.PI / 180.0;
        }
    }




    #region Length
    public abstract class LengthUnit: IUnitType
    {
        public abstract double ToMM(double v);
        public abstract double ToAdobePt(double v);
        public abstract double ToInch(double v);

        public double To(LengthUnit unit, double v)
        {
            if (unit is MM)
                return ToMM(v);
            else if (unit is AdobePt)
                return ToAdobePt(v);
            else if (unit is Inch)
                return ToInch(v);
            throw new NotImplementedException(unit.ToString());
        }
    }

    public class MM: LengthUnit // ミリメータ
    {
        public override double ToMM(double v)
        {
            return v;
        }

        public override double ToInch(double v)
        {
            return v / 25.4;
        }

        public override double ToAdobePt(double v)
        {
            var inch = ToInch(v);
            return Unit.inch.ToAdobePt(inch);
        }
    }

	public class Inch : LengthUnit
	{
		public override double ToInch(double v)
		{
			return v;
		}

        public override double ToMM(double v)
        {
            return v * 25.4;
        }

        public override double ToAdobePt(double v)
        {
            return v * 72;
        }
	}

    public class AdobePt: LengthUnit
    {
        public override double ToAdobePt(double v)
        {
            return v;
        }

        public override double ToInch(double v)
        {
            return v / 72;
        }

        public override double ToMM(double v)
        {
            var inch = ToInch(v);
            return Unit.inch.ToMM(inch);
        }
    }
    #endregion
}
