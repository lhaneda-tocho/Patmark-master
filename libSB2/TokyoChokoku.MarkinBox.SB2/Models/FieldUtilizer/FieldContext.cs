using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using Fields;
    using CollisionUtil;
    using System;
    using Monad;

    /// <summary>
    /// フィールドリストの変更処理と編集状態の遷移を担当する Model の基底クラスです．
    /// 複数のフィールドの保持，衝突判定によるフィールドの選択，編集状態に遷移を行うことができます．
    /// </summary>
    public abstract class FieldContext<OwnerType, EditorType>
        where OwnerType : Owner
        where EditorType : Editor
    {
        /// <summary>
        /// フィールドを追加された順に保持するリスト．
        /// </summary>
        protected
        List<OwnerType> FieldList { get; }

        /// <summary>
        /// フィールドを追加された順に保持するリスト．内部で保持するリストをラップしたものであり，
        /// このコンテキストの変更によって このリストの内容が変化します．
        /// </summary>
        /// <value>The readonly field list.</value>
        public
        IReadOnlyList<OwnerType> ReadonlyFieldList {
            get {
                return new ReadOnlyCollection<OwnerType> (FieldList).ToList ();
            }
        }

        /// <summary>
        /// フィールドリストをキャストしたものです．
        /// </summary>
        /// <value>The base field list copy.</value>
        public
        IReadOnlyList<Owner> BaseFieldListCopy {
            get {
                return FieldList.Cast<Owner> ().ToList ();
            }
        }

        /// <summary>
        /// フィールドを追加された順に保持するリスト．内部で保持するリストを複製したものであり，
        /// このコンテキストの変更の影響を受けません．
        /// </summary>
        /// <value>The field list copy.</value>
        public
        List<OwnerType> FieldListCopy {
            get {
                return new List<OwnerType> (FieldList);
            }
        }
        /// <summary>
        /// フィールドを追加された順に保持するリスト．内部で保持するリストを複製したものであり，
        /// このコンテキストの変更の影響を受けません．
        /// </summary>
        /// <value>The field list immutable.</value>
        public
        ImmutableList<OwnerType> FieldListImmutable {
            get {
                return ImmutableList.CreateRange (FieldList);
            }
        }

        /// <summary>
        /// 打刻器に送信できる形式に変換し，IEnumerable 型で返します．
        /// </summary>
        /// <value>The serializable.</value>
        public
        IEnumerable<MBData> Serializable {
            get {
                return
                    from owner in FieldList
                    select owner.ToSerializable ();
            }
        }

        /// <summary>
        /// フィールドリストを HashSet として複製します．
        /// </summary>
        /// <value> new hash set of field list.</value>
        protected
        HashSet<OwnerType>       FieldSetCopy {
            get {
                return new HashSet<OwnerType> (FieldList);
            }
        }

        /// <summary>
        /// タッチの衝突判定を管理するオブジェクト
        /// </summary>
        protected
        CollisionKit2<OwnerType> CollisionKit { get; }

        /// <summary>
        /// はみ出し判定用の衝突判定を管理するオブジェクト
        /// </summary>
        //protected
        //CollisionKit2<OwnerType> PreciseCollisionKit { get; }

        /// <summary>
        /// 編集中のフィールド
        /// null の時は未編集状態です．
        /// </summary>
        /// <value>The editing.</value>
        public EditorType Editing { get; protected set; }

        /// <summary>
        /// <code>Editing</code> に対応するリスト内のフィールドです．
        /// このフィールドと編集後の新しいフィールドを置換することで
        /// Editing の変更をリストに反映することができます．
        /// Editingが null の時は このプロパティも null になります．
        /// 
        /// </summary>
        /// <value>The editing target.</value>
        public OwnerType  EditTarget { get; protected set; }

        /// <summary>
        /// 何らかのフィールドが編集中の時に trueを返します．
        /// そうでなければ，つまり Editing が null の時は false となります．
        /// </summary>
        /// <value>The is editing.</value>
        public bool IsEditing {
            get { return Editing != null; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected FieldContext ()
        {
            FieldList    = new List<OwnerType> ();
            CollisionKit = new CollisionKit2<OwnerType> (new CollisionDataSource<OwnerType> (FieldList));
            //PreciseCollisionKit = new CollisionKit2<OwnerType> (new CollisionDataSource<OwnerType> (FieldList, true));
            Editing      = null;
        }

        /// <summary>
        /// FieldList の複製，CollisionKit の再構築, Editing, EditTargetの
        /// 複製を行い，インスタンスを初期化します．
        /// 
        /// コピー元と複製後の FieldList は別のインスタンスとなります．
        /// リストの要素は参照をコピーするだけで，複製を行いません．
        /// 
        /// CollisionKit は複製後のオブジェクトの内容から再構築します．
        /// 
        /// Editing は 可変オブジェクトのため，ディープコピーを行います．
        /// このとき，コピー元のEditingに設定されているデリゲートは複製されないことに注意してください．
        /// 
        /// EditTarget は 参照をそのままコピーします．
        /// 
        /// </summary>
        /// <param name="">.</param>
        protected FieldContext (FieldContext<OwnerType, EditorType> source)
        {
            FieldList = source.FieldListCopy;
            if (source.IsEditing)
                Editing = source.Editing.Duplicate <EditorType> ();
            EditTarget = source.EditTarget;
            CollisionKit = new CollisionKit2<OwnerType> (new CollisionDataSource<OwnerType> (FieldList));
            //PreciseCollisionKit = new CollisionKit2<OwnerType> (new CollisionDataSource<OwnerType> (FieldList, true));
        }

        /// <summary>
        /// FieldList，Editing, EditTargetの
        /// 複製を行います．
        /// 
        /// このオブジェクトに記録されている要素は破棄されます．
        /// リストの内容を破棄したのち，コピー元の要素を追加します．
        /// 
        /// Editing は 可変オブジェクトのため，ディープコピーを行います．
        /// このとき，コピー元のEditingに設定されているデリゲートは複製されないことに注意してください．
        /// 
        /// EditTarget は 参照をそのままコピーします．
        /// 
        /// </summary>
        /// <param name="">.</param>
        public virtual void Clone (FieldContext<OwnerType, EditorType> source)
        {
            FieldList.Clear ();
            FieldList.AddRange (source.FieldList);
            if (source.IsEditing)
                Editing = source.Editing.Duplicate<EditorType> ();
            EditTarget = source.EditTarget;
        }

        /// <summary>
        /// old に指定したフィールドを newer に指定したフィールドで置き換えます．
        /// フィールドを置き換えできた場合は true を返します．
        /// oldのフィールドが見つからず，置き換えできなかった時は false を返します．
        /// </summary>
        bool UnsafeReplace (OwnerType old, OwnerType newer)
        {
            var index = FieldList.IndexOf (old);
            if (index < 0)
                // フィールドが存在しない
                return false;
            FieldList [index] = newer;
            return true;
        }

        /// <summary>
        /// 指定したフィールドをこのオブジェクトから除去します．
        /// フィールドを除去できた場合は true を返します．
        /// 除去するべきフィールドがこのオブジェクトから見つからず，
        /// 除去出来なかった場合は false を返します．
        /// </summary>
        bool UnsafeDelete (OwnerType owner)
        {
            return FieldList.Remove (owner);
        }

        /// <summary>
        /// 指定したフィールドをこのオブジェクトに登録します．
        /// フィールドを登録できた場合は true を返します．
        /// フィールドがすでに存在しており，追加できなかった場合は false を返します．
        /// </summary>
        /// <returns>登録できた場合は true, そうでなければ false.</returns>
        /// <param name="owner">Owner.</param>
        public bool TrySubmit (OwnerType owner)
        {
            var index = FieldList.IndexOf (owner);
            if (index >= 0)
                // フィールドが存在する
                return false;
            // フィールドが存在しない
            FieldList.Add (owner);
            return true;
        }

        /// <summary>
        /// 指定したフィールドを可能な限りこのオブジェクトに登録します．
        /// フィールドを登録できた場合は その数を返します．
        /// 登録できたフィールドがない場合は 0 を返します．
        /// </summary>
        /// <returns>登録できた場合は true, そうでなければ false.</returns>
        /// <param name="owners">Owner.</param>
        public int TrySubmitAll (ICollection<OwnerType> owners)
        {
            // 登録済みのフィールドを振り落とす
            var set = FieldSetCopy;
            var submitable = owners.Where ((owner) => !set.Contains (owner)).ToList ();
            // 追加
            FieldList.AddRange (submitable);
            // 追加した数を返す．
            return submitable.Count;
        }

        /// <summary>
        /// old に指定したフィールドを newer に指定したフィールドで置き換えます．
        /// フィールドを置き換えできた場合は true を返します．
        /// oldのフィールドが見つからず，置き換えできなかった時は false を返します．
        /// 
        /// 編集中のものに関連づくフィールドを置き換えることはできません．
        /// その場合は false が返ります．
        /// 
        /// </summary>
        /// <returns>置き換えできた場合は true, そうでなければ false.</returns>
        /// <param name="old">Old.</param>
        /// <param name="newer">Newer.</param>
        public bool TryReplace (OwnerType old, OwnerType newer)
        {
            if (old == EditTarget)
                // 編集中のフィールドである場合
                return false;
            // 置き換え可能なフィールドである場合
            return UnsafeReplace (old, newer);
        }

        /// <summary>
        /// 指定したフィールドをこのオブジェクトから除去します．
        /// フィールドを除去できた場合は true を返します．
        /// 除去するべきフィールドがこのオブジェクトから見つからず，
        /// 除去出来なかった場合は false を返します．
        /// 
        /// 編集中のものに関連づくフィールドを削除することはできません．
        /// その場合は false が返ります．
        /// 
        /// </summary>
        /// <returns>除去に成功した場合は true, そうでなければ false.</returns>
        /// <param name="owner">Owner.</param>
        public bool TryDelete (OwnerType owner)
        {
            if (owner == EditTarget)
                // 編集中のフィールドである場合
                return false;
            // 削除可能なフィールドである場合
            return UnsafeDelete (owner);
        }

        /// <summary>
        /// このオブジェクトの内容を空の状態にします．
        /// 消去できたら true を返します．
        /// 編集中のフィールドがある場合は何もせず false を返します．
        /// </summary>
        /// <returns>The clear.</returns>
        public bool TryDeleteAll ()
        {
            if (IsEditing)
                // 編集中の時
                return false;
            // 消去可能な時
            FieldList.Clear ();
            return true;
        }

        /// <summary>
        /// このオブジェクトの内容を空の状態にします．
        /// 編集中のフィールドがある場合はそれも削除します．
        /// </summary>
        /// <returns>The delete all.</returns>
        public void ForceDeleteAll ()
        {
            if (IsEditing) {
                Editing = null;
                EditTarget = null;
            }
            FieldList.Clear ();
        }

        /// <summary>
        /// 指定したフィールドをリストから探し出し，それを編集状態にします．
        /// 編集状態になった場合は true を返します．
        /// リストからフィールドが見つからず，編集状態にできない場合は 
        /// 何もせず false を返します．
        /// </summary>
        /// <returns>The edit.</returns>
        /// <param name="owner">Owner.</param>
        public bool TryEdit (OwnerType owner)
        {
            var index = FieldList.IndexOf (owner);
            if (index < 0)
                // 見つからなかった時
                return false;
            // 見つかった時
            EditTarget = owner;
            Editing = owner.ToEditor <EditorType> ();
            return true;
        }

        /// <summary>
        /// 編集を強行します．
        /// </summary>
        /// <returns>The edit.</returns>
        /// <param name="owner">Owner.</param>
        public void ForceEdit (OwnerType owner)
        {
            var index = FieldList.IndexOf (owner);
            if (index < 0)
                // 見つからなかった時
                FieldList.Add (owner);
            EditTarget = owner;
            Editing = owner.ToEditor<EditorType> ();
        }

        /// <summary>
        /// 編集中のフィールドの内容をリストに適用します．
        /// リストに適用できた場合は true を返します.
        /// 編集中のフィールドが存在せず，適用できない場合は false を返します．
        /// 編集中のオブジェクトがリスト中に存在しない場合は
        /// 新しくリストにフィールドを追加し，true を返します．
        /// </summary>
        /// <returns>The commit editing.</returns>
        public bool TryCommitEditing ()
        {
            if (!IsEditing)
                // 編集中ではない場合
                return false;
            // 編集中の時
            var newTarget = Editing.ToOwner<OwnerType> ();
            if (!UnsafeReplace (EditTarget, newTarget))
                // 強制置き換え出来なかった場合は 追加．
                TrySubmit (newTarget);
            EditTarget = newTarget;

            // ジョグ機能(フィールドの位置を打刻ヘッドが追跡する機能)を起動します。
            EditTarget.Jogger.RequestToJog();

            return true;
        }

        /// <summary>
        /// 編集中のフィールドの内容をリストに適用し，編集を終了します．
        /// リストに適用できた場合は true を返します.
        /// 編集中のオブジェクトが存在しない場合は false を返します．
        /// 編集中のオブジェクトがリスト中に存在しない場合は
        /// 新しくリストにフィールドを追加し，true を返します．
        /// </summary>
        /// <returns>The commit editing.</returns>
        public bool TryCommitAndClose ()
        {
            if (!TryCommitEditing ())
                // 適用に失敗
                return false;
            // 適用に成功
            EditTarget = null;
            Editing    = null;
            return true;
        }

        /// <summary>
        /// 編集中のフィールドを複製し，それをリストに追加します．
        /// この複製を行う際に <code>TryCommitEditing</code> が実行されます．
        /// コピーしたあと true を返します．
        /// 編集中のフィールドが存在しない場合は false を返します．
        /// </summary>
        /// <returns>The copy editing.</returns>
        public bool TryDuplicateEditing ()
        {
            if (!TryCommitEditing ())
                // 適用に失敗した場合
                return false;
            // 適用できた場合
            var duplicatedOwner = EditTarget.Duplicate<OwnerType> ();
            FieldList.Add (duplicatedOwner);
            return true;
        }


        /// <summary>
        /// 編集中のフィールドを削除し，編集を終了します．
        /// 削除に成功した場合は true を返します．
        /// 編集中のフィールドが存在しない場合は なにもせず false を返します．
        /// リストに削除対象のフィールドがない場合は 編集を終了し，true を返します．
        /// </summary>
        /// <returns>The delete editing.</returns>
        public bool TryDeleteEditing ()
        {
            if (!IsEditing)
                // 編集中でない場合
                return false;
            var target = EditTarget;
            FieldList.Remove (target);
            EditTarget = null;
            Editing    = null;
            return true;
        }

        /// <summary>
        /// 編集中のフィールドに対して衝突判定を行います．
        /// 成功したら true, 失敗したら false を返します．
        /// 編集中のフィールドが存在しない場合も false を返します．
        /// </summary>
        /// <returns>The editing on circle.</returns>
        /// <param name="center">Center.</param>
        /// <param name="radius">Radius.</param>
        public bool SelectEditingOnCircle (Position2D center, double radius)
        {
            if (!IsEditing)
                return false;
            var col = Editing.Collision;
            return col.OnCircle (center.Homogeneous, radius);
        }

        /// <summary>
        /// 全てのフィールドに対して衝突判定を行います．
        /// 衝突判定に成功し，フィールドを取得できた場合はそのフィールドを返します．
        /// 衝突判定に失敗し，なにも得られなかった場合は null を返します．
        /// 編集中のオブジェクトがある場合，EditTargetの値が採用されます．
        /// 編集中のオブジェクトを選択したい場合は予め Commit する必要があります．
        /// </summary>
        /// <returns>The on circle.</returns>
        /// <param name="center">Center.</param>
        /// <param name="radius">Radius.</param>
        public OwnerType SelectOnCircle (Position2D center, double radius)
        {
            return CollisionKit.OnCircle (center.Homogeneous, radius);
        }

        /// <summary>
        /// シリアルを持つフィールドを探し，それをシリアル番号ごとに仕分けて返します．
        /// </summary>
        /// <returns>The serial fields.</returns>
        public Dictionary<int, List<OwnerType>> SearchSerialFields ()
        {
            var count = SerialSettingsManager.Instance.Settings.Count;


            // シリアルを持つ要素全て探索
            var keyValuePairEnumerableEnumerable =
                from everyone in FieldList

                where TextFieldCollector.IsSerialContainer (everyone)
                where TextFieldCollector.HasSerial (everyone)

                let parsed = TextFieldCollector.ParseText (everyone)

                select from someNode in parsed.ElementEnumerable

                       where someNode.FieldTextType == FieldTextType.Serial
                       select (SerialNode)someNode into serialNode

                       where serialNode.SerialNumberIsValid
                       select Tuple.Create (serialNode.SerialNumber, everyone);

            var keyValuePairs =
                keyValuePairEnumerableEnumerable.SelectMany ((manyPair) => manyPair);
            
            // ---- 探索結果を返す ----

            // シリアル設定の数に合わせて初期化
            var serialFieldDict = new Dictionary<int, List<OwnerType>> (count);

            // 空のリスト作成
            for (int i = 1; i <= count; ++i)
                serialFieldDict.Add (i, new List<OwnerType> ());

            foreach (var keyValuePair in keyValuePairs) {
                var key = keyValuePair.Item1;
                var value = keyValuePair.Item2;
                serialFieldDict [key].Add (value);
            }

            return serialFieldDict;
        }

        /// <summary>
        /// 未使用のシリアル番号を取得します
        /// </summary>
        /// <value>The unused serial no.</value>
        public Option<int> UnusedSerialNo {
            get {
                var dict = SearchSerialFields ();

                for (int i = 1; i <= dict.Count; ++i) {
                    if (dict[i].Count == 0)
                        return Option.Return(() => i);
                }

                return Option.Nothing<int>();
            }
        }

        sealed class CollisionDataSource <Type> : CollisionKitDataSource<Type>
            where Type : Owner
        {
            readonly bool       usesPreciseCollision;
            readonly List<Type> list;

            public CollisionDataSource (List<Type> list)
            {
                this.usesPreciseCollision = false;
                this.list = list;
            }

            public CollisionDataSource (List<Type> list, bool usesPreciseCollision)
            {
                this.usesPreciseCollision = usesPreciseCollision;
                this.list = list;
            }

            public IList<Type> DataList ()
            {
                return list;
            }

            public ICollision GetCollision (Type value)
            {
                if (usesPreciseCollision)
                    return value.PreciseCollision;
                else
                    return value.Collision;
            }
        }
    }
}

