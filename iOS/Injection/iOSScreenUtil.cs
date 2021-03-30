using System;
using TokyoChokoku.Patmark.Common;
using UIKit;

namespace TokyoChokoku.Patmark.iOS.Injection
{
    public class iOSScreenUtil: ScreenUtil
	{
        public override DPI getDPI()
        {
            // dot per point
            double dpp = UIScreen.MainScreen.Scale;
            // point to inch
            double dpi = dpp * 72.00;
            return new DPI(dpi);
        }
    }
}
