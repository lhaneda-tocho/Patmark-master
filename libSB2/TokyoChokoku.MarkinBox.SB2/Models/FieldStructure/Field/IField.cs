using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
    /// <summary>
    /// フィールドと呼ばれる図形や文字列などの打刻情報を表現するインターフェースです．
    /// このインターフェースを実装するクラスは 不変であり，内容が更新することはできません．
    /// </summary>
	public interface IField <out ParamType> where ParamType:IConstantParameter
	{
        /// <summary>
        /// Gets the Parameter as ParamType.
        /// </summary>
        /// <value>The base parameter.</value>
		ParamType Parameter {
			get; 
		}

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
		FieldType FieldType	{
			get;
		}

        /// <summary>
        /// Gets a list of convertable field types.
        /// </summary>
        /// <value>The convertable field types.</value>
		ISet<FieldType> ChangeableTypes {
			get;
		}

        /// <summary>
        /// Validate this instance.
        /// </summary>
        ValidationResult Validate (
        );

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
		MBData ToSerializable  (
		);

        /// <summary>
        /// Converts to mutable field.
        /// </summary>
        /// <returns>The generic mutable.</returns>
		IMutableField <IMutableParameter> ToGenericMutable ();

        /// <summary>
        /// Accept the specified visitor and arg.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        /// <param name="arg">Argument.</param>
        /// <typeparam name="Return">The 1st type parameter.</typeparam>
        /// <typeparam name="Argument">The 2nd type parameter.</typeparam>
		Return Accept<Return, Argument> (
			IFieldVisitor <Return,Argument> visitor, Argument arg
		);
	}
}

