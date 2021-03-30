namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    using System;
    using CoreGraphics;
    using Foundation;
    using Functional.Maybe;
    using ToastIOS;
    using TokyoChokoku.MarkinBox.Sketchbook.Communication;
    using UIKit;
    using System.Threading.Tasks;


    /// <summary>
    /// Fieldの変更と再描画を担当する Controller クラスです．
    /// 
    /// ビューからイベントを取得するためのイベントリスナ生成メソッドを持ちます．
    /// イベントリスナの配線は UIViewController が行います．
    /// 
    /// ビューからのイベントごとに CanvasPresentationManger から適当なメソッドを呼び出すようにします．
    /// </summary>
    public class iOSFieldManager : BaseFieldManager<iOSOwner, iOSEditor, iOSFieldContext>
    {
        readonly CanvasPresentationManager presentationManager;



        /// <summary>
        /// コンテキストの複製
        /// </summary>
        /// <value>The copy context.</value>
        public iOSFieldContext ContextCopy {
            get {
                return iOSFieldContext.CopyOf (context);
            }
        }


        /// <summary>
        /// 空のインスタンスを作成します．
        /// コンテキストにアクセスできるようにマネージャーとコンテキストの参照を返します．
        /// </summary>
        public static Tuple Create ()
        {
            var mng = new iOSFieldManager ();
            var cnt = mng.context;
            return new Tuple (mng, cnt);
        }

        iOSFieldManager ()
        {
            presentationManager = CanvasPresentationManager.CreateEmpty ();
        }

        /// <summary>
        /// 新規作成コンストラクタ
        /// </summary>
        /// <seealso cref="BaseFieldManager::new ()"/>
        /// <param name="presentationManager">Presentation manager.</param>
        public iOSFieldManager (CanvasPresentationManager presentationManager)
        {
            this.presentationManager = presentationManager;
        }

        /// <summary>
        /// 再構築コンストラクタ
        /// presentationManager に null を指定すると，
        /// <see cref="CanvasPresentationManager.CreateEmpty ()"/> が代用されます．
        /// </summary>
        /// <seealso cref="BaseFieldManager::new (ContextType)"/>
        /// <param name="context">Context.</param>
        /// <param name="presentationManager">Presentation manager.</param>
        public iOSFieldManager (iOSFieldContext context, CanvasPresentationManager presentationManager) : base (context)
        {
            if (presentationManager == null)
                presentationManager = CanvasPresentationManager.CreateEmpty ();
            this.presentationManager = presentationManager;
        }

        /// <summary>
        /// 再描画用オブジェクトを入れ替えたものを返します．
        /// 返り値が持つ iOSFieldContext は元のオブジェクトのを共有します．
        /// </summary>
        /// <returns>The presentation manager.</returns>
        public iOSFieldManager ReplacePresentationManager (CanvasPresentationManager presentationManager)
        {
            return new iOSFieldManager (context, presentationManager);
        }

        /// <summary>
        /// 内部で持つコンテキストを複製したもので入れ替えたものを返します．
        /// 返り値が持つ CanvasPresentationManager は CanvasPresentationManager.CreateEmpty () で上書きされます．
        /// 
        /// needsCommit が true である場合は．更に．複製したものに対して編集状態の適用と編集の終了を行います．
        /// 
        /// </summary>
        /// <param name="needsCommit">needs commit.</param>
        /// <returns>The context.</returns>
        public Tuple Standalone (bool needsCommit)
        {
            var newContext = iOSFieldContext.CopyOf (context, needsCommit);
            var newManager = new iOSFieldManager (newContext, CanvasPresentationManager.CreateEmpty ());
            return new Tuple (newManager, newContext);
        }

        /// <summary>
        /// 対応するコンテキストの内容をそのままコピーします．
        /// </summary>
        public void Reload (iOSFieldContext context)
        {
            this.context.Clone (context);
            presentationManager.DrawRequest ();
        }

        /// <summary>
        /// 編集中の内容をフィールドリストに適用します．
        /// </summary>
        public void Refreash ()
        {
            context.TryCommitEditing ();
        }

        /// <summary>
        /// 指定した フィールドを 編集対象にします．
        /// リストに入っていない場合は新規作成します．
        /// </summary>
        /// <param name="owner">Owner.</param>
        public void Edit (iOSOwner owner)
        {
            context.TryCommitEditing ();
            context.ForceEdit (owner);
            presentationManager.DrawRequest ();
        }

        void UnsafeSelectField (CGPoint locationInView, Action<TapField> listener)
        {
            var maybeOwner = context.SelectLastEditing (locationInView, presentationManager);

            if (maybeOwner.HasValue) {
                var owner = maybeOwner.Value;
                if (context.IsEditTarget (owner)) {
                    // 判定されたけど編集中のものでした．
                    listener (TapField.To.SelectEditing.Create (this));
                    return;
                }

                // 別のフィールドに判定されました
                context.TryCommitAndClose ();
                context.ForceEdit (owner);
                listener (TapField.To.SelectOther.Create (this));

                presentationManager.LookAtIfNeeded (owner);
                return;
            }

            // 判定なし
            context.TryCommitAndClose ();
            listener (TapField.To.Deselect.Create (this));

            // 更新
            var canvasInfo = presentationManager.CanvasInfo;
            presentationManager.DrawRequest ();
        }


        // ---- View から ジェスチャーイベントを取得する ----
        /// <summary>
        /// フィールドをタップして選択するための UITapGestureRecognizer を作成します．
        /// PresentationManager からViewを取得できない場合は <see cref="Maybe{Type}.Nothing"/> となります．
        /// </summary>
        /// <returns>The field to select.</returns>
        /// <param name="listener">Listener.</param>
        public Maybe<UITapGestureRecognizer> TapFieldToSelect (Action<TapField> listener)
        {
            if (listener == null)
                listener = (ev) => { }; // empty
            return
                from view in presentationManager.View

                select new UITapGestureRecognizer ((sender) => {
                    if (sender.State != UIGestureRecognizerState.Ended)
                        return;

                    var point = sender.LocationInView (view);
                    UnsafeSelectField (point, listener);
                });
        }

        /// <summary>
        /// フィールドをタップして選択するための UITapGestureRecognizer を作成します．
        /// PresentationManager からViewを取得できない場合は <see cref="Maybe{Type}.Nothing"/> となります．
        /// </summary>
        /// <returns>The field to select.</returns>
        /// <param name="listener">Listener.</param>
        public Maybe<UITapGestureRecognizer> TapToAddField (Func<Position2D, iOSOwner> factory, Action<AddField> listener)
        {
            if (listener == null)
                listener = (ev) => { }; // empty
            return
                from view in presentationManager.View

                select new UITapGestureRecognizer ((sender) => {
                    if (sender.State != UIGestureRecognizerState.Ended)
                        return;

                    var point = sender.LocationInView (view).ToPosition2D ();
                    var viewCanvas = presentationManager.CanvasInfo.ViewCanvasMatrix;
                    var owner = factory (viewCanvas * point);
                    context.TrySubmit (owner);
                    context.ForceEdit (owner);
                    listener (new AddField (this));
                    presentationManager.LookAtIfNeeded (owner);
                    return;
                });
        }

        /// <summary>
        /// フィールドをパンして移動するための UIPanGestureRecognizer を作成します．
        /// PresentationManager からViewを取得できない場合は <see cref="Maybe{Type}.Nothing"/> となります．
        /// </summary>
        /// <returns>The field to select.</returns>
        /// <param name="listener">Listener.</param>
        public Maybe<UIPanGestureRecognizer> PanFieldToMove (Action<PanField> listener)
        {
            var ready = false;
            var fieldStart = new CGPoint ();
            var start = new CGPoint ();
            var end = new CGPoint ();

            if (listener == null)
                listener = (ev) => { }; // empty
            return
                from view in presentationManager.View

                select new UIPanGestureRecognizer ((sender) => {
                    switch (sender.State) {
                    case UIGestureRecognizerState.Began: {
                            start = sender.LocationInView (view);

                            if (context.SelectEditing (start, presentationManager)) {
                                // 編集中のフィールドの選択に成功
                                listener (PanField.To.SelectEditing.Create (this));
                            } else {
                                // 編集中のフィールドの選択に失敗
                                var owner = context.Select (start, presentationManager);
                                if (owner.HasValue) {
                                    // 選択したものを編集状態に切り替える
                                    context.TryCommitEditing ();
                                    context.ForceEdit (owner.Value);
                                    listener (PanField.To.SelectOther.Create (this));
                                } else {
                                    // 選択できなかったときは編集終了
                                    context.TryCommitAndClose ();
                                    listener (PanField.To.Deselect.Create (this));
                                }
                            }

                            // 編集状態の確認
                            if (!context.IsEditing) {
                                ready = false;
                                presentationManager.DrawRequest ();
                                return;
                            }

                            // フィールドの初期位置記憶
                            fieldStart.X = (nfloat)Editing.Field.Parameter.X;
                            fieldStart.Y = (nfloat)Editing.Field.Parameter.Y;
                            ready = true;
                            presentationManager.DrawRequest ();
                            return;
                        }

                    case UIGestureRecognizerState.Changed: {
                            if (!ready)
                                return;

                            end = sender.LocationInView (view);
                            var difference = new Position2D (
                                end.X - start.X,
                                end.Y - start.Y
                            );

                            var canvasInfo = presentationManager.CanvasInfo;
                            var canvasDistance = canvasInfo.ViewCanvasScRotMatrix * difference;

                            // フィールドの移動
                            var p = context.Editing.Field.Parameter;
                            p.XStore.Content = decimal.Round ( (decimal)(fieldStart.X + canvasDistance.X), 2);
                            p.YStore.Content = decimal.Round ( (decimal)(fieldStart.Y + canvasDistance.Y), 2);
                            listener (PanField.To.BePanning.Create (this));

                            // 更新
                            presentationManager.DrawRequest ();
                            return;
                        }

                    default: {
                            if (!ready)
                                return;

                            end = sender.LocationInView (view);
                            var difference = new Position2D (
                                end.X - start.X,
                                end.Y - start.Y
                            );

                            var canvasInfo = presentationManager.CanvasInfo;
                            var canvasDistance = canvasInfo.ViewCanvasScRotMatrix * difference;

                            // フィールドの移動
                            var p = context.Editing.Field.Parameter;
                            p.XStore.Content = decimal.Round ((decimal)(fieldStart.X + canvasDistance.X), 2);
                            p.YStore.Content = decimal.Round ((decimal)(fieldStart.Y + canvasDistance.Y), 2);

                            // 適用
                            context.TryCommitEditing ();

                            listener (PanField.To.HavePanned.Create (this));

                            // 更新
                            presentationManager.DrawRequest ();

                            ready = false;
                            return;
                        }
                    }
                });
        }


        // ---- View から キャンバスの状態変更イベントを取得する ----
        public void ActScaleUp (object sender, EventArgs e)
        {
            presentationManager.IncreseScale ();
        }

        public void ActScaleDown (object sender, EventArgs e)
        {
            presentationManager.DecreseScale ();
        }

        public void ActResetScale (object sender, EventArgs e)
        {
            presentationManager.DefaultScale ();
        }

        public void ActResetPosition (object sender, EventArgs e)
        {
            presentationManager.DefaultPosition ();
        }

        // ---- EditBox関係 ----
        public IEditBoxCommonDelegate CreateEditBoxCommonDelegate (iOSEditBoxManager editbox)
        {
            return new MyEditBoxCommonDelegate (this, editbox);
        }

        // ---- 追加・削除用メソッド ----
        public void Copy ()
        {
            if (context.TryDuplicateEditing ()) {
                presentationManager.DrawRequest ();
                Toast.MakeText ("Copied on the same position.".Localize (), Toast.LENGTH_SHORT).Show ();
            } else
                // TODO: エラーメッセージをローカライズする
                Toast.MakeText ("Erorr!! Fail to copy...", Toast.LENGTH_SHORT).Show ();
        }

        public void CheckThenDelete (iOSEditBoxManager editbox)
        {
            //Create Alert
            var alert = UIAlertController.Create (
                NSBundle.MainBundle.LocalizedString ("Clear the field", ""),
                NSBundle.MainBundle.LocalizedString ("Are you sure to clear this field?", ""),
                UIAlertControllerStyle.Alert
            );
            alert.AddAction (
                UIAlertAction.Create (
                    NSBundle.MainBundle.LocalizedString ("Cancel", ""),
                    UIAlertActionStyle.Cancel,
                    null)
            );
            alert.AddAction (
                UIAlertAction.Create (
                    NSBundle.MainBundle.LocalizedString ("OK", ""),
                    UIAlertActionStyle.Default, (action) => {
                        context.TryDeleteEditing ();
                        editbox.Close ();
                        presentationManager.DrawRequest ();
                    })
            );
            //Present Alert
            ControllerUtils.FindTopViewController ().PresentViewController (alert, true, null);
        }

        public void CheckThenDeleteAll ()
        {
            //Create Alert
            var alert = UIAlertController.Create (
                NSBundle.MainBundle.LocalizedString ("Clear all fields", ""),
                NSBundle.MainBundle.LocalizedString ("Are you sure to clear all fields?", ""),
                UIAlertControllerStyle.Alert
            );
            alert.AddAction (
                UIAlertAction.Create (
                    NSBundle.MainBundle.LocalizedString ("Cancel", ""),
                    UIAlertActionStyle.Cancel,
                    null)
            );
            alert.AddAction (
                UIAlertAction.Create (
                    NSBundle.MainBundle.LocalizedString ("OK", ""),
                    UIAlertActionStyle.Default,
                    (action) => {
                        ForceDeleteAll();
                    })
            );
            //Present Alert
            ControllerUtils.FindTopViewController ().PresentViewController (alert, true, null);
        }

        // 強制的に削除します
        public void ForceDeleteAll()
        {
            context.ForceDeleteAll ();
            presentationManager.DrawRequest ();
            // 結果は何でも構わない
            Task.Run(async () =>{
                await CommandExecuter.ClearPermanentMarking();
            });
        }


        // 編集ボックスからの変更を受け取るデリゲート
        class MyEditBoxCommonDelegate : IEditBoxCommonDelegate
        {
            readonly iOSFieldManager manager;
            readonly iOSEditBoxManager editbox;

            public MyEditBoxCommonDelegate (iOSFieldManager field, iOSEditBoxManager editbox)
            {
                manager = field;
                this.editbox = editbox;
            }

            public void Apply ()
            {
                manager.context.TryCommitEditing ();
                manager.presentationManager.DrawRequest ();
            }

            public void Rebuild ()
            {
                manager.context.TryCommitEditing ();
                editbox.Build (manager);
                manager.presentationManager.DrawRequest ();
            }
        }

        /// <summary>
        /// マネージャーとコンテキストの両方をまとめて返すための不変クラス
        /// </summary>
        public sealed class Tuple
        {
            public iOSFieldManager Manager { get; }
            public iOSFieldContext Context { get; }

            internal Tuple (iOSFieldManager manager, iOSFieldContext context)
            {
                Manager = manager;
                Context = context;
            }
        }

    }
}

