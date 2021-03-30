using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "QrCode" field.
	/// </summary>
    public partial class QrCodeParameter : IConstantParameter
	{

		private static readonly QrCodeParameter DefaultObject;

		static QrCodeParameter () {
			DefaultObject = new QrCodeParameter (
                raw : new MBData (),
				Text     : "QRCODE",
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : false,
				BasePoint: 0,
				Mirrored : false,
				X        : 0m,
				Y        : 0m,
				Height   : 3.0m,
				Angle    : 0m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static QrCodeParameter Create () {
			return DefaultObject;
		}

		public static QrCodeParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
            var fileparam = new QrCodeParameter (
                raw : data,
				Text     : data.Text,
				Power    : data.Power,
                Speed: data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				Mirrored : option.Mirrored,
				X        : (decimal) data.X,
				Y        : (decimal) data.Y,
                Height   : (decimal) data.Pitch,
				Angle    : (decimal) data.Angle
            );
            return CopyOf(
                new FileQrCodeParameter(fileparam)
                .ToAppParameter()
            );
		}


        /// <summary>
        /// Create Serializable data.
        /// </summary>
        override
        public MBData ToSerializable()
        {
            return FileQrCodeParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
        }

        public MBData ForceToSerializable () {
            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);
            var mode = new ModeWrapper (model);
            var rootNode = ParseText();
            var barcode = new BarcodeWrapper (model);

            model.Type      = (byte)QrCode.Type;
            model.Text      = Text;
			model.Power     = Power;
			model.Speed     = Speed;
            option.Pause    = Pause;
			model.BasePoint = BasePoint;
			option.Mirrored = Mirrored;
			model.X         = (float) X;
			model.Y         = (float) Y;
			model.Pitch     = (float) Height;
			model.Angle     = (float) Angle;

            mode.QrCode = true;
            mode.Calender = rootNode.HasCalendar;

            if (rootNode.HasSerial && rootNode.SerialNodeList[0].SerialNumberIsValid)
            {
                mode.Serial = true;
                model.SerialNo = (byte)rootNode.SerialNodeList[0].SerialIndexOrNull;
            }
            else
            {
                // non serial or fallback
                mode.Serial = false;
            }

            barcode.BarcodeType = BarcodeType.QrCode;
            barcode.SetPrmFlBarcodeMode ();

			return new MBData (model);
		}
	}
}

