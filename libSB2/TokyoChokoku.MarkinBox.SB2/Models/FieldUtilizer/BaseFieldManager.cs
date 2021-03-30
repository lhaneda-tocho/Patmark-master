namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using System.Linq;
    using System.Collections.Generic;
    using Fields;
    using System.Collections.Immutable;
    using Monad;
    
    /// <summary>
    /// コンテキストの内容の変更と再描画の呼び出しまでを担当する Controller の基底クラスです．
    /// サブクラスは 描画ロジックを担当する Model クラスの定義・集約と，
    /// イベントの取得対象となる View もしくはそのデリゲートメソッドの定義を行う必要があります．
    /// </summary>
    public abstract class BaseFieldManager<OwnerType, EditorType, ContextType>
        where ContextType : FieldContext<OwnerType, EditorType>, new()
        where OwnerType : Owner
        where EditorType : Editor
    {
        /// <summary>
        /// このマネージャが持つコンテキスト
        /// </summary>
        protected readonly ContextType context;

        /// <summary>
        /// 保持するコンテキストを指定して初期化します．
        /// </summary>
        /// <param name="context">Context.</param>
        /// <exception cref="System.NullReferenceException">contextに null が指定された時</exception>
        protected BaseFieldManager (ContextType context)
        {
            if (context == null)
                throw new System.NullReferenceException ();
            this.context = context;
        }

        /// <summary>
        /// 引数なしコンストラクタからコンテキストを作成し，初期化します．
        /// </summary>
        protected BaseFieldManager ()
        {
            context = new ContextType ();
        }

        /* ---- コンテキスト橋渡し ---- */
        /// <summary>
        /// 記憶しているフィールドのリストを返します．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.FieldList"/>
        /// <value>The field list.</value>
        public IReadOnlyList<OwnerType> FieldList {
            get {
                return context.ReadonlyFieldList;
            }
        }

        /// <summary>
        /// フィールドリストをキャストしたものです．
        /// </summary>
        /// <value>The base field list copy.</value>
        public IReadOnlyList<Owner> BaseFieldListCopy {
            get {
                return context.BaseFieldListCopy;
            }
        }

        /// <summary>
        /// 記憶しているフィールドのコピーを返します．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.FieldListCopy"/>
        /// <value>The field list copy.</value>
        public List<OwnerType> FieldListCopy {
            get {
                return context.FieldListCopy;
            }
        }

        /// <summary>
        /// 記憶しているフィールドのコピーを返します．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.FieldListImmutable"/>
        /// <value>The field list immutable.</value>
        public ImmutableList<OwnerType> FieldListImmutable {
            get {
                return context.FieldListImmutable;
            }
        }

        /// <summary>
        /// 編集対象となっているリスト中の要素を返します．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.EditTarget"/>
        /// <value>The edit target.</value>
        public OwnerType EditTarget {
            get {
                return context.EditTarget;
            }
        }

        /// <summary>
        /// 現在 編集中のフィールドを返します．
        /// </summary>
        /// <value>The editing.</value>
        public EditorType Editing {
            get {
                return context.Editing;
            }
        }

        /// <summary>
        /// 現在編集中のフィールドがあるかどうかを返します．
        /// </summary>
        /// <value>The is editing any.</value>
        public bool IsEditingAny {
            get {
                return context.IsEditing;
            }
        }


        /// <summary>
        /// <code>FieldList</code>をMBDataに変換したものを返します．
        /// </summary>
        /// <value>The serializable list.</value>
        public List<MBData> SerializableList {
            get {
                return context.ReadonlyFieldList.Select ((arg) => {
                    return arg.ToSerializable ();
                }).ToList ();
            }
        }

        /// <summary>
        /// 指定されたフィールドが編集中かどうか調べます．
        /// 引数に指定するフィールドは<code>FieldList</code>の要素である必要があります．
        /// </summary>
        /// <returns>The editing.</returns>
        /// <param name="checkee">Checkee.</param>
        public bool IsEditing (OwnerType checkee)
        {
            return ReferenceEquals (context.EditTarget, checkee);
        }

        /// <summary>
        /// シリアルを持つフィールドを探し，それをシリアル番号ごとに仕分けて返します．
        /// </summary>
        /// <returns>The serial fields.</returns>
        public Dictionary<int, List<OwnerType>> SearchSerialFields ()
        {
            return context.SearchSerialFields ();
        }

        /// <summary>
        /// 未使用のシリアル番号を取得します
        /// </summary>
        /// <value>The unused serial no.</value>
        public Option<int> UnusedSerialNo {
            get {
                return context.UnusedSerialNo;
            }
        }
    }
}

