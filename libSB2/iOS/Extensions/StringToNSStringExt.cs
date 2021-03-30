using System;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class StringToNSStringExt
    {
        public static NSString ToNSString (this string text)
        {
            return new NSString (text);
        }
    }
}

