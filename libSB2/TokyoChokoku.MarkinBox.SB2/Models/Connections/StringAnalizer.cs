using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Connections
{
	public delegate bool StringAnalizer<Type> (string raw, out Type result);
}

