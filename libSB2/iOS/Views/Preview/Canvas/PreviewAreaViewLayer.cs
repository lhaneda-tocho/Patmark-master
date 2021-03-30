using System;
using UIKit;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class PreviewAreaViewLayer : CALayer
    {
        public static string MoveCanvasAnimationKey = "canvasinfo";


        /// <summary>
        /// レイヤーの再描画が予約されるたびに呼び出されます．
        /// Gridとの同期を目的に 利用されます．
        /// </summary>
        public event Action<PreviewAreaViewLayer> UpdateListener;


        [Export ("canvasTransformX")]
        public nfloat X { get; set; }

        [Export ("canvasTransformY")]
        public nfloat Y { get; set; }

        [Export ("canvasScale")]
        public nfloat Scale { get; set; } = 1;

        public nfloat DefaultCountXOfDrawableMilliGridsInView {
            get {
                return MachineModelNoManager.Get ().LatticeSize ().X;
            }
        }

        public static RectangleArea PreviewBounds {
            get {
                var sizei = MachineModelNoManager.Get ().LatticeSize ();
                return new RectangleArea (0, 0, sizei.X, sizei.Y);
            }
        }

        public CGRect VisibleFrameInCanvas {
            get {
                CanvasInfo info;
                TryGetAnimationValue (out info);
                var trans = info.ViewCanvasMatrix.ToCGAffine ();
                return trans.TransformRect (Bounds);
            }
        }

        public nfloat PixelPerMilli {
            get {
                return Bounds.Width / DefaultCountXOfDrawableMilliGridsInView;
            }
        }

        public CanvasInfo CanvasInfo {
            get {
                return new CanvasInfo (
                    (float)X,
                    (float)Y,
                    (float)Scale,
                    0,
                    (float)PixelPerMilli);
            }
        }

        bool TryGetAnimationValue (out CanvasInfo toValue)
        {
            var oldAnim = AnimationForKey (MoveCanvasAnimationKey);
            if (oldAnim == null) {
                toValue = CanvasInfo;
                return false;
            }
            var player = (PreviewAreaViewLayer)PresentationLayer;
            toValue = player.CanvasInfo;
            return true;
        }

        public CGRect Viewport { get; set; } = new CGRect ();

        public CanvasPresentationManager CreateManager (UIView view)
        {
            return new MyCanvasPresentationManager (this, view);
        }

        [Export ("initWithLayer:")]
        public PreviewAreaViewLayer (CALayer other) : base (other)
        {
            if (other is PreviewAreaViewLayer) {
                var mylayer = (PreviewAreaViewLayer)other;
                FieldManager = mylayer.FieldManager;
                X = mylayer.X;
                Y = mylayer.Y;
                Scale = mylayer.Scale;
                UpdateListener = mylayer.UpdateListener;
            }
        }

        [Export ("initWithLayer:")]
        public PreviewAreaViewLayer ()
        {
        }

        // We must copy our own state here from the original layer
        public override void Clone (CALayer other)
        {
            base.Clone (other);

            var resolved = (PreviewAreaViewLayer)other;
            FieldManager = resolved.FieldManager;
            X = resolved.X;
            Y = resolved.Y;
            Scale = resolved.Scale;
            UpdateListener = resolved.UpdateListener;
        }


        [Export ("needsDisplayForKey:")]
        static bool NeedsDisplayForKey (NSString key)
        {
            switch (key.ToString ()) {
            case "canvasTransformX":
            case "canvasTransformY":
            case "canvasScale":
                return true;
            default:
                return CALayer.NeedsDisplayForKey (key);
            }
        }

        public override void SetNeedsDisplay ()
        {
            base.SetNeedsDisplay ();
            if (UpdateListener != null) {
                UpdateListener (this);
            }
        }

        public override void SetNeedsDisplayInRect (CGRect r)
        {
            base.SetNeedsDisplayInRect (r);
            if (UpdateListener != null) {
                UpdateListener (this);
            }
        }

        public void PurgeListener ()
        {
            UpdateListener = null;
        }

        /// <summary>
        /// アニメーションオブジェクトの作成
        /// </summary>
        /// <returns>The move canvas animation.</returns>
        /// <param name="fromInView">From in view.</param>
        /// <param name="toInView">To in view.</param>
        /// <param name="yourDelegate">Your delegate.</param>
        static CAAnimation CreateMoveCanvasAnimation (CGPoint fromInView, CGPoint toInView, nfloat scaleFrom, nfloat scaleTo, CAAnimationDelegate yourDelegate)
        {
            var xanim = CABasicAnimation.FromKeyPath ("canvasTransformX");
            xanim.From = NSNumber.FromNFloat (fromInView.X);
            xanim.To = NSNumber.FromNFloat (toInView.X);

            var yanim = CABasicAnimation.FromKeyPath ("canvasTransformY");
            yanim.From = NSNumber.FromNFloat (fromInView.Y);
            yanim.To = NSNumber.FromNFloat (toInView.Y);

            var scaleAnim = CABasicAnimation.FromKeyPath ("canvasScale");
            scaleAnim.From = NSNumber.FromNFloat (scaleFrom);
            scaleAnim.To = NSNumber.FromNFloat (scaleTo);

            var group = new CAAnimationGroup ();
            group.Delegate = yourDelegate;
            group.Duration = 0.25;
            group.RepeatCount = 1;

            group.Animations = new CAAnimation [] {
                xanim, yanim, scaleAnim
            };

            return group;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <returns>The in context.</returns>
        /// <param name="ctx">The green component.</param>
        public override void DrawInContext (CGContext ctx)
        {
            UIGraphics.PushContext (ctx);

            base.DrawInContext (ctx);

            if (FieldManager == null)
                return;

            var canvas = new FieldCanvas (
                ctx,
                CanvasInfo
            );

            // 格子を描画
            DrawLattice (canvas);

            // フィールドを描画
            DrawOwnersExceptEdited (canvas);

            // 編集中のオブジェクトを描画する
            DrawEditedOwner (canvas);

            UIGraphics.PopContext ();
        }


        public iOSFieldManager FieldManager {
            get; set;
        } = null;

        static CGColor MarkinBlue {
            get {
                return UIColor.FromRGBA (97, 197, 231, 255).CGColor;
            }
        }

        static CGColor MarkinBrown {
            get {
                return UIColor.FromRGBA (53, 31, 20, 255).CGColor;
            }
        }

        /// <summary>
        /// 格子の色
        /// </summary>
        /// <value>The color of the lattice.</value>
        static CGColor LatticeColor {
            get {
                return new CGColor (0.75f, 1.0f);
            }
        }

        /// <summary>
        /// フィールドの色
        /// </summary>
        /// <value>The color of the field stroke.</value>
        static CGColor FieldColor {
            get {
                return UIColor.FromRGBA (0, 0, 0, 255).CGColor;
            }
        }

        /// <summary>
        /// 選択されたフィールドを囲む，もしくは塗りつぶす色
        /// </summary>
        /// <value>The color of the field selected.</value>
        static CGColor FieldSelectedColor {
            get {
                return new CGColor (0.2f, 0.4f, 1.0f, 1.0f);
                // return MarkinBlue;
            }
        }

        /// <summary>
        /// 選択されたフィールドを囲む線の太さ
        /// </summary>
        /// <value>The width of the field selected line.</value>
        static nfloat FieldSelectedLineWidth {
            get {
                return 1.7f;
            }
        }


        /// <summary>
        /// 問題のあるフィールドを囲む，もしくは塗りつぶす色
        /// </summary>
        /// <value>The color of the field selected.</value>
        static CGColor FieldErrorColor {
            get {
                return new CGColor (1.0f, 0.4f, 0.1f, 1.0f);
            }
        }

        /// <summary>
        /// 選択されたフィールドを囲む線の太さ
        /// </summary>
        /// <value>The width of the field selected line.</value>
        static nfloat FieldErrorLineWidth {
            get {
                return 1.0f;
            }
        }



        /// <summary>
        /// 基準点の枠の色
        /// </summary>
        /// <value>The color of the base point stroke.</value>
        static CGColor BasePointStrokeColor {
            get {
                return new CGColor (0.0f, 0.45f, 0.0f, 1.0f);
            }
        }

        /// <summary>
        /// 基準点の塗りつぶし色
        /// </summary>
        /// <value>The color of the base point fill.</value>
        static CGColor BasePointFillColor {
            get {
                return new CGColor (0.0f, 1.0f, 0.0f, 1.0f);
            }
        }



        /// <summary>
        /// 罫線を描画します。
        /// </summary>
        /// <returns>The lattice.</returns>
        /// <param name="canvas">Canvas.</param>
        void DrawLattice (FieldCanvas canvas)
        {
            var g = canvas.Context;

            g.SaveState ();
            {
                g.SetLineWidth (1);
                canvas.Context.SetStrokeColor (LatticeColor);

                var latticeSize = MachineModelNoManager.Get ().LatticeSize ();
                var maxX = 1 + latticeSize.X;
                var maxY = 1 + latticeSize.Y;

                for (var y = 0; y < maxY; y++) {
                    canvas.DrawLine (
                        canvas.CanvasViewMatrix,
                        0, y,
                        maxX - 1, y
                    );
                }

                for (var x = 0; x < maxX; x++) {
                    canvas.DrawLine (
                        canvas.CanvasViewMatrix,
                        x, 0,
                        x, maxY - 1
                    );
                }
            }
            g.RestoreState ();
        }

        void DrawOwnersExceptEdited (FieldCanvas canvas)
        {
            var ownerList = FieldManager.FieldList;
            var g = canvas.Context;

            g.SaveState ();
            {
                g.SetTextDrawingMode (CGTextDrawingMode.Fill);
                // プレビューの領域
                var previewBounds = PreviewBounds;

                // 全ての Ownerを 巡る
                foreach (var owner in ownerList) {
                    var drawable = owner.Drawable;

                    // 編集中のと同じオブジェクトは 描画しない．
                    if (FieldManager.IsEditing (owner))
                        continue;

                    if (owner.PreciseCollision.InBox (previewBounds)) {
                        // 通常描画
                        // フィールドの描画
                        canvas.Context.SaveState ();
                        {
                            g.SetStrokeColor (FieldColor);
                            g.SetFillColor (FieldColor);
                            drawable.Draw (canvas);
                        }
                        canvas.Context.RestoreState ();
                    } else {
                        // はみ出しているオブジェクトは強調表示
                        canvas.Context.SaveState ();
                        {
                            g.SetStrokeColor (FieldColor);
                            g.SetFillColor (FieldColor);
                            drawable.Draw (canvas);
                        }
                        canvas.Context.RestoreState ();

                        // 枠の描画
                        canvas.Context.SaveState ();
                        {
                            g.SetStrokeColor (FieldErrorColor);
                            g.SetFillColor (FieldErrorColor);
                            g.SetLineWidth (FieldErrorLineWidth);
                            drawable.DrawBorder (canvas);
                        }
                        canvas.Context.RestoreState ();
                    }


                }
            }
            g.RestoreState ();
        }

        void DrawEditedOwner (FieldCanvas canvas)
        {
            var g = canvas.Context;

            if (FieldManager.IsEditingAny) {
                // プレビューの領域
                var previewBounds = PreviewBounds;
                var drawable = FieldManager.Editing.Drawable;
                var collision = FieldManager.Editing.PreciseCollision;


                if (collision.InBox (previewBounds)) {
                    // フィールドの描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (FieldColor);
                        g.SetFillColor (FieldColor);
                        drawable.Draw (canvas);
                    }
                    canvas.Context.RestoreState ();

                    // 枠の描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (FieldSelectedColor);
                        g.SetFillColor (FieldSelectedColor);
                        g.SetLineWidth (FieldSelectedLineWidth);
                        drawable.DrawBorder (canvas);
                    }
                    canvas.Context.RestoreState ();

                    // ベースポイントの描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (BasePointStrokeColor);
                        g.SetFillColor (BasePointFillColor);
                        drawable.DrawBasePoint (canvas);
                    }
                    canvas.Context.RestoreState ();
                } else {
                    // はみ出しているオブジェクトは強調表示
                    // フィールドの描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (FieldColor);
                        g.SetFillColor (FieldColor);
                        drawable.Draw (canvas);
                    }
                    canvas.Context.RestoreState ();

                    // 枠の描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (FieldErrorColor);
                        g.SetFillColor (FieldErrorColor);
                        g.SetLineWidth (FieldSelectedLineWidth);
                        drawable.DrawBorder (canvas);
                    }
                    canvas.Context.RestoreState ();

                    // ベースポイントの描画
                    canvas.Context.SaveState ();
                    {
                        g.SetStrokeColor (BasePointStrokeColor);
                        g.SetFillColor (BasePointFillColor);
                        drawable.DrawBasePoint (canvas);
                    }
                    canvas.Context.RestoreState ();
                }
            }
        }

        class MyCanvasPresentationManager : CanvasPresentationManager
        {
            readonly PreviewAreaViewLayer me;
            readonly UIView               presentationView;

            public MyCanvasPresentationManager (PreviewAreaViewLayer me, UIView view)
            {
                if (me == null || view == null)
                    throw new NullReferenceException ();
                this.me = me;
                presentationView = view;
            }

            public override RectangleArea VisibleArea {
                get {
                    var area = me.Viewport;
                    return new RectangleArea (area.X, area.Y, area.Width, area.Height);
                }
            }

            public override Maybe<UIView> View{
                get {
                    return presentationView.ToMaybe ();
                }
            }

            public override CanvasInfo CanvasInfo {
                get {
                    return me.CanvasInfo;
                }
            }

            public override CGPoint Position {
                get {
                    return new CGPoint (me.X, me.Y);
                }
            }

            public override nfloat Scale {
                get {
                    return me.Scale;
                }
            }

            public override void DrawRequest ()
            {
                me.SetNeedsDisplay ();
            }

            protected override void StopAnimation ()
            {
                var oldAnim = me.AnimationForKey (MoveCanvasAnimationKey);
                if (oldAnim != null) {
                    var player = (PreviewAreaViewLayer)me.PresentationLayer;
                    me.X = player.X;
                    me.Y = player.Y;
                    me.Scale = player.Scale;
                    me.RemoveAnimation (MoveCanvasAnimationKey);
                }
            }

            protected override void OnlyMoveCanvas (CanvasInfo to, bool animate)
            {
                if (!animate) {
                    me.X = to.X;
                    me.Y = to.Y;
                    me.Scale = to.Scale;
                    DrawRequest ();
                    return;
                }

                var fromPoint = new CGPoint (me.X, me.Y);
                var toPoint = new CGPoint (to.X, to.Y);
                var fromScale = me.Scale;
                var toScale = to.Scale;

                var anim = CreateMoveCanvasAnimation (
                    fromPoint,
                    toPoint,
                    fromScale,
                    toScale,
                    new AnimationDelegate (me));

                // アニメーションが始まる前からスタンバイしてもらう
                // アニメーションが終わった時に位置を戻さない
                anim.FillMode = CAFillMode.Both;

                // アニメーションが終わった時にデリゲートから取得削除できるようにする
                anim.RemovedOnCompletion = false;

                // 曲線の設定
                anim.TimingFunction = CAMediaTimingFunction.FromName (
                    CAMediaTimingFunction.EaseOut
                );

                // アニメーションの開始時間
                me.AddAnimation (anim, MoveCanvasAnimationKey);
            }

            protected override bool TryGetToValue (out CanvasInfo toValue)
            {
                var oldAnim = me.AnimationForKey (MoveCanvasAnimationKey);
                if (oldAnim == null) {
                    toValue = CanvasInfo;
                    return false;
                }
                var player = (PreviewAreaViewLayer)me.PresentationLayer;
                toValue = player.CanvasInfo;
                return true;
            }
        }

        sealed class AnimationDelegate : CAAnimationDelegate
        {
            readonly PreviewAreaViewLayer me;

            public AnimationDelegate (PreviewAreaViewLayer me)
            {
                this.me = me;
            }



            public override void AnimationStopped (CAAnimation anim, bool finished)
            {
                if (finished) {
                    var player = (PreviewAreaViewLayer)me.PresentationLayer;
                    me.X = player.X;
                    me.Y = player.Y;
                    me.Scale = player.Scale;
                    me.RemoveAnimation (MoveCanvasAnimationKey);
                }
            }
        }
    }
}

