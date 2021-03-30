

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    using System.Collections.Generic;
    using CoreGraphics;
    using Fields;
    using Functional.Maybe;

    public sealed class iOSFieldContext : FieldContext <iOSOwner, iOSEditor>
    {
        /// <summary>
        /// 初めてアプリを開いた時に表示するフィールドを追加します．
        /// </summary>
        /// <returns>The startup.</returns>
        public static void LoadDemo (iOSFieldContext dest)
        {
            var sketchbook = HorizontalText.Mutable.Create ();
            sketchbook.Parameter.Text = "MarkinBOX";
            sketchbook.Parameter.X = 0.5m;
            sketchbook.Parameter.Y = 2.0m;
            sketchbook.Parameter.Pitch /= 2;


            var touch = HorizontalText.Mutable.Create ();
            touch.Parameter.Text = "SINCE 2009";
            touch.Parameter.X = 0.5m;
            touch.Parameter.Y = 5.0m;
            touch.Parameter.Pitch /= 2;


            dest.TrySubmit (new iOSOwner (sketchbook.ToConstant ()));
            dest.TrySubmit (new iOSOwner (touch.ToConstant ()));
        }


        /// <summary>
        /// このオブジェクトを空の状態で初期化します．
        /// </summary>
        public static iOSFieldContext Create ()
        {
            return new iOSFieldContext ();
        }

        public static iOSFieldContext CreateFrom (ICollection <MBData> serializables) 
        {
            var context = Create ();
            context.TrySubmitAll (iOSOwner.From (FieldFactory.Create (serializables)));
            return context;
        }

        public static iOSFieldContext CreateFrom (ICollection<iOSOwner> list)
        {
            var context = Create ();
            context.TrySubmitAll (list);
            return context;
        }

        /// <summary>
        /// 対象のコンテキストを複製します．
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="source">Source.</param>
        public static iOSFieldContext CopyOf (iOSFieldContext source)
        {
            return new iOSFieldContext (source);
        }

        /// <summary>
        /// 対象のコンテキストを複製します．加えて．複製されたブジェクトの編集状態を適用し，編集を終了します．
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="source">Source.</param>
        public static iOSFieldContext CopyOfCommitted (iOSFieldContext source)
        {
            var copied = CopyOf (source);
            copied.TryCommitAndClose ();
            return source;
        }

        /// <summary>
        /// 対象のコンテキストを複製します．
        /// needscCommitが true である場合は，更に，複製されたブジェクトの編集状態を適用し，編集を終了します．
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="source">Source.</param>
        public static iOSFieldContext CopyOf (iOSFieldContext source, bool needsCommit)
        {
            if (needsCommit)
                return CopyOfCommitted (source);
            return CopyOf (source);
        }


        public iOSFieldContext ()
        {
        }

        public iOSFieldContext (FieldContext<iOSOwner, iOSEditor> source) : base (source)
        {
        }

        public override void Clone (FieldContext<iOSOwner, iOSEditor> source)
        {
            base.Clone (source);
        }

        /// <summary>
        /// 編集状態のフィールドと衝突判定を行います．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.SelectEditingOnCircle (Position2D, double)"/>
        /// <returns>The editig.</returns>
        /// <param name="center">Center.</param>
        /// <param name="presentationManager">Presentation manager.</param>
        public bool SelectEditing (CGPoint center, CanvasPresentationManager presentationManager)
        {
            var info = presentationManager.CanvasInfo;
            var viewToCanvas = info.ViewCanvasMatrix;
            return SelectEditingOnCircle (
                viewToCanvas * center.ToPosition2D (),
                TouchCircleRadius.Get (info.Scale, info.PixelPerMilli)
            );
        }

        /// <summary>
        /// 全てのフィールドと衝突判定を行います．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.SelectOnCircle (Position2D, double)"/>
        /// <param name="center">Center.</param>
        /// <param name="presentationManager">Presentation manager.</param>
        public Maybe<iOSOwner> Select (CGPoint center, CanvasPresentationManager presentationManager)
        {
            var info = presentationManager.CanvasInfo;
            var viewToCanvas = info.ViewCanvasMatrix;
            var maybeOwner = SelectOnCircle (
                viewToCanvas * center.ToPosition2D (),
                TouchCircleRadius.Get (info.Scale, info.PixelPerMilli)
            );
            if (maybeOwner == null)
                return Maybe <iOSOwner> .Nothing;
            return maybeOwner.ToMaybe ();
        }

        /// <summary>
        /// 全てのフィールドと衝突判定を行います．
        /// 編集中のオブジェクトとの判定を一番低い優先度に設定します．
        /// 編集中のオブジェクトがない場合は 
        /// <see cref="Select(CGPoint, CanvasPresentationManager)"/>
        /// と同じ動作になります．
        /// </summary>
        /// <seealso cref="FieldContext{OwnerType, EditorType}.SelectOnCircle (Position2D, double)"/>
        /// <param name="center">Center.</param>
        /// <param name="presentationManager">Presentation manager.</param>
        public Maybe<iOSOwner> SelectLastEditing (CGPoint center, CanvasPresentationManager presentationManager)
        {
            if (IsEditing)
                CollisionKit.LastCollision = EditTarget;
            return Select (center, presentationManager);
        }

        public bool IsEditTarget (iOSOwner checkee)
        {
            return ReferenceEquals (checkee, EditTarget);
        }
    }
}

