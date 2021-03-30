using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public static class MetaFields
	{
		public static ReadOnlyCollection<FieldDefinition> Definitions{ get; }



		static MetaFields () {
			var def = new List<FieldDefinition> ();

			def.Add (MetaHorizontalText.Instance);
			def.Add (MetaYVerticalText.Instance);
			def.Add (MetaXVerticalText.Instance);
			def.Add (MetaInnerArcText.Instance);
			def.Add (MetaOuterArcText.Instance);
			def.Add (MetaQrCode.Instance);
			def.Add (MetaDataMatrix.Instance);
			def.Add (MetaRectangle.Instance);
			def.Add (MetaTriangle.Instance);
			def.Add (MetaCircle.Instance);
			def.Add (MetaLine.Instance);
			def.Add (MetaBypass.Instance);
			def.Add (MetaEllipse.Instance);


			Definitions = def.AsReadOnly ();
		}
	}
}

