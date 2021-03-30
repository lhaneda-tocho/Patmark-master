using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class UITransparentView : UIView
	{
		public UITransparentView (IntPtr handle) : base (handle)
		{
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
	}
}
