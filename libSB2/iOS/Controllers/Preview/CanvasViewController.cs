using System;
using UIKit;
using Functional.Maybe;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class CanvasViewController : UIViewController
    {
        public Maybe<Action<PanField>> PanFieldToMoveListener   { get; set; }
        public Maybe<Action<TapField>> TapFieldToSelectListener { get; set; }

        /// <summary>
        /// フィールドマネージャ
        /// </summary>
        /// <value>The field manager.</value>
        public iOSFieldManager FieldManager {
            get {
                return CanvasView.FieldManager;
            }
        }

        public iOSFieldContext FieldContext {
            get {
                return CanvasView.FieldContext;
            }
        }

        public CanvasPresentationManager CanvasPresentationManager {
            get {
                return CanvasView.CanvasPresentationManager;
            }
        }

        public CGRect Viewport {
            get {
                return CanvasView.Viewport;
            }
            set {
                CanvasView.Viewport = value;
            }
        }

        /// <summary>
        /// オペレーションモードに応じて ジェスチャの設定を行います．
        /// </summary>
        public void SetupGesture ()
        {
            if (OperationModeManager.IsAdministrator ()) {
                CanvasView.ListenTapFieldToSelect (TapFieldToSelectListener);
                CanvasView.ListenPanFieldToMove   (PanFieldToMoveListener);
            } else {
                CanvasView.ListenTapFieldToSelect (Maybe<Action<TapField>>.Nothing);
                CanvasView.ListenPanFieldToMove   (Maybe<Action<PanField>>.Nothing);
            }
            CanvasView.ListenPanPreviewToTranslate (true);
            CanvasView.ListenPinchPreviewToScale (true);
        }

        /// <summary>
        /// ジェスチャを破棄する
        /// </summary>
        public void DestroyGesture ()
        {
            CanvasView.ListenTapFieldToSelect (Maybe<Action<TapField>>.Nothing);
            CanvasView.ListenPanFieldToMove (Maybe<Action<PanField>>.Nothing);

            CanvasView.ListenPanPreviewToTranslate (false);
            CanvasView.ListenPinchPreviewToScale (false);
        }

        public CanvasViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            CanvasView.MyLayer.UpdateListener += (layer) => {
                var frame = layer.VisibleFrameInCanvas;
                // 縦用と横用に別々に計算
                var hinfo = new RulerInfo (frame.Left, frame.Right);
                var vinfo = new RulerInfo (frame.Top, frame.Bottom);

                // グリッドの間隔が違う場合は 小さい方に合わせる
                if (hinfo.GridScale < vinfo.GridScale)
                    vinfo.GridScale = hinfo.GridScale;

                if (vinfo.GridScale < hinfo.GridScale)
                    hinfo.GridScale = vinfo.GridScale;

                HorizontalRulerView.RulerInfo = hinfo;
                VerticalRulerView.RulerInfo = vinfo;
            };
        }
    }
}