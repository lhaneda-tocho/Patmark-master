using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "Triangle" field.
	/// </summary>
	public partial class TriangleParameter : IConstantParameter
	{

		private static readonly TriangleParameter DefaultObject;

		static TriangleParameter () {
			DefaultObject = new TriangleParameter (
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
				HornX    : 0.5m,
				Height   : 1m,
				Aspect   : 100m,
				Angle    : 0m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static TriangleParameter Create () {
			return DefaultObject;
		}

		public static TriangleParameter Create ( MBData data ) {

            decimal height = (decimal)(data.Y - data.Pitch);
            decimal width  = (decimal)(data.Height - data.X);
            decimal trueAspect;

            if (height != 0)
            {
                trueAspect = 100m * width / height;
            }
            else
            {
                trueAspect = 100m;
            }

			decimal hornX  = (decimal)data.Aspect;


            var option = new OptionWrapper (data.ToMutable ());

            var fileparam = new TriangleParameter (
                raw : data,
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				X        : (decimal) data.X,
				Y        : (decimal)data.Y,
				HornX    : hornX,
				Height   : height,
				Aspect   : trueAspect,
				Angle    : (decimal) data.Angle
            );

            return CopyOf(
                new FileTriangleParameter(fileparam)
                .ToAppParameter()
            );
        }





		/// <summary>
		/// Create Serializable data.
        /// </summary>
        override
		public MBData ToSerializable () {
            var width = Width;

            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);
            var mode = new ModeWrapper (model);

            decimal modelX, modelY;

            switch (BasePoint) {
            default:
            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointLT:
                modelX = X;
                break;

            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointCT:
                modelX = (X - width / 2);
                break;

            case Consts.FieldBasePointRB:
            case Consts.FieldBasePointRM:
            case Consts.FieldBasePointRT:
                modelX = (X - width);
                break;
            }

            switch (BasePoint) {
            default:
            case Consts.FieldBasePointLT:
            case Consts.FieldBasePointCT:
            case Consts.FieldBasePointRT:
                modelY = (Y + Height);
                break;

            case Consts.FieldBasePointLM:
            case Consts.FieldBasePointCM:
            case Consts.FieldBasePointRM:
                modelY = (Y + Height / 2);
                break;

            case Consts.FieldBasePointLB:
            case Consts.FieldBasePointCB:
            case Consts.FieldBasePointRB:
                modelY = (Y);
                break;
            }

            model.X = (float)modelX;
            model.Y = (float)modelY;
            model.Height = (float)(modelX + width);
            model.Pitch  = (float)(modelY - Height);
            model.Aspect = (float)(X + HornX);

            model.Type = (byte)Triangle.Type;
            model.Power = Power;
            model.Speed = Speed;
            option.Pause = Pause;
            model.BasePoint = BasePoint;
            model.Angle = (float)Angle;

            mode.Triangle = true;

            return new MBData (model);
		}
	}
}

