using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public sealed class MetaQrCode : FieldDefinition
	{
		public static MetaQrCode Instance { get; } = new MetaQrCode ();



		public MetaQrCode () : base ("QrCode")
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
					.Build();
		}

        override
        public ReadOnlyCollection<FieldDefinition> Changable {
            get {
                List < FieldDefinition > def = new List < FieldDefinition > ();

                def.Add ( MetaDataMatrix.Instance );

                return def.AsReadOnly ();
            }
        }
	}
}

