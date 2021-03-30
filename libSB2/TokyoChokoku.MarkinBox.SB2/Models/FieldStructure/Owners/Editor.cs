using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
    using Parameters;
    using Validators;
    using CollisionUtil;

    /// <summary>
    /// Editor.
    /// </summary>
    public abstract class Editor
	{
        /// <summary>
        /// 所持するフィールド
        /// </summary>
        /// <value>The field.</value>
		public  IMutableField <IMutableParameter> Field {
			get;
			private set;
		}

        /// <summary>
        /// このフィールドの衝突判定 (タッチ判定用)
        /// </summary>
        /// <value>The collision.</value>
        public ICollision Collision {
            get { return Field.CreateCollision (); }
        }

        /// <summary>
        /// このフィールドの衝突判定 (厳密な判定)
        /// </summary>
        /// <value>The collision.</value>
        public ICollision PreciseCollision {
            get { return Field.CreatePreciseCollision (); }
        }

        /// <summary>
        /// 所持するフィールドのタイプ
        /// </summary>
        /// <value>The type of the field.</value>
        public FieldType FieldType {
			get{ return Field.FieldType; }
		}

        /// <summary>
        /// 変更可能な FieldType
        /// </summary>
        /// <value>The changeable types.</value>
        public ISet<FieldType> ChangeableTypes { 
			get{ return Field.ChangeableTypes; }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="field">Field.</param>
        protected Editor (IMutableField<IMutableParameter> field)
		{
			Field = field;
		}

        /// <summary>
        /// 所持する Field を指定された FieldType に変換します．
        /// 
        /// 変換できない FieldType を指定された場合は例外が発生します．
        /// 
        /// </summary>
        /// <returns>The change type to.</returns>
        /// <param name="type">Type.</param>
		public void ChangeTypeTo (FieldType type)
		{
            Field = Field.ConvertTo (type).ToGenericMutable ();
		}

        /// <summary>
        /// 所持する Field を指定された FieldType に変換しようと試みます．
        /// 変換した後のオブジェクトに 妥当性チエックを行います．
        /// 問題があった場合は変更しません．
        /// 
        /// 変換できない FieldType を指定された場合は例外が発生します．
        /// 
        /// </summary>
        /// <returns>The change type to.</returns>
        /// <param name="type">Type.</param>
        public ValidationResult TryChangeTypeTo (FieldType type)
        {
            var converted = Field.ConvertTo (type);
            var result = converted.Validate ();
            if (!result.HasError)
                Field = converted.ToGenericMutable ();
            return result;
        }

        /// <summary>
        /// 所持するフィールドを MBData に変換したものを返します，
        /// </summary>
        /// <returns>The serializable.</returns>
		public MBData ToSerializable () {
			return Field.ToSerializable ();
		}

        /// <summary>
        /// 所持するフィールドに Visitor パターンを適用します．
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        /// <param name="arg">Argument.</param>
        /// <typeparam name="R">The 1st type parameter.</typeparam>
        /// <typeparam name="T">The 2nd type parameter.</typeparam>
		public R Accept<R,T>(IMutableFieldVisitor<R,T> visitor, T arg)
		{
			return Field.Accept (visitor, arg);
		}

        override
        public string ToString ()
        {
            var name = GetType ().FullName;
            var mes = Field.ToString ();

            return name + '(' + mes + ')';
        }

        /// <summary>
        /// このオブジェクトを対応するOwnerに変換し，指定された型にキャストして返します．
        /// 指定された型にキャストできない場合は ArgumentOutOfRangeException を発生させます．
        /// ．
        /// </summary>
        /// <returns>The editor.</returns>
        /// <typeparam name="NextType">変換したEditorのキャスト先</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     指定された型にキャストできない場合
        /// </exception>
        public abstract NextType ToOwner<NextType> ()
            where NextType : Owner;

        /// <summary>
        /// このオブジェクトを複製し，指定された型にキャストして返します．
        /// この複製の時，デリゲートはコピーされないことに注意してください．
        /// 指定された型にキャストできない場合は ArgumentOutOfRangeException を発生させます．
        /// </summary>
        /// <typeparam name="NextType">複製したEditorのキャスト先</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     指定された型にキャストできない場合
        /// </exception>
        public abstract NextType Duplicate<NextType> ()
            where NextType : Editor;
	}
}

