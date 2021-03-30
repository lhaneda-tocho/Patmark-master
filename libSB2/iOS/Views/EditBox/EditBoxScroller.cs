using Foundation;
using System;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// キーボードが表示された時に編集領域をスクロールするためのビュー
    /// </summary>
    public partial class EditBoxScroller : UIScrollView
    {
        NSObject showObserver;
        NSObject hideObserver;


        public EditBoxScroller (IntPtr handle) : base (handle)
        {
            ScrollEnabled = false;
        }



        /// <summary>
        /// 自身を タッチできないようにする．
        /// </summary>
        /// <returns>The test.</returns>
        /// <param name="point">Point.</param>
        /// <param name="uievent">Uievent.</param>
        public override UIView HitTest (CoreGraphics.CGPoint point, UIEvent uievent)
        {
            UIView view = base.HitTest (point, uievent);
            if (view == this)
                return null;
            else
                return view;
        }

        /// <summary>
        /// キーボードの監視を始めます．
        /// キーボードが開いた時，テキストボックスが 隠れないようにスクロールするようにします．
        /// </summary>
        public void StartObserveKeyboard ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            if (showObserver == null)
                showObserver = nc.AddObserver ("UIKeyboardWillShowNotification".ToNSString (), WillShowKeyboard);
            if (hideObserver == null)
                hideObserver = nc.AddObserver ("UIKeyboardWillHideNotification".ToNSString (), WillHideKeyboard);
        }

        /// <summary>
        /// キーボードの監視を終了します．
        /// </summary>
        public void StopObserveKeyboard ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            if (showObserver != null)
                nc.RemoveObserver (showObserver);
            if (hideObserver != null)
                nc.RemoveObserver (hideObserver);
            showObserver = null;
            hideObserver = null;
        }

        void WillShowKeyboard (NSNotification notify)
        {
            var maybeFirstRes = this.FindFirstResponder ();
            // first responder 取得
            if (!maybeFirstRes.HasValue)
                return;
            var firstRes = maybeFirstRes.Value;

            // キーボードの位置情報を取得
            var keyboardFrame = UIKeyboard.FrameEndFromNotification (notify);

            // キーボード表示のアニメーション時間を取得
            var duration = UIKeyboard.AnimationDurationFromNotification (notify);

            // フォーカスされているオブジェクトの TableView上での位置を計算
            var activeFrameInMe = ConvertRectFromView (
                firstRes.Bounds, firstRes
            );

            // キーボードの座標を TableView上での座標に変換
            var keyboardFrameInMe = ConvertRectFromView (keyboardFrame, null);

            // テキストフィールドがキーボードによって隠される範囲を求める．
            var hiddenHeight = activeFrameInMe.Bottom - keyboardFrameInMe.Top;

            // 隠れた分だけ スクロールする
            // 隠れてなければ何もしない
            if (hiddenHeight > 0) {
                // 隠れてしまう場合は 移動
                UpdateScrollViewSize (hiddenHeight, duration);
            }
        }

        void WillHideKeyboard (NSNotification notify)
        {
            RestoreScrollViewSize ();
        }

        void UpdateScrollViewSize (nfloat moveSize, double duration)
        {
            BeginAnimations ("ResizeForKeyboard");
            SetAnimationDuration (duration);

            // 下に moveSize分の余白を作る設定

            var contentInsets = new UIEdgeInsets (0, 0, moveSize, 0);
            // 余白の設定．スクロール時に表示される棒の設定も必要なので合わせて行う
            ContentInset = contentInsets;
            ScrollIndicatorInsets = contentInsets;

            // 
            ContentOffset = new CGPoint (0, moveSize);

            CommitAnimations ();
        }

        void RestoreScrollViewSize ()
        {
            ContentInset = UIEdgeInsets.Zero;
            ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }


    }
}