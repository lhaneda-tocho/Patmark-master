using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class VisitorOfFieldCollisionFactory : CommonFieldVisitor <ICollision, Nil>
	{
        readonly bool precision;

        public VisitorOfFieldCollisionFactory (): this (false)
        {

        }

        public VisitorOfFieldCollisionFactory (bool precision)
        {
            this.precision = precision;
        }

		public override ICollision Visit (HorizontalText.Constant target, Nil arg)
		{
            if (precision)
                return HorizontalTextCollisionProductor.CreatePrecision (target);
            else
                return HorizontalTextCollisionProductor.Create (target);
		}

		public override ICollision Visit (YVerticalText.Constant target, Nil arg)
		{
            if (precision)
                return YVerticalTextCollisionProductor.CreatePrecision (target);
            else
                return YVerticalTextCollisionProductor.Create (target);
		}

		public override ICollision Visit (XVerticalText.Constant target, Nil arg)
		{
            if (precision)
                return XVerticalTextCollisionProductor.CreatePrecision (target);
            else
			    return XVerticalTextCollisionProductor.Create (target);
		}

		public override ICollision Visit (OuterArcText.Constant target, Nil arg)
		{
            if (precision)
			    return OuterArcTextCollisionProductor.CreatePrecision (target);
            else
                return OuterArcTextCollisionProductor.Create (target);
		}

		public override ICollision Visit (InnerArcText.Constant target, Nil arg)
		{
            if (precision)
			    return InnerArcTextCollisionProductor.CreatePrecision (target);
            else
                return InnerArcTextCollisionProductor.Create (target);
		}

		public override ICollision Visit (DataMatrix.Constant target, Nil arg)
        {
            if (precision)
                return DataMatrixCollisionProductor.CreatePrecision (target);
            else
                return DataMatrixCollisionProductor.Create (target);
		}

		public override ICollision Visit (QrCode.Constant target, Nil arg)
        {
            if (precision)
                return QrCodeCollisionProductor.CreatePrecision (target);
            else
                return QrCodeCollisionProductor.Create (target);
		}

		public override ICollision Visit (Rectangle.Constant target, Nil arg)
        {
            if (precision)
                return RectangleCollisionProductor.CreatePrecision (target);
            else
                return RectangleCollisionProductor.Create (target);
		}

		public override ICollision Visit (Circle.Constant target, Nil arg)
        {
            if (precision)
                return CircleCollisionProductor.CreatePrecision (target);
            else
                return CircleCollisionProductor.Create (target);
		}

		public override ICollision Visit (Triangle.Constant target, Nil arg)
        {
            if (precision)
                return TriangleCollisionProductor.CreatePrecision (target);
            else
                return TriangleCollisionProductor.Create (target);
		}

		public override ICollision Visit (Line.Constant target, Nil arg)
		{
            if (precision)
                return LineCollisionProductor.CreatePrecision (target);
            else
                return LineCollisionProductor.Create (target);
		}

		public override ICollision Visit (Bypass.Constant target, Nil arg)
        {
            if (precision)
                return BypassCollisionProductor.CreatePrecision (target);
            else
                return BypassCollisionProductor.Create (target);
		}

		public override ICollision Visit (Ellipse.Constant target, Nil arg)
        {
            if (precision)
                return EllipseCollisionProductor.CreatePrecision (target);
            else
                return EllipseCollisionProductor.Create (target);
		}
	}
}

