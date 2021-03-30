using System;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.UnitKit;

using AG=Android.Graphics;

namespace TokyoChokoku.Patmark.Droid.RenderKitForDroid
{
    public class DroidFontFactory: IFontFactory
    {
        public CommonFont Create(string family, UnitPair<double, LengthUnit> size)
        {
            return new DroidCommonFont(family, size);
        }
    }

    public class DroidCommonFont: CommonFont
    {
        AG.Paint    Paint = new AG.Paint();
        AG.Typeface typeface;

        public DroidCommonFont(string family, UnitPair<double, LengthUnit> size): base(family, size)
        {
            //var SizePt = size.Fold((v, u)=>u.ToAdobePt(v));
            typeface = AG.Typeface.Create(family, AG.TypefaceStyle.Normal);
			Paint.SetTypeface(typeface);
			Paint.TextSize = (float)SizePt;
            Paint.TextScaleX = 1;
            // TODO: 他Localeへの対応について考える
            Paint.TextLocale = Java.Util.Locale.Japan;
        }

        public override Size2D TextSize(string text)
        {
            var rect = new AG.Rect();
            Paint.GetTextBounds(text, 0, text.Length, rect);
            return Size2D.Init(rect.Width(), Paint.TextSize);
        }
    }
}
