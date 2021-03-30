using System;
using Android.Graphics;
namespace TokyoChokoku.Patmark.Droid.Presenter.Embossment
{
    public enum WifiButtonColor
    {
		Offline,
		Excluded,
        Online,
    }

    public static class WifiButtonColorExt {
        public static Color ToNativeColor(this WifiButtonColor it)
        {
            switch(it) {
                default: throw new ArgumentOutOfRangeException();
                case WifiButtonColor.Offline:
                    return Color.LightGray;
                case WifiButtonColor.Excluded:
                    return Color.Orange;
                case WifiButtonColor.Online:
                    return Color.Green;
            }
        }
    }
}
