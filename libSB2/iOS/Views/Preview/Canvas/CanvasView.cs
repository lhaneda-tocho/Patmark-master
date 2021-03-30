using Foundation;
using System;
using UIKit;
using ObjCRuntime;
using CoreGraphics;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class CanvasView : UIView
    {


        /// <summary>
        /// アニメーション用のレイヤー
        /// </summary>
        /// <value>My layer.</value>
        public PreviewAreaViewLayer MyLayer {
            get {
                return (PreviewAreaViewLayer)Layer;
            }
        }

        /// <summary>
        /// プレビューが管理しているフィールド
        /// </summary>
        /// <value>The field manager.</value>
        public iOSFieldManager FieldManager {
            private set {
                MyLayer.FieldManager = value;
            }
            get {
                return MyLayer.FieldManager;
            }
        }

        public iOSFieldContext FieldContext {
            get;
        }

        public CanvasPresentationManager CanvasPresentationManager { get; }

        /// <summary>
        /// このビューで表示可能な領域を返します．
        /// </summary>
        /// <value>The viewport.</value>
        public CGRect Viewport {
            get {
                return MyLayer.Viewport;
            }
            set {
                MyLayer.Viewport = value;
            }
        }

        // ---- ジェスチャ設定用のプロパティ ---
        Maybe<UITapGestureRecognizer> tapFieldToSelect;
        public Maybe<UITapGestureRecognizer> TapFieldToSelect {
            get {
                return tapFieldToSelect;
            }

            private set {
                // 解放
                tapFieldToSelect.Do (RemoveGestureRecognizer);

                // 記憶
                tapFieldToSelect = value;

                // 設定
                value.Do (newer => {
                    newer.NumberOfTouchesRequired = 1;
                    newer.NumberOfTapsRequired = 1;
                    AddGestureRecognizer (newer);
                });
            }
        }

        Maybe<UIPanGestureRecognizer> panFieldToMove;
        public Maybe<UIPanGestureRecognizer> PanFieldToMove {
            get {
                return panFieldToMove;
            }
            private set {
                // 解放
                panFieldToMove.Do (RemoveGestureRecognizer);

                // 記憶
                panFieldToMove = value;

                // 設定
                value.Do (newer => {
                    newer.MaximumNumberOfTouches = 1;
                    newer.MinimumNumberOfTouches = 1;
                    AddGestureRecognizer (newer);
                });
            }
        }

        Maybe<UIPanGestureRecognizer> panPreviewToTranslate;
        public Maybe<UIPanGestureRecognizer> PanPreviewToTranslate {
            get {
                return panPreviewToTranslate;
            }
            private set {
                // 解放
                panPreviewToTranslate.Do (RemoveGestureRecognizer);

                // 記憶
                panPreviewToTranslate = value;

                // 設定
                value.Do (newer => {
                    newer.MaximumNumberOfTouches = 2;
                    newer.MinimumNumberOfTouches = 2;
                    AddGestureRecognizer (newer);
                });
            }
        }

        Maybe<UIPinchGestureRecognizer> pinchPreviewToScale;
        public Maybe<UIPinchGestureRecognizer> PinchPreviewToScale {
            get {
                return pinchPreviewToScale;
            }
            private set {
                // 解放
                pinchPreviewToScale.Do (RemoveGestureRecognizer);

                // 記憶
                pinchPreviewToScale = value;

                // 設定
                value.Do (AddGestureRecognizer);
            }
        }




        /// <summary>
        /// インスタンスの初期化，復元
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CanvasView (IntPtr handle) : base (handle)
        {
            // Manager 初期化
            CanvasPresentationManager = MyLayer.CreateManager (this);

            // デフォルト値を設定したFieldManager・FieldContextを作成
            var tuple = iOSFieldManager.Create ();
            iOSFieldContext.LoadDemo (tuple.Context);

            // FieldManager初期化
            FieldManager = tuple.Manager.ReplacePresentationManager (CanvasPresentationManager);

            // FieldContext初期化
            FieldContext = tuple.Context;

            // Bounds プロパティが変更されるときに 再描画するかどうか
            Layer.NeedsDisplayOnBoundsChange = true;

            // レイヤーの情報を修正
            Layer.ShouldRasterize = true;
            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ContentsScale = UIScreen.MainScreen.Scale;
        }

        public override void LayoutIfNeeded ()
        {
            base.LayoutIfNeeded ();
        }

        //
        // This instructs the runtime that whenever a MyView is created
        // that it should instantiate a CATiledLayer and assign that to the
        // UIView.Layer property
        //
        [Export ("layerClass")]
        public static Class LayerClass ()
        {
            return new Class (typeof (PreviewAreaViewLayer));
        }

        public override void SetNeedsDisplay ()
        {
            base.SetNeedsDisplay ();
            MyLayer.SetNeedsDisplay ();
        }

        public override void SetNeedsDisplayInRect (CGRect rect)
        {
            base.SetNeedsDisplayInRect (rect);
            MyLayer.SetNeedsDisplayInRect (rect);
        }

        // Canvas Event Source
        public void ListenTapFieldToSelect (Maybe<Action<TapField>> maybe)
        {
            TapFieldToSelect =
                from action in maybe
                select FieldManager.TapFieldToSelect (action);
        }

        public void ListenPanFieldToMove (Maybe<Action<PanField>> maybe)
        {
            PanFieldToMove =
                from action in maybe
                select FieldManager.PanFieldToMove (action);
        }

        public void ListenPanPreviewToTranslate (bool enable)
        {
            if (enable)
                PanPreviewToTranslate = CanvasPresentationManager.PanPreviewToTranslate ();
            else
                PanPreviewToTranslate = Maybe<UIPanGestureRecognizer>.Nothing;
        }

        public void ListenPinchPreviewToScale (bool enable)
        {
            if (enable)
                PinchPreviewToScale = CanvasPresentationManager.PinchPreviewToScale ();
            else
                PinchPreviewToScale = Maybe<UIPinchGestureRecognizer>.Nothing;
        }
        
    }
} 