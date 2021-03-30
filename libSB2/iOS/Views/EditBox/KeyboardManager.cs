using Foundation;
using System;
using UIKit;
using CoreGraphics;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class KeyboardManager
    {
        NSObject showObserver;
        NSObject hideObserver;
        NSObject changeObserver;

        UIScrollView scrollable;

        public KeyboardManager (UIScrollView scrollable)
        {
            this.scrollable = scrollable;
        }

        /// <summary>
        /// キーボードの監視を始めます．
        /// キーボードが開いた時，テキストボックスが 隠れないようにスクロールするようにします．
        /// </summary>
        public void StartObserve ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            if (showObserver == null)
                showObserver = nc.AddObserver ("UIKeyboardWillShowNotification".ToNSString (), WillShowKeyboard);
            if (hideObserver == null)
                hideObserver = nc.AddObserver ("UIKeyboardWillHideNotification".ToNSString (), WillHideKeyboard);
            if (hideObserver == null)
                changeObserver = nc.AddObserver ("UIKeyboardDidChangeFrameNotification".ToNSString (), WillChangeFrameKeyboard);
        }

        /// <summary>
        /// キーボードの監視を終了します．
        /// </summary>
        public void StopObserve ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            if (showObserver != null)
                nc.RemoveObserver (showObserver);
            if (hideObserver != null)
                nc.RemoveObserver (hideObserver);
            if (changeObserver != null)
                nc.RemoveObserver (changeObserver);
            showObserver = null;
            hideObserver = null;
            changeObserver = null;
        }

        void WillShowKeyboard (NSNotification notify)
        {
            scrollable.FindFirstResponder ().Do (active => {
                // キーボードの大きさと表示までの時間を取得
                var keyboardInfo = FrameDurationData.FromKeyboardNotification (notify);
                // Active(FirstResponder)なUI部品と，キーボードの表示領域を スクロールView内での座標値として取得
                var activeBoundsInScrollable  = scrollable.ConvertRectFromView (active.Bounds, active);
                var keyboardFrameInScrollable = scrollable.ConvertRectFromView (keyboardInfo.Frame, null);
                // テキストフィールドがキーボードによって隠される範囲を求める．
                var hiddenHeight = calcHiddenHeight (activeBoundsInScrollable, keyboardFrameInScrollable);
                // 隠れた分だけ スクロールする．
                UpdateScrollViewSize (keyboardInfo, (nfloat)hiddenHeight);
            });
        }

        void WillChangeFrameKeyboard (NSNotification notify)
        {
            scrollable.FindFirstResponder ().Do (active => {
                // キーボードの大きさと表示までの時間を取得
                var keyboardInfo = FrameDurationData.FromKeyboardNotification (notify);
                // Active(FirstResponder)なUI部品と，キーボードの表示領域を スクロールView内での座標値として取得
                var activeBoundsInScrollable = scrollable.ConvertRectFromView (active.Bounds, active);
                var keyboardFrameInScrollable = scrollable.ConvertRectFromView (keyboardInfo.Frame, null);
                // テキストフィールドがキーボードによって隠される範囲を求める．
                var hiddenHeight = calcHiddenHeight (activeBoundsInScrollable, keyboardFrameInScrollable);
                // 隠れた分だけ スクロールする．
                UpdateScrollViewSize (keyboardInfo, (nfloat)hiddenHeight);
            });
        }

        void WillHideKeyboard (NSNotification notify)
        {
            RestoreScrollViewSize ();
        }

        void UpdateScrollViewSize (FrameDurationData keyboardInfo, nfloat moveSize)
        {
            UIView.BeginAnimations ("ResizeForKeyboard");
            UIView.SetAnimationDuration (keyboardInfo.Duration);
            // 下に moveSize分の余白を作る設定
            var contentInsets = new UIEdgeInsets (0, 0, keyboardInfo.Frame.Height, 0);
            // 余白の設定．スクロール時に表示される棒の設定も必要なので合わせて行う
            scrollable.ContentInset          = contentInsets;
            scrollable.ScrollIndicatorInsets = contentInsets;
            // スクロールを行う
            ScrollDeltaY (moveSize);
            UIView.CommitAnimations ();
        }

        void RestoreScrollViewSize ()
        {
            scrollable.ContentInset          = UIEdgeInsets.Zero;
            scrollable.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        // ---- Utility ----
        double calcHiddenHeight (CGRect hidee, CGRect hider)
        {
            var height = hidee.Bottom - hider.Top;
            return (height > 0) ? height : 0;
        }

        // 指定した量だけ Y方向下向きに スクロールする
        void ScrollDeltaY (double moveSize)
        {
            var offset = scrollable.ContentOffset;
            offset.Y += (nfloat) moveSize;
            scrollable.ContentOffset = offset;
        }

        class FrameDurationData
        {
            public CGRect Frame     { get; }
            public double Duration  { get; }



            FrameDurationData (CGRect frame, double duration)
            {
                Frame    = frame;
                Duration = duration;
            }


            public static FrameDurationData FromKeyboardNotification (NSNotification notifycation)
            {
                // キーボードの位置情報を取得
                var frame = UIKeyboard.FrameEndFromNotification (notifycation);

                // キーボード表示のアニメーション時間を取得
                var duration = UIKeyboard.AnimationDurationFromNotification (notifycation);

                return new FrameDurationData (frame, duration);
            }
        }
    }
}

