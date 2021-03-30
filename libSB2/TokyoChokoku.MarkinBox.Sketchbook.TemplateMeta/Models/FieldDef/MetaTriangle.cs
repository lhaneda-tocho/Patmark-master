using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaTriangle : FieldDefinition
	{
		public static MetaTriangle Instance { get; } = new MetaTriangle ();



		public MetaTriangle () : base ("Triangle")
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
					.AddX("HornX")
					.AddHeight("Height")
					.AddAspect("Aspect")
					.AddAngle("Angle")
					.Build();
			}
	}
}

