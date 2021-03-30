using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaHorizontalText : FieldDefinition
	{
		public static MetaHorizontalText Instance { get; } = new MetaHorizontalText ();



        public MetaHorizontalText () : base ("HorizontalText")
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

