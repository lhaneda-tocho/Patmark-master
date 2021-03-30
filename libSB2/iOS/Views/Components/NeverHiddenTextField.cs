using Foundation;
using System;
using UIKit;

using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class NeverHiddenTextField : UITextField
    {
        NSObject KeyboardShowObserver;
        NSObject KeyboardHideObserver;

        CGRect LastFrame = CGRect.Empty;
        CGRect KeyboardFrame = CGRect.Empty;

        public NeverHiddenTextField (IntPtr handle) : base (handle)
        {
            this.EditingDidEndOnExit += (sender, e) => {
                this.EndEditing(true);
                RestorePosition();
            };

            this.ShouldReturn += t =>
            {
                t.ResignFirstResponder();
                RestorePosition();
                return true;
            };
            // キーボード表示・非表示の通知開始
            KeyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWasShown);
            KeyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillBeHidden);
        }

        override
        public bool BecomeFirstResponder()
        {
            FixPositionConsideringKeyboard();
            return base.BecomeFirstResponder();
        }

        override
        protected void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // キーボード表示非表示の通知削除
            NSNotificationCenter.DefaultCenter.RemoveObserver(KeyboardShowObserver);
            NSNotificationCenter.DefaultCenter.RemoveObserver(KeyboardHideObserver);
        }

        private void KeyboardWasShown(NSNotification notification)
        {
            KeyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            FixPositionConsideringKeyboard();
        }

        private void KeyboardWillBeHidden(NSNotification notification)
        {
            KeyboardFrame = CGRect.Empty;
            RestorePosition();
        }


        private void FixPositionConsideringKeyboard()
        {
            if (IsFirstResponder && IsHiddenByKeyboard)
            {
                Frame = CreateFrameNotHiddenByKeyboard();
            }
        }

        private void RestorePosition()
        {
            if (LastFrame != CGRect.Empty)
            {
                Frame = LastFrame;
                LastFrame = CGRect.Empty;
            }
        }

        bool IsHiddenByKeyboard
        {
            get
            {
                return KeyboardFrame != CGRect.Empty && KeyboardFrame.Y < (Frame.Y + Frame.Height);
            }
        }

        private CGRect CreateFrameNotHiddenByKeyboard()
        {
            return new CGRect(
                Frame.X,
                KeyboardFrame.Y - Frame.Height,
                Frame.Width,
                Frame.Height
            );
        }
    }
}