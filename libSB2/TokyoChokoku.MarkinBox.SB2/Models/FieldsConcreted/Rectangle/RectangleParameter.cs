using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    /// <summary>
    /// Ediable parameters of "Rectangle" field.
    /// </summary>
    public partial class RectangleParameter
    {

        private static readonly RectangleParameter DefaultObject;

        static RectangleParameter()
        {
            DefaultObject = new RectangleParameter(
                raw: new MBData(),
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause: false,
                BasePoint: 0,
                X: 0m,
                Y: 0m,
                Height: 1m,
                Aspect: 100m,
                Angle: 0m);
        }


        /// <summary>
        /// Create default parameter.
        /// </summary>
        public static RectangleParameter Create()
        {
            return DefaultObject;
        }

        public static RectangleParameter Create(MBData data)
        {

            decimal width = (decimal)(data.Height - data.X);
            decimal height = (decimal)(data.Y - data.Pitch);
            decimal aspect;

            if (height != 0)
            {
                aspect = 100m * width / height;
            }
            else
            {
                aspect = 100m;
            }

            var option = new OptionWrapper(data.ToMutable());

            var fileparam = new RectangleParameter(
                raw: data,
                Power: data.Power,
                Speed: data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
                Pause: option.Pause,
                BasePoint: data.BasePoint,
                X: (decimal)data.X,
                Y: (decimal)data.Y,
                Height: height,
                Aspect: aspect,
                Angle: (decimal)data.Angle
            );
            return CopyOf(
                new FileRectangleParameter(fileparam)
                .ToAppParameter()
            );
        }


        /// <summary>
        /// Create Serializable data.
        /// </summary>
        override
        public MBData ToSerializable()
        {
            return FileRectangleParameter
                .FromAppParameter(this)
                .Unwrap
                .ForceToSerializable();
        }

        MBData ForceToSerializable()
        {
            var model = raw.ToMutable();
            var option = new OptionWrapper(model);
            var mode = new ModeWrapper(model);


            model.X = (float)X;
            model.Y = (float)Y;
            model.Height = (float)(X + Width);
            model.Pitch = (float)(Y - Height);

            model.Type = (byte)Rectangle.Type;
            model.Power = Power;
            model.Speed = Speed;
            option.Pause = Pause;
            model.BasePoint = BasePoint;
            model.Angle = (float)Angle;

            mode.RectangleLineTriangle = true;

            return new MBData(model);
        }
    }
}

