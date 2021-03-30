using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Fields
{
    using Parameters;
    using CollisionUtil;

    /// <summary>
    /// Owner.
    /// </summary>
	public abstract class Owner
	{
        /// <summary>
        /// 所持するフィールド
        /// </summary>
        /// <value>The field.</value>
		public  IField <IConstantParameter> Field {
			get;
		}

        /// <summary>
        /// このフィールドの衝突判定 (タッチ判定用)
        /// </summary>
        /// <value>The collision.</value>
        public ICollision Collision {
            get;
        }

        /// <summary>
        /// このフィールドの衝突判定 (厳密な判定)
        /// </summary>
        /// <value>The collision.</value>
        public ICollision PreciseCollision {
            get;
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
        /// フィールドの座標に合わせてヘッドの位置を移動させます。
        /// </summary>
        /// <value>The jogger.</value>
        public FieldJogger Jogger { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="field">Field.</param>
        protected Owner (IField<IConstantParameter> field)
        {
            Field = field;
            Collision = field.CreateCollision ();
            PreciseCollision = field.CreatePreciseCollision ();
            Jogger = new FieldJogger(field);
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
		public R Accept<R,T>(IFieldVisitor<R,T> visitor, T arg)
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
        /// このオブジェクトを対応するEditorに変換し，指定された型にキャストして返します．
        /// 指定された引数にキャストできない場合は ArgumentOutOfRangeException を発生させます．
        /// </summary>
        /// <returns>The editor.</returns>
        /// <typeparam name="NextType">変換したEditorのキャスト先</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     指定された型にキャストできない場合
        /// </exception>
        public abstract NextType ToEditor <NextType> ()
            where NextType : Editor;

        /// <summary>
        /// このオブジェクトを複製し，指定された型にキャストして返します．
        /// 指定された型にキャストできない場合は ArgumentOutOfRangeException を発生させます．
        /// </summary>
        /// <typeparam name="NextType">複製したOwnerのキャスト先</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     指定された型にキャストできない場合
        /// </exception>
        public abstract NextType Duplicate<NextType> ()
            where NextType : Owner;
	}
}

