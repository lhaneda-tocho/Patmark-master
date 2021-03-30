using System;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaXVerticalText : FieldDefinition
	{
		public static MetaXVerticalText Instance { get; } = new MetaXVerticalText ();



		public MetaXVerticalText ()
            : base ("XVerticalText")
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
					.AddMirrored("Mirrored")
                    .AddJogging("Jogging")
					.AddX("X")
					.AddY("Y")
					.AddText("Text")
                    .AddFont ("Font")
					.AddPitch("Pitch")
					.AddHeight("Height")
					.AddAspect("Aspect")
					.AddAngle("Angle")
					.Build();
		}
	}
}

