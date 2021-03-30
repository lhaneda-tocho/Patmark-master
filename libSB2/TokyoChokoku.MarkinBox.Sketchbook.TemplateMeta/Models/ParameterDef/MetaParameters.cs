using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public static class MetaParameters
	{
		public static ReadOnlyCollection<ParameterDefinition> Definitions{ get; }

		private static string StoreNamespace { get; } = "";




		public static ParameterDefinition HorizontalText { 
			get {
				return MetaHorizontalText.Instance.Parameter;
			}
		}
		/*--------------*/


		private static ParameterDefinition YVerticalText {
			get {
				return MetaYVerticalText.Instance.Parameter;
			}
		}
		/*--------------*/


		private static ParameterDefinition XVerticalText { 
			get {
				return MetaXVerticalText.Instance.Parameter;
			}
		}
		/*--------------*/



		private static ParameterDefinition InnerArcText {
			get {
				return MetaInnerArcText.Instance.Parameter;
			}
		}
		/*--------------*/


		private static ParameterDefinition OuterArcText {
			get {
				return MetaOuterArcText.Instance.Parameter;
			}
		}
		/*--------------*/



		public static ParameterDefinition QrCode {
			get {
				return MetaQrCode.Instance.Parameter;
			}
		}
		/*--------------*/



		public static ParameterDefinition DataMatrix {
			get {
				return MetaDataMatrix.Instance.Parameter;
			}
		}
		/*--------------*/



		public static ParameterDefinition Rectangle { 
			get {
				return MetaRectangle.Instance.Parameter;
			}
		}
		/*--------------*/




		public static ParameterDefinition Triangle {
			get {
				return MetaTriangle.Instance.Parameter;
			}
		}
		/*--------------*/




		public static ParameterDefinition Circle {
			get {
				return MetaCircle.Instance.Parameter;
			}
		}
		/*--------------*/





		public static ParameterDefinition Line {
			get {
				return MetaLine.Instance.Parameter;
			}
		}
		/*--------------*/




		public static ParameterDefinition Bypass { 
			get {
				return MetaBypass.Instance.Parameter;
			}
		}
		/*--------------*/




		static MetaParameters ()
		{
			var def = new List<ParameterDefinition> ();

			foreach ( FieldDefinition entityField in MetaFields.Definitions ) {
				def.Add (entityField.Parameter);
			}

			Definitions = def.AsReadOnly ();

		}



	}
}

