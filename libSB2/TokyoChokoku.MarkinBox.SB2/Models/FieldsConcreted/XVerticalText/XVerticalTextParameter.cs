using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "XVerticalText" field.
	/// </summary>
	public partial class XVerticalTextParameter : IConstantParameter
	{

		private static readonly XVerticalTextParameter DefaultObject;

		static XVerticalTextParameter () {
            DefaultObject = new XVerticalTextParameter (
                raw : new MBData (),
				Text     : "Text",
                Font     : FontMode.FontTC,
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : false,
				BasePoint: 0,
				Mirrored : false,
				X        : 0m,
				Y        : 1m,
				Pitch    : 1.2m,
				Height   : 1m,
				Aspect   : 100m,
				Angle    : 0m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static XVerticalTextParameter Create () {
			return DefaultObject;
		}

		public static XVerticalTextParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
            var fileparam = new XVerticalTextParameter (
                raw : data,
				Text     : data.Text,
                Font     : FontModeExt.ValueOf (data.PrmFl),
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging  : false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse  : false,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				Mirrored : option.Mirrored,
				X        : (decimal) data.X,
				Y        : (decimal) data.Y,
				Pitch    : (decimal) data.Pitch,
				Height   : (decimal) data.Height,
				Aspect   : (decimal) data.Aspect,
				Angle    : (decimal) data.Angle
            );
            return CopyOf(
                new FileXVerticalTextParameter(fileparam)
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
            return FileXVerticalTextParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
        }

        // パラメタの変換をかけずにシリアライズ可能な形式にする.
        MBData ForceToSerializable()
        {
            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);


            var mode = new ModeWrapper (model);
            var rootNode = ParseText ();
            mode.Calender = rootNode.HasCalendar;
            mode.VerticalX = true;

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

            model.Type = (byte)XVerticalText.Type;
			model.Text = Text;
            model.PrmFl = Font.ToPrmFl ();
			model.Speed = Speed;
            option.Pause = Pause;
			model.BasePoint = BasePoint;
            option.Mirrored = Mirrored;
			model.X = (float) X;
			model.Y = (float) Y;
			model.Pitch = (float) Pitch;
			model.Height = (float) Height;
			model.Aspect = (float) Aspect;
			model.Angle = (float) Angle;


			return new MBData (model);
		}
	}
}

