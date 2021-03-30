using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "Circle" field.
	/// </summary>
	public partial class CircleParameter : IConstantParameter
	{

		private static readonly CircleParameter DefaultObject;

		static CircleParameter () {
			DefaultObject = new CircleParameter (
                raw : new MBData (),
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : false,
				BasePoint: 0,
				X        : 0m,
				Y        : 0m,
				Radius   : 1m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static CircleParameter Create () {
			return DefaultObject;
		}


		public static CircleParameter Create ( MBData data ) {
            decimal diameter = (decimal)(data.Y - data.Pitch);

            var option = new OptionWrapper (data.ToMutable ());

			var fileparam = new CircleParameter (
                raw : data,
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				X        : (decimal)data.X,
				Y        : (decimal)data.Y,
                Radius   : (decimal)diameter / 2
            );
            return CopyOf(
                new FileCircleParameter(fileparam)
                .ToAppParameter()
            );
		}

        /// <summary>
        /// Create Serializable data.
        /// </summary>
        override
        public MBData ToSerializable()
        {
            return FileCircleParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
        }

        /// <summary>
        /// Create Serializable data.
        /// </summary>
        public MBData ForceToSerializable () {
            var diameter = Radius * 2;

            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);
            var mode = new ModeWrapper (model);

            model.X = (float)X;
            model.Y = (float)Y;
            model.Height = (float)(X + diameter);
            model.Pitch  = (float)(Y - diameter);

            model.Type = (byte)Circle.Type;
            model.Power = Power;
            model.Speed = Speed;
            option.Pause = Pause;
            model.BasePoint = BasePoint;

            mode.CircleEllipse = true;

			return new MBData (model);
		}
	}
}

