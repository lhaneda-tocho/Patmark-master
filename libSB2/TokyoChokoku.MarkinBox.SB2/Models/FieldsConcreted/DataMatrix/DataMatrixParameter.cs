using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "DataMatrix" field.
	/// </summary>
	public partial class DataMatrixParameter : IConstantParameter
	{

		private static readonly DataMatrixParameter DefaultObject;

		static DataMatrixParameter () {
			DefaultObject = new DataMatrixParameter (
                raw : new MBData (),
				Text     : "Data Matrix",
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
				Angle    : 0m,
                DotCount : DataMatrixConstant.DotCount10x10);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static DataMatrixParameter Create () {
			return DefaultObject;
		}


		public static DataMatrixParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
            var dm = new DataMatrixWrapper (data.ToMutable ());

			var fileparam = new DataMatrixParameter (
                raw      : data,
				Text     : data.Text,
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging  : false,
                Reverse  : option.Reverse,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				Mirrored : option.Mirrored,
				X        : (decimal)data.X,
				Y        : (decimal)data.Y,
                Height   : (decimal)dm.Height,
				Angle    : (decimal)data.Angle,
                DotCount : new DotCount2D (dm.HorizontalDotCount, dm.VerticalDotCount)
            );
            
            return CopyOf(
                new FileDataMatrixParameter(fileparam)
                .ToAppParameter()
            );
		}

        /// <summary>
        /// Create Serializable data.
        /// </summary>
        /// <returns>The serializable.</returns>
        override
        public MBData ToSerializable()
        {
            return FileDataMatrixParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
        }

		public MBData ForceToSerializable () {
            var model = raw.ToMutable ();

            var dm     = new DataMatrixWrapper (model);
            var option = new OptionWrapper     (model);
            var mode   = new ModeWrapper       (model);
            var rootNode = ParseText();

            option.Pause    = Pause;
            option.Mirrored = Mirrored;
            option.Reverse  = Reverse;

            model.Type      = (byte) DataMatrix.Type;
            dm.Text         = Text;
			dm.Power        = Power;
			dm.Speed        = Speed;

			dm.BasePoint    = BasePoint;
            dm.X            = (float) X;
            dm.Y            = (float) Y;
            dm.Angle        = (float) Angle;

            dm.Width                = (float) Width;
            dm.BarcodeType          = BarcodeType.DataMatrix;
            dm.HorizontalDotCount   = this.DotCount.Horizontal;
            dm.VerticalDotCount     = this.DotCount.Vertical;

            dm.SetPrmFlBarcodeMode ();

            mode.DataMatrix = true;
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

            return new MBData (model);
		}
	}
}

