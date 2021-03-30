using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "YVerticalText" field.
	/// </summary>
	public partial class YVerticalTextParameter : IConstantParameter
	{

		private static readonly YVerticalTextParameter DefaultObject;

		static YVerticalTextParameter () {
            DefaultObject = new YVerticalTextParameter (
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
				Y        : 4.6m,
				Pitch    : 1.2m,
				Height   : 1m,
				Aspect   : 100m,
				Angle    : 0m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static YVerticalTextParameter Create () {
			return DefaultObject;
		}


		public static YVerticalTextParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
			var fileparam = new YVerticalTextParameter (
                raw : data,
				Text     : data.Text,
                Font     : FontModeExt.ValueOf (data.PrmFl),
				Power    : data.Power,
				Speed    : data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse: false,
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
            mode.VerticalY = true;

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

            model.Type = (byte)YVerticalText.Type;
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

        static Position2D DeltaFilePosToAppPos(IBaseYVerticalTextParameter p)
        {
            // 移動量サイズ比率
            double offsetRateX, offsetRateY;
            // X
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointLT:
                    offsetRateX = +0.0;
                    break;

                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointCT:
                    offsetRateX = +0.5;
                    break;

                case Consts.FieldBasePointRB:
                case Consts.FieldBasePointRM:
                case Consts.FieldBasePointRT:
                    offsetRateX = +1.0;
                    break;
            }
            // Y
            switch (p.BasePoint)
            {
                default:
                case Consts.FieldBasePointLB:
                case Consts.FieldBasePointCB:
                case Consts.FieldBasePointRB:
                    offsetRateY = -0.0;
                    break;

                case Consts.FieldBasePointLM:
                case Consts.FieldBasePointCM:
                case Consts.FieldBasePointRM:
                    offsetRateY = -0.5;
                    break;

                case Consts.FieldBasePointLT:
                case Consts.FieldBasePointCT:
                case Consts.FieldBasePointRT:
                    offsetRateY = -1.0;
                    break;
            }
            // 位置ベクトルに
            var offset = new Position2D(
                x: offsetRateX * p.BoxWidth,
                y: offsetRateY * p.BoxHeight
            );
            // 回転の影響を加味する
            var m = AffineMatrix2D.NewRotation(-(double)p.Angle);
            var ans = m * offset;
            return ans;
        }

        static Position2D FilePosToAppPos(IBaseYVerticalTextParameter p)
        {
            var delta = DeltaFilePosToAppPos(p);
            // 修正後の位置を計算
            var prev = new Position2D((double)p.X, (double)p.Y);
            return prev + delta;
        }

        static Position2D AppPosToFilePos(IBaseYVerticalTextParameter p)
        {
            var delta = DeltaFilePosToAppPos(p);
            // 修正後の位置を計算
            var prev = new Position2D((double)p.X, (double)p.Y);
            return prev - delta;
        }


        /// <summary>
        /// 保存用にパラメタを修正した新しいパラメタオブジェクトを返します.
        /// </summary>
        static YVerticalTextParameter App2FileParam(IBaseYVerticalTextParameter p)
        {
            var mutable = MutableYVerticalTextParameter.CopyOf(p);
            var newpos = AppPosToFilePos(mutable);
            mutable.X = (decimal)newpos.X;
            mutable.Y = (decimal)newpos.Y;
            return YVerticalTextParameter.CopyOf(mutable);
        }

        /// <summary>
        /// ファイル保存用のパラメタをアプリ内で扱う形式に修正した新しいパラメタオブジェクトを返します.
        /// </summary>
        /// <returns></returns>
        static YVerticalTextParameter File2AppParam(IBaseYVerticalTextParameter p)
        {
            var mutable = MutableYVerticalTextParameter.CopyOf(p);
            var newpos = FilePosToAppPos(mutable);
            mutable.X = (decimal)newpos.X;
            mutable.Y = (decimal)newpos.Y;
            return YVerticalTextParameter.CopyOf(mutable);
        }
    }
}

