using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class PropertyEditBox : UITableView
	{
        KeyboardManager keyboardManager;


        public PropertyEditBox ()
        {
            keyboardManager = new KeyboardManager (this);
        }

        public PropertyEditBox (NSCoder coder) : base (coder)
        {
            keyboardManager = new KeyboardManager (this);
        }

        public PropertyEditBox (NSObjectFlag t) : base (t)
        {
            keyboardManager = new KeyboardManager (this);
        }

        public PropertyEditBox (IntPtr handle) : base (handle)
        {
            keyboardManager = new KeyboardManager (this);
        }

        public PropertyEditBox (CoreGraphics.CGRect frame) : base (frame)
        {
            keyboardManager = new KeyboardManager (this);
        }

        public PropertyEditBox (CoreGraphics.CGRect frame, UITableViewStyle style) : base (frame, style)
        {
            keyboardManager = new KeyboardManager (this);
        }

        public void SetPropertyEditBoxSource (PropertyEditBoxSource source)
        {
            PurgeDelegate ();
            Source = source;
            SetDelegate (source);
            ReloadData ();
        }

        /// <summary>
        /// キーボードの監視を始めます．
        /// キーボードが開いた時，テキストボックスが 隠れないようにスクロールするようにします．
        /// </summary>
        public void StartObserveKeyboard ()
        {
            keyboardManager.StartObserve ();
        }

        /// <summary>
        /// キーボードの監視を終了します．
        /// </summary>
        public void StopObserveKeyboard ()
        {
            keyboardManager.StopObserve ();
        }

        void PurgeDelegate ()
        {
            var source = Source as PropertyEditBoxSource;
            if (source == null)
                return;
            source.PurgeTableViewUpdateEvent ();
        }

        void SetDelegate (PropertyEditBoxSource source)
        {
            source.OnInsertRequested += (indexPath, sender) => {
                NSIndexPath [] paths = {indexPath};
                BeginUpdates ();
                InsertRows (paths, UITableViewRowAnimation.Fade);
                EndUpdates ();
            };

            source.OnRemoveRequested += (indexPath, sender) => {
                NSIndexPath [] paths = { indexPath };
                DeleteRows (paths, UITableViewRowAnimation.Fade);
            };
        }

        /// <summary>
        /// 隠れている時は タッチできないようにする．
        /// </summary>
        /// <returns>The test.</returns>
        /// <param name="point">Point.</param>
        /// <param name="uievent">Uievent.</param>
        public override UIView HitTest (CoreGraphics.CGPoint point, UIEvent uievent)
        {
            if (Hidden)
                return null;
            else
                return base.HitTest (point, uievent);
        }





	}
}
