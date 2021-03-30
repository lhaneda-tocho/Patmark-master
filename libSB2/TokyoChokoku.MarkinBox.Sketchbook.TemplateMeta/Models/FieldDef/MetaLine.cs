using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaLine : FieldDefinition
	{
		public static MetaLine Instance { get; } = new MetaLine ();



		public MetaLine () : base ("Line")
		{}



		protected override ParameterDefinition BuildParameter (ParameterDefinition.Builder builder)
		{
			return 
				builder
					.AddPower ("Power")
					.AddSpeed ("Speed")
                    .AddReverse ("Reverse")
                    .AddPause ("Pause")
                    .AddJogging("Jogging")
					.AddX     ("StartX")
					.AddY     ("StartY")
					.AddX     ("CenterX")
					.AddY     ("CenterY")
					.AddX     ("EndX")
					.AddY     ("EndY")
					.AddIsBezierCurve
					          ("IsBezierCurve")
					.Build();
			}
	}
}

