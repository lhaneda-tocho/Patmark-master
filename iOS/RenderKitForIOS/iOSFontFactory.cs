using System;
using Foundation;
using UIKit;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;

namespace TokyoChokoku.Patmark.iOS.RenderKitForIOS
{
    public class iOSFontFactory: IFontFactory
    {
        public CommonFont Create(string family, UnitPair<double, LengthUnit> size)
        {
            return new iOSCommonFont(family, size);
        }
    }

    public class iOSCommonFont: CommonFont
    {
        UIFont   NativeFont { get; }

        public iOSCommonFont(string family, UnitPair<double, LengthUnit> size): base(family, size)
        {
            NativeFont = UIFont.FromName(family, (nfloat)(SizePt));
        }

        public override Size2D TextSize(string text)
        {
            var nt = new NSString(text);
            return nt.GetSizeUsingAttributes(new UIStringAttributes(){
                Font = NativeFont
            }).ToCommon();
        }
    }
}
