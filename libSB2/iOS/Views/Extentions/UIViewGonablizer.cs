using System;
using System.Drawing;
using UIKit;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	static class UIViewGonablizer
	{
		public static Dictionary<UIView, NSLayoutConstraint[]> Goners = new Dictionary<UIView, NSLayoutConstraint[]>();

		public static void Init(){
			Goners.Clear ();
		}

		public static void Gone(this UIView view, bool gone){
			if (gone) {
				if (! Goners.ContainsKey (view)) {
					Goners [view] = new NSLayoutConstraint[] {
						NSLayoutConstraint.Create (
							view,
							NSLayoutAttribute.Width,
							NSLayoutRelation.Equal,
							null,
							NSLayoutAttribute.Width,
							1f,
							0
						),
						NSLayoutConstraint.Create (
							view,
							NSLayoutAttribute.Height,
							NSLayoutRelation.Equal,
							null,
							NSLayoutAttribute.Height,
							1f,
							0
						)
					};
					view.AddConstraints (Goners [view]);
				}
			}else {
				if (Goners.ContainsKey (view)) {
					view.RemoveConstraints (Goners [view]);
					Goners.Remove (view);
				}
			}
			if (view.Superview != null) {
				view.Superview.LayoutIfNeeded ();
			} else {
				view.LayoutIfNeeded ();
			}
		}
	}
}