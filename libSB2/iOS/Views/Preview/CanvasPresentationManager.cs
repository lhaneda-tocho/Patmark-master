using System;
using Functional.Maybe;
using CoreGraphics;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class CanvasPresentationManager
    {
        /// <summary>
        /// 描画ロジックが不要である場合に利用します．
        /// </summary>
        /// <returns>The empty.</returns>
        public static CanvasPresentationManager CreateEmpty ()
        {
            return new EmptyPresentationManager ();
        }

        /// <summary>
        /// ユーザから見える領域を返します
        /// </summary>
        /// <value>The visible area.</value>
        public abstract RectangleArea VisibleArea { get; }

        /// <summary>
        /// キャンバスのパラメータを取得します．
        /// アニメーション中は<code>GetCanvasInfo()</code>の内容は更新されません．
        /// 更新する場合は<code>StopAnimation()</code>を実行してください．
        /// </summary>
        /// <returns>The canvas info.</returns>
        public abstract CanvasInfo CanvasInfo { get; }

        /// <summary>
        /// キャンバスを表示する View を取得します．
        /// 表示する View が存在しない場合は <see cref="Maybe{Type}.Nothing"/> となります．
        /// </summary>
        /// <value>The view.</value>
        public abstract Maybe<UIView> View { get; }

        /// <summary>
        /// キャンバスの表示位置
        /// </summary>
        /// <value>The position.</value>
        public abstract CGPoint Position { get; }

        /// <summary>
        /// キャンバスの拡大率
        /// </summary>
        /// <value>The scale.</value>
        public abstract nfloat Scale { get; }

        /// <summary>
        /// キャンバスの表示更新
        /// </summary>
        /// <returns>The canvas.</returns>
        public abstract void DrawRequest ();

        /// <summary>
        /// アニメーションの停止
        /// アニメーションを停止した時の状態で<code>GetCanvasInfo()</code>の内容が更新されます．
        /// </summary>
        /// <returns>The animation.</returns>
        protected abstract void StopAnimation ();

        /// <summary>
        /// アニメーションの目標値を取得しようと試みます．
        /// この取得に成功するのはアニメーション実行されている時だけです．
        /// 取得に成功した場合，true を返して toValue を目標値に更新します．
        /// 取得に失敗した場合，falseを返して toValue をCanvasInfoプロパティ値で更新します．
        /// </summary>
        /// <returns>The get to value.</returns>
        /// <param name="toValue">To value.</param>
        protected abstract bool TryGetToValue (out CanvasInfo toValue);

        /// <summary>
        /// キャンバスの移動アニメーション
        /// この呼び出し中にアニメーションが実行されている時は，そのアニメーションの停止を行います．
        /// animate 引数に true が指定される場合は アニメーションします．
        /// animate 引数に false が指定されていれば アニメーションされません．
        /// </summary>
        /// <returns>The canvas.</returns>
        /// <param name="to">To.</param>
        /// <param name="animate">Animate.</param>
        /// <exception cref="NullReferenceException">to に null が指定された時．</exception>
        public void MoveCanvas (CanvasInfo to, bool animate)
        {
            StopAnimation ();
            OnlyMoveCanvas (to, animate);
        }

        /// <summary>
        /// アニメーションを実行のみを行います．
        /// このメソッドではアニメーションの停止が行われません．ユティリティメソッドを作るのに必要となります．
        /// </summary>
        /// <returns>The move canvas.</returns>
        /// <param name="to">To.</param>
        /// <param name="animate">Animate.</param>
        /// <exception cref="NullReferenceException">to に null が指定された時．</exception>
        protected abstract void OnlyMoveCanvas (CanvasInfo to, bool animate);

        void LookAtCanvasPoint (Position2D toPoint, bool amimate)
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            OnlyMoveCanvas (
                to.LookAtViewPoint(to.CanvasToView(toPoint), VisibleArea),
                amimate
            );
        }

        void LookAtCanvasPointIfNeeded (Position2D toPoint, bool amimate)
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            var toPointInView = to.CanvasToView (toPoint);
            if (toPointInView.inRect (VisibleArea))
                DrawRequest ();
            else
                OnlyMoveCanvas (
                    to.LookAtViewPoint (toPointInView, VisibleArea),
                    amimate
                );
        }

        /// <summary>
        /// フィールドの注目アニメーション
        /// </summary>
        /// <param name="owner">Owner.</param>
        public void LookAt (iOSOwner owner)
        {
            LookAtCanvasPoint (owner.Field.Parameter.Bounds.GetCenter (), true);
        }

        /// <summary>
        /// フィールドが画面から隠れてしまう時にフィールドの注目アニメーションを行います
        /// </summary>
        public void LookAtIfNeeded (iOSOwner owner)
        {
            LookAtCanvasPointIfNeeded (owner.Field.Parameter.Bounds.GetCenter (), true);
        }

        /// <summary>
        /// 拡大率の増加
        /// </summary>
        /// <returns>The scale.</returns>
        public void IncreseScale ()
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            OnlyMoveCanvas (to.CreateIncresedScale (), true);
        }

        /// <summary>
        /// 拡大率を加算し，アニメーションを実行します．
        /// </summary>
        /// <returns>The scale.</returns>
        public void DecreseScale ()
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            OnlyMoveCanvas (to.CreateDecresedScale (), true);
        }

        /// <summary>
        /// 拡大率を減算し，アニメーションを実行します．
        /// </summary>
        /// <returns>The scale.</returns>
        public void DefaultScale ()
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            OnlyMoveCanvas (to.CraeteDefaultScale (), true);
        }

        /// <summary>
        /// 表示位置を原点に戻し，アニメーションを実行します．
        /// </summary>
        /// <returns>The position.</returns>
        public void DefaultPosition ()
        {
            CanvasInfo to;
            TryGetToValue (out to);
            StopAnimation ();
            OnlyMoveCanvas (to.CreateDefaultPosition (), true);
        }

        // ---- View から ジェスチャーイベントを取得する ----
        public Maybe<UIPinchGestureRecognizer> PinchPreviewToScale ()
        {
            var start = CanvasInfo;
            var startPos = CanvasInfo.GetPosition ();
            var center = Position2D.Zero;
            return
                from view in View
                select new UIPinchGestureRecognizer ((sender) => {
                    switch (sender.State) {
                    case UIGestureRecognizerState.Began: {
                            start = CanvasInfo;
                            startPos = start.GetPosition ();
                            center = sender.LocationInView (view).ToPosition2D ();
                        }
                        break;

                    default: {
                            var inputScale = sender.Scale;
                            var newScale = inputScale * start.Scale;

                            // 任意中心での拡大縮小を表す行列．
                            var scaling =
                                AffineMatrix2D.NewTranslation (center) *
                                AffineMatrix2D.NewScaling (inputScale, inputScale) *
                                AffineMatrix2D.NewTranslation (-center);

                            var newPosition = scaling * startPos;

                            var nextInfo = start
                                          .SetPosition (newPosition)
                                          .SetScale ((float)(newScale));

                            OnlyMoveCanvas (nextInfo, false);
                        }
                        break;
                    }
                });
        }

        public Maybe<UIPanGestureRecognizer> PanPreviewToTranslate ()
        {
            var start = CanvasInfo;
            return
                from view in View
                select new UIPanGestureRecognizer ((sender) => {
                    switch (sender.State) {
                    case UIGestureRecognizerState.Began: {
                            start = CanvasInfo;
                            return;
                        }

                    default: {
                            var point = sender.TranslationInView (view).ToPosition2D ();
                            var newInfo = start.Move (point);
                            OnlyMoveCanvas (newInfo, false);
                        }
                        break;
                    }
                });
        }

        class EmptyPresentationManager : CanvasPresentationManager
        {
            CanvasInfo content = new CanvasInfo ();

            public override RectangleArea VisibleArea {
                get {
                    return new RectangleArea (0, 0, 1, 1);
                }
            }
            public override CanvasInfo CanvasInfo {
                get {
                    return content;
                }
            }

            public override Maybe<UIView> View {
                get {
                    return Maybe<UIView>.Nothing;
                }
            }

            public override CGPoint Position {
                get {
                    return new CGPoint (content.X, content.Y);
                }
            }

            public override nfloat Scale {
                get {
                    return content.Scale;
                }
            }

            public override void DrawRequest ()
            {
            }

            protected override void OnlyMoveCanvas (CanvasInfo to, bool animate)
            {
                if (to == null)
                    throw new NullReferenceException ();
                content = to;
            }

            protected override void StopAnimation ()
            {

            }

            protected override bool TryGetToValue (out CanvasInfo toValue)
            {
                toValue = content;
                return true;
            }
        }
    }
}

