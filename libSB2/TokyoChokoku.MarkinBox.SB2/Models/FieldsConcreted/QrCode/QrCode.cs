using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
	/// <summary>
	/// 
	/// </summary>
	public abstract partial class QrCode
	{
        public IField <IConstantParameter> ToDataMatrix () {
            return DataMatrix.Constant.Create (ToSerializable ());
        }
	}

}

