using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaCircle : FieldDefinition
	{
		public static MetaCircle Instance { get; } = new MetaCircle ();



		public MetaCircle () : base ("Circle")
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
					.AddArcRadius("Radius")
					.Build();
			}
	}
}

