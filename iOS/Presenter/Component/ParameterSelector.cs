using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace TokyoChokoku.Patmark.iOS.Presenter.Component
{
    [Register("ParameterSelector"), DesignTimeVisible(true)]
    public class ParameterSelector : UISegmentedControl
    {
        #region Init
        public ParameterSelector(object[] args) : base(args)
		{
			InitParameterSelector();
        }

        public ParameterSelector(UIImage[] images) : base(images)
		{
			InitParameterSelector();
        }

        public ParameterSelector(NSString[] strings) : base(strings)
		{
			InitParameterSelector();
        }

        public ParameterSelector(string[] strings) : base(strings)
		{
			InitParameterSelector();
        }

        public ParameterSelector()
		{
			InitParameterSelector();
        }

        public ParameterSelector(NSCoder coder) : base(coder)
		{
			InitParameterSelector();
        }

        public ParameterSelector(NSObjectFlag t) : base(t)
		{
			InitParameterSelector();
        }

        public ParameterSelector(IntPtr handle) : base(handle)
		{
			InitParameterSelector();
        }

        public ParameterSelector(NSArray items) : base(items)
		{
			InitParameterSelector();
        }

        public ParameterSelector(CoreGraphics.CGRect frame) : base(frame)
        {
            InitParameterSelector();
        }

        void InitParameterSelector()
        {
        }
        #endregion


        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
