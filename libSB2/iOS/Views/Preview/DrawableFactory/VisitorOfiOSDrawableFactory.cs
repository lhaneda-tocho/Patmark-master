using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class VisitorOfiOSDrawableFactory : CommonFieldVisitor<FieldDrawable, Nil>
	{
		

		public override FieldDrawable Visit (HorizontalText.Constant target, Nil arg)
		{
			return new HorizontalTextDrawable (target);
		}

		public override FieldDrawable Visit (YVerticalText.Constant target, Nil arg)
		{
			return new YVerticalTextDrawable (target);
		}

		public override FieldDrawable Visit (XVerticalText.Constant target, Nil arg)
		{
			return new XVerticalTextDrawable (target);
		}

		public override FieldDrawable Visit (OuterArcText.Constant target, Nil arg)
		{
			return new OuterArcTextDrawable (target);
		}

		public override FieldDrawable Visit (InnerArcText.Constant target, Nil arg)
		{
			return new InnerArcTextDrawable (target);
		}

		public override FieldDrawable Visit (DataMatrix.Constant target, Nil arg)
		{
			return new DataMatrixDrawable (target);
		}

		public override FieldDrawable Visit (QrCode.Constant target, Nil arg)
		{
			return new QrCodeDrawable (target);
		}

		public override FieldDrawable Visit (Rectangle.Constant target, Nil arg)
		{
			return new RectangleDrawable (target);
		}

		public override FieldDrawable Visit (Circle.Constant target, Nil arg)
		{
			return new CircleDrawable (target);
		}

		public override FieldDrawable Visit (Triangle.Constant target, Nil arg)
		{
			return new TriangleDrawable (target);
		}

		public override FieldDrawable Visit (Line.Constant target, Nil arg)
		{
            return new LineDrawable (target);
		}

		public override FieldDrawable Visit (Bypass.Constant target, Nil arg)
		{
			return new BypassDrawable (target);
		}

		public override FieldDrawable Visit (Ellipse.Constant target, Nil arg)
		{
			return new EllipseDrawable (target);
		}
	}
}

