using System;
using UIKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;
using TokyoChokoku.Patmark.Presenter;


namespace TokyoChokoku.Patmark.iOS.Presenter.Implementation
{
    class iOSViewProperties: IViewProperties
    {

        public iOSViewProperties(UIView view)
        {
            TheView = view;
		}


		private UIView TheView { get; }

        public Frame2D Bounds
        {
            get 
            {
                return TheView.Bounds.ToCommon();
            }
        }

        public Frame2D Frame
        {
            get
            {
                return TheView.Frame.ToCommon();
            }
        }
    }
}