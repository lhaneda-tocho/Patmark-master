using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
    /// <summary>
    /// フィールドと呼ばれる図形や文字列などの打刻情報を表現するインターフェースです．
    /// </summary>
	public interface IBaseField<out ParamType> where ParamType:IParameter
	{
        /// <summary>
        /// Gets the Parameter as ParamType.
        /// </summary>
        /// <value>The base parameter.</value>
		ParamType   		        BaseParameter   { get; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
		FieldType         	        FieldType  		{ get; }

        /// <summary>
        /// Gets a list of convertable field types.
        /// </summary>
        /// <value>The convertable field types.</value>
		ISet<FieldType>   	        ChangeableTypes	{ get; }

        /// <summary>
        /// Converts to specified field type.
        /// </summary>
        /// <returns>Specified field type.</returns>
        /// <param name="type">Field Type.</param>
		IField <IConstantParameter> ConvertTo (
			FieldType type
		);

        /// <summary>
        /// Converts to the MBData.
        /// </summary>
        /// <returns>MBData</returns>
		MBData				        ToSerializable  ();
	}
}

