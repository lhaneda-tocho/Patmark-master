using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using CoreGraphics;
using TokyoChokoku.iOS.Custom;
using TokyoChokoku.Patmark.Presenter.Ruler;
using TokyoChokoku.Patmark.iOS.Presenter.Implementation;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;

namespace TokyoChokoku.Patmark.iOS.Presenter.Ruler
{
	[Register("RulerView"), DesignTimeVisible(true)]
    public class RulerView : BorderView, IComponent
    {
		public override CGRect Frame
		{
			get
			{
				return base.Frame;
			}
			set
			{
				base.Frame = value;
				SetNeedsDisplay();
			}
		}

		public override CGRect Bounds
		{
			get
			{
				return base.Bounds;
			}
			set
			{
				base.Bounds = value;
				SetNeedsDisplay();
			}
		}

        #region Init
        public RulerView()
        {
            InitRulerView();
        }

        public RulerView(Foundation.NSCoder coder) : base(coder)
        {
            InitRulerView();
        }

        public RulerView(Foundation.NSObjectFlag t) : base(t)
        {
            InitRulerView();
        }

        public RulerView(IntPtr handle) : base(handle)
        {
            InitRulerView();
        }

        public RulerView(CoreGraphics.CGRect frame) : base(frame)
        {
            InitRulerView();
        }

        void InitRulerView()
        {
            CommonView = new CommonRulerView(new iOSViewProperties(this));
        }
        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

			if (Site != null && Site.DesignMode)
			{
				Injection.Injecter.DesignTimeInit();
			}
        }

        #region IComponent implementation
        public ISite Site { get; set; }
        public event EventHandler Disposed;
        #endregion

        CommonRulerView CommonView { get; set; }

        public void InjectContentView(IRulerViewContent content)
        {
            CommonView.Content = content;
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);

            var ctx = UIGraphics.GetCurrentContext();
            var canvas = new CanvasForiOS(ctx);
            CommonView.Draw(canvas);
        }
    }
}
