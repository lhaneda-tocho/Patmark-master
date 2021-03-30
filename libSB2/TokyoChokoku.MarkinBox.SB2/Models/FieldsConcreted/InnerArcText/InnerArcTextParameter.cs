using System;
using MathNet.Numerics.LinearAlgebra;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "InnerArcText" field.
	/// </summary>
	public partial class InnerArcTextParameter : IConstantParameter
	{

		private static readonly InnerArcTextParameter DefaultObject;

		static InnerArcTextParameter () {
			DefaultObject = new InnerArcTextParameter (
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
		public static InnerArcTextParameter Create () {
			return DefaultObject;
		}

		public static InnerArcTextParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable());
			var fileparam = new InnerArcTextParameter (
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
				Radius   : (decimal) data.ArcRadius);
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
            mode.InnerArc = true;

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

            model.Type = (byte)InnerArcText.Type;
			model.Text = Text;
            model.PrmFl = Font.ToPrmFl ();
			model.Power = Power;
			model.Speed = Speed;
            option.Pause = Pause;
			model.BasePoint = BasePoint;
			option.Mirrored = Mirrored;
			model.X = (float) X;
			model.Y =(float) Y;
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
        static InnerArcTextParameter App2FileParam(IBaseInnerArcTextParameter p)
        {
            var f = FileInnerArcTextParameter.FromAppParameter(p);
            return f.Unwrap;
        }

        /// <summary>
        /// ファイル保存用のパラメタをアプリ内で扱う形式に修正した新しいパラメタオブジェクトを返します.
        /// </summary>
        /// <returns></returns>
        static InnerArcTextParameter File2AppParam(IBaseInnerArcTextParameter p)
        {
            var f = new FileInnerArcTextParameter(p);
            return CopyOf(f.ToAppParameter());
        }
    }
}

