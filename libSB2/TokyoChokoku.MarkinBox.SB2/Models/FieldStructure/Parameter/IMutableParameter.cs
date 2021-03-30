using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	/// <summary>
	/// 各MBObjectのデータの 大元となる インターフェースです．
	/// </summary>
	public interface IMutableParameter : IParameter
	{
		/// <summary>
		/// X座標
		/// </summary>
		decimal   X { get; set; }

		/// <summary>
		/// Y座標
		/// </summary>
		decimal   Y { get; set; }

        XStore XStore { get; }
        YStore YStore { get; }
	}
}

