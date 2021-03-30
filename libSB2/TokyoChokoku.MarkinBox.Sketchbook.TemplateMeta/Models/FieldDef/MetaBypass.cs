using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaBypass : FieldDefinition
	{
		public static MetaBypass Instance { get; } = new MetaBypass ();



		public MetaBypass () : base ("Bypass")
		{}



		protected override ParameterDefinition BuildParameter (ParameterDefinition.Builder builder)
		{
            return
                builder
                    .AddPower ("Power")
                    .AddSpeed ("Speed")
                    .AddReverse("Reverse")
                    .AddPause ("Pause")
                    .AddJogging("Jogging")
					.AddX     ("X")
					.AddY     ("Y")
					.Build    ();
		}
	}
}

