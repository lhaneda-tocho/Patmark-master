using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaDataMatrix : FieldDefinition
	{
		public static MetaDataMatrix Instance { get; } = new MetaDataMatrix ();



		public MetaDataMatrix () : base ("DataMatrix")
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
					.AddHeight("Height")
					.AddAngle("Angle")
					.AddDotCount("DotCount")
					.Build();
		}

        override
        public ReadOnlyCollection<FieldDefinition> Changable {
            get {
                List < FieldDefinition > def = new List < FieldDefinition > ();

                def.Add ( MetaQrCode.Instance );

                return def.AsReadOnly ();
            }
        }
	}
}

