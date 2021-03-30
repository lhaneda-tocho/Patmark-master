using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.RenderKit.Context;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AG = Android.Graphics;
using AT = Android.Text;
using TokyoChokoku.Patmark.Droid.Custom;

namespace TokyoChokoku.Patmark.Droid.RenderKitForDroid
{
    public class PaintContext
    {
        public AG.Paint Paint { get; }
        public AG.Color FillColor { get; set; } = AG.Color.Argb(255, 0, 0, 0);
        public AG.Color StrokeColor { get; set; } = AG.Color.Argb(255, 0, 0, 0);

        public AT.TextPaint CreateTextPaint()
        {
            return new AT.TextPaint(Paint);
        }

        public PaintContext(AG.Paint initState)
        {
            if (initState == null)
                throw new NullReferenceException("Not allowed null");
            Paint = initState;
            // TODO: 他Localeへの対応について考える
            Paint.TextLocale = Java.Util.Locale.Japan;
        }

        public PaintContext Copy()
        {
            var dup = new PaintContext(new AG.Paint(Paint));
            dup.FillColor = FillColor;
            dup.StrokeColor = StrokeColor;
            return dup;
        }

        #region Feature

        public float StrokeWidth {
            get {
                return Paint.StrokeWidth;
            }
            set {
                Paint.StrokeWidth = value;
            }
        }

        public void StrokeMode()
        {
            Paint.SetStyle(AG.Paint.Style.Stroke);
            Paint.Color = StrokeColor;
        }

        public void FillMode()
        {
            Paint.SetStyle(AG.Paint.Style.Fill);
            Paint.Color = FillColor;
        }

        //public void S() {
        //    Paint.Text
        //}

        #endregion
    }
}
