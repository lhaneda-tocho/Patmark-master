using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;


namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "OuterArcText" field.
	/// </summary>
	public partial class OuterArcTextParameter : IConstantParameter
	{

		private static readonly OuterArcTextParameter DefaultObject;

		static OuterArcTextParameter () {
			DefaultObject = new OuterArcTextParameter (
                raw : new MBData (),
				Text     : "Text",
                Font     : FontMode.FontTC,
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging: false,
                Reverse  : false,
                Pause    : false,
				BasePoint: 0,
				Mirrored : false,
				X        : 1m,
				Y        : 1m,
				Pitch    : 1.2m,
				Height   : 1m,
				Aspect   : 100m,
				Angle    : 0m,
				Radius   : 1m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static OuterArcTextParameter Create () {
			return DefaultObject;
		}

		public static OuterArcTextParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
            var fileparam = new OuterArcTextParameter (
                raw : data,
				Text     : data.Text,
                Font     : FontModeExt.ValueOf (data.PrmFl),
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging: false,
                Reverse  : option.Reverse,
                Pause    : option.Pause,
				BasePoint: data.BasePoint,
				Mirrored : option.Mirrored,
				X        : (decimal) data.X,
				Y        : (decimal) data.Y,
				Pitch    : (decimal) data.Pitch,
				Height   : (decimal) data.Height,
				Aspect   : (decimal) data.Aspect,
				Angle    : (decimal) data.Angle,
				Radius   : (decimal) data.ArcRadius
            );
            return File2AppParam(fileparam);
        }

        /// <summary>
        /// Create Serializable data.
        /// </summary>
        /// <returns>The serializable.</returns>
        override
        public MBData ToSerializable()
        {
            var p = App2FileParam(this);
            return p.ForceToSerializable();
        }

        /// <summary>
        /// Create Serializable data.
        /// </summary>
        public MBData ForceToSerializable() {
            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);


            var mode = new ModeWrapper (model);
            var rootNode = ParseText ();
            mode.Calender = rootNode.HasCalendar;
            mode.OuterArc = true;

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

            model.Type = (byte)OuterArcText.Type;
			model.Text = Text;
            model.PrmFl = Font.ToPrmFl ();
			model.Power = Power;
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
			model.ArcRadius = (float) Radius;

            option.Reverse = Reverse;

			return new MBData (model);
		}

        /// <summary>
        /// 保存用にパラメタを修正した新しいパラメタオブジェクトを返します.
        /// </summary>
        static OuterArcTextParameter App2FileParam(IBaseOuterArcTextParameter p)
        {
            var f = FileOuterArcTextParameter.FromAppParameter(p);
            return f.Unwrap;
        }

        /// <summary>
        /// ファイル保存用のパラメタをアプリ内で扱う形式に修正した新しいパラメタオブジェクトを返します.
        /// </summary>
        /// <returns></returns>
        static OuterArcTextParameter File2AppParam(IBaseOuterArcTextParameter p)
        {
            var f = new FileOuterArcTextParameter(p);
            return CopyOf(f.ToAppParameter());
        }

    }
}

