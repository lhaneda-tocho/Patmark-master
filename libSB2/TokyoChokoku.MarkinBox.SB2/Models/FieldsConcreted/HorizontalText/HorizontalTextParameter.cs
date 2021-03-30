using System;
using System.Diagnostics;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "HorizontalText" field.
	/// </summary>
	public partial class HorizontalTextParameter : IConstantParameter
	{
		private static readonly HorizontalTextParameter DefaultObject;

		static HorizontalTextParameter () {
			DefaultObject = new HorizontalTextParameter (
                raw : new MBData (),
				Text     : "Text",
                Font     : FontMode.FontTC,
                Power    : PowerRange.Default,
                Speed    : SpeedRange.Default,
                Jogging: false,
                Reverse  : false,
                Pause    : false,
				BasePoint: 0,
				Mirrored : false,
				X        : 0m,
				Y        : 1m,
				Pitch    : 3.0m,
				Height   : 2.0m,
				Aspect   : 60m,
				Angle    : 0m
            );
		}

		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static HorizontalTextParameter Create () {
			return DefaultObject;
		}

		public static HorizontalTextParameter Create ( MBData data ) {
            OptionWrapper option = new OptionWrapper (data.ToMutable());
            var fileparam = new HorizontalTextParameter(
                raw: data,
                Text     : data.Text,
                Font     : FontModeExt.ValueOf(data.PrmFl),
                Power    : data.Power,
                Speed    : data.Speed,
                Jogging  : false,
                Reverse  : option.Reverse,
                Pause    : option.Pause,
                BasePoint: data.BasePoint,
                Mirrored : option.Mirrored,
                X        : (decimal)data.X,
                Y        : (decimal)data.Y,
                Pitch    : (decimal)data.Pitch,
                Height   : (decimal)data.Height,
                Aspect   : (decimal)data.Aspect,
                Angle    : (decimal)data.Angle
            );
            return CopyOf(
                new FileHorizontalTextParameter(fileparam)
                .ToAppParameter()
            );
        }

		/// <summary>
		/// Create Serializable data.
		/// </summary>
        /// <returns>The serializable.</returns>
        override
		public MBData ToSerializable ()
        {
            return FileHorizontalTextParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
		}

        // パラメタの変換をかけずにシリアライズ可能な形式にする.
        MBData ForceToSerializable()
        {
            var model = raw.ToMutable();
            var option = new OptionWrapper(model);


            var mode = new ModeWrapper(model);
            var rootNode = ParseText();

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

            model.Type = (byte)HorizontalText.Type;
            model.Text = Text;
            model.PrmFl = Font.ToPrmFl();
            model.Power = Power;
            model.Speed = Speed;
            option.Pause = Pause;
            model.BasePoint = BasePoint;
            option.Mirrored = Mirrored;
            model.X = (float)X;
            model.Y = (float)Y;
            model.Pitch = (float)Pitch;
            model.Height = (float)Height;
            model.Aspect = (float)Aspect;
            model.Angle = (float)Angle;

            option.Reverse = Reverse;

            return new MBData(model);
        }
    }
}
