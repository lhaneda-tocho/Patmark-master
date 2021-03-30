using System;
using Foundation;

namespace TokyoChokoku.Patmark.iOS
{
    public static class StringLocalizer
    {
        public static string Localize(this String val)
        {
            return Localize(val, null);
        }

        public static string Localize(this String val, string comment)
        {
            return NSBundle.MainBundle.LocalizedString(val, comment);
        }
    }
}

