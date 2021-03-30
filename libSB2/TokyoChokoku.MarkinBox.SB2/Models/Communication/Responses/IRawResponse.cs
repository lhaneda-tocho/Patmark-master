using System;

using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public interface IRawResponse
	{
        bool IsOk { get; }
        List<byte> Data  { get; }
	}
}