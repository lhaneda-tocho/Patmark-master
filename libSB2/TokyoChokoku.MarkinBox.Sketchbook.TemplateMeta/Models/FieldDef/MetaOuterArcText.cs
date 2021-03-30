using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaOuterArcText : FieldDefinition
	{
		public static MetaOuterArcText Instance { get; } = new MetaOuterArcText ();



		public MetaOuterArcText () : base ("OuterArcText")
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
                    .AddFont("Font")
					.AddPitch("Pitch")
					.AddHeight("Height")
					.AddAspect("Aspect")
					.AddArcRadius("Radius")
					.AddAngle("Angle")
					.Build();
		}

        override
        public ReadOnlyCollection<FieldDefinition> Changable {
            get {
                List < FieldDefinition > def = new List < FieldDefinition > ();

                def.Add ( MetaInnerArcText.Instance );

                return def.AsReadOnly ();
            }
        }
	}
}

