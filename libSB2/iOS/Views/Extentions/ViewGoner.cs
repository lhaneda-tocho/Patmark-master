using System;
using System.Drawing;
using UIKit;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class ViewGoner
	{
		private UIView View;

		NSLayoutConstraint WidthConstraint;
		NSLayoutConstraint HeightConstraint;

		private bool _Gone = false;
		public bool Gone
		{
			get { return _Gone; }
			set {
				if (value) {
					if(!_Gone){
						View.AddConstraint (WidthConstraint);
						View.AddConstraint (HeightConstraint);
						//View.Hidden = true;
					}
				} else {
					if(_Gone){
						View.RemoveConstraint (WidthConstraint);
						View.RemoveConstraint (HeightConstraint);
						//View.Hidden = false;
					}
				}
				if (View.Superview != null) {
					View.Superview.LayoutIfNeeded ();
				} else {
					View.LayoutIfNeeded ();
				}
				_Gone = value;
			}
		}

		public ViewGoner(UIView view)
		{
			View = view;

			WidthConstraint = NSLayoutConstraint.Create (
				View,
				NSLayoutAttribute.Width,
				NSLayoutRelation.Equal,
				null,
				NSLayoutAttribute.Width,
				1f,
				0
			);
			WidthConstraint.Priority = 1000;

			HeightConstraint = NSLayoutConstraint.Create (
				View,
				NSLayoutAttribute.Height,
				NSLayoutRelation.Equal,
				null,
				NSLayoutAttribute.Height,
				1f,
				0
			);
			HeightConstraint.Priority = 1000;
		}
	}
}