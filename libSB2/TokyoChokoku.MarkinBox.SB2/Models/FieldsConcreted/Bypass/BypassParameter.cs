using System;
using System.ComponentModel;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// Ediable parameters of "Bypass" field.
	/// </summary>
	public partial class BypassParameter : IConstantParameter
	{

		private static readonly BypassParameter DefaultObject;

		static BypassParameter () {
            DefaultObject = new BypassParameter (
                raw: new MBData (),
                Power: PowerRange.Default,
                Speed: SpeedRange.Default,
                Jogging : false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse : false,
                Pause   : false,
				X       : 0m,
				Y       : 0m);
		}


		/// <summary>
		/// Create default parameter.
		/// </summary>
		public static BypassParameter Create () {
			return DefaultObject;
		}


		public static BypassParameter Create ( MBData data ) {
            var option = new OptionWrapper (data.ToMutable ());
            return new BypassParameter (
                raw     : new MBData (),
				Power   : data.Power,
				Speed   : data.Speed,
                Jogging: false,
                // TODO: Reverseを MBDataから読み取ること
                Reverse : false,
                Pause   : option.Pause,
				X       : (decimal)data.X,
				Y       : (decimal)data.Y);
		}


		/// <summary>
		/// Create Serializable data.
		/// </summary>
        override
		public MBData ToSerializable () {
            var model = raw.ToMutable ();
            var option = new OptionWrapper (model);
            var mode = new ModeWrapper (model);
            mode.BitSet = 0x0100;


            model.Type = (byte)Bypass.Type;
			model.Power = Power;
			model.Speed = Speed;
            option.Pause = Pause;
			model.X     = (float) X;
			model.Y 	= (float) Y;

			return new MBData (model);
		}
	}
}

