using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	public interface IConstantParameter : IParameter
	{
		/// <summary>
		/// X座標
		/// </summary>
		decimal   X { get; }

		/// <summary>
        /// Y座標
        /// </summary>
        decimal   Y { get; }

        /// <summary>
        /// ジョグ機能 ON/OFF
        /// </summary>
        bool Jogging { get; }
	}
}

