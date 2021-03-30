using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaRectangle : FieldDefinition
	{
		public static MetaRectangle Instance { get; } = new MetaRectangle ();



		public MetaRectangle () : base ("Rectangle")
		{}



		protected override ParameterDefinition BuildParameter (ParameterDefinition.Builder builder)
		{
			return 
				builder
					.AddPower("Power")
					.AddSpeed("Speed")
                    .AddReverse ("Reverse")
                    .AddPause ("Pause")
					.AddBasePoint("BasePoint")
                    .AddJogging("Jogging")
					.AddX("X")
					.AddY("Y")
					.AddHeight("Height")
					.AddAspect("Aspect")
					.AddAngle("Angle")
					.Build();
			}
	}
}

