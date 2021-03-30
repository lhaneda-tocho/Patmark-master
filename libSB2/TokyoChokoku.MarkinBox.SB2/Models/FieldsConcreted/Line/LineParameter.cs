using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "Line" field.
	/// </summary>
	public partial class LineParameter : IConstantParameter
	{
        public decimal X {
            get {
                return StartX;
            }
        }

        public decimal Y {
            get {
                return StartY;
            }
        }

		private static readonly LineParameter DefaultObject;

		static LineParameter () {
			DefaultObject = new LineParameter (
                raw : new MBData (),
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                // TODO: Reverseを MBDataから読み取ること
                Jogging: false,
                Reverse: false,
                Pause    : false,
				StartX   : 0m,
				StartY   : 0m,
				CenterX  : 2m,
				CenterY  : 2m,
				EndX     : 4m,
				EndY     : 4m,
                IsBezierCurve
                         : false);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static LineParameter Create () {
			return DefaultObject;
		}

		public static LineParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable());
            return new LineParameter (
                raw : data,
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : option.Pause,
				StartX   : (decimal) data.X,
				StartY   : (decimal) data.Y,
				CenterX  : (decimal) data.Angle,
				CenterY  : (decimal) data.ArcRadius,
				EndX     : (decimal) data.Height,
                EndY     : (decimal) data.Pitch,
                IsBezierCurve : 
                           0.9f < data.Aspect && data.Aspect < 1.1f);
			
			 // Mode     : 4000,
		}

		/// <summary>
		/// Create Serializable data.
        /// </summary>
        override
		public MBData ToSerializable () {
            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);
            var mode = new ModeWrapper (model);

            model.Type = (byte)Line.Type;
			model.Power = Power;
			model.Speed = Speed;
            option.Pause = Pause;
			model.X = (float) X;
			model.Y = (float) Y;
            model.Angle     = (float) CenterX;
            model.ArcRadius = (float) CenterY;
            model.Height    = (float) EndX;
            model.Pitch    = (float) EndY;

            mode.RectangleLineTriangle = true;

			return new MBData (model);
		}
	}
}

