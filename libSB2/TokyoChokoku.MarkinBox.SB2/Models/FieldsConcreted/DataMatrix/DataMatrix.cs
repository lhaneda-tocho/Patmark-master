using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
	/// <summary>
	/// 
	/// </summary>
	public abstract partial class DataMatrix
	{
        public IField <IConstantParameter> ToQrCode () {
            return QrCode.Constant.Create (ToSerializable ());
        }
	}
}

