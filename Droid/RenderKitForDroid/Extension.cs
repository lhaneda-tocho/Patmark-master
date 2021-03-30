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
using TokyoChokoku.Patmark.Droid.Custom;

namespace TokyoChokoku.Patmark.Droid.RenderKitForDroid
{
    public static class Extension
    {
        public static AG.Color ToNative(this CommonColor color)
        {
            var bc = color.ByteRGBA;
            return AG.Color.Argb(bc.A, bc.R, bc.G, bc.B);
        }

        public static CommonColor ToCommon(this AG.Color ag)
        {
            return CommonColor.FromByteRGBA(ag.R, ag.G, ag.B, ag.A);
        }

        public static AG.Path ToNativePath(this IStrip strip)
        {
            var path = new AG.Path();
            if(strip.Count < 2) {
                return path;
            }
            var first = strip.First();
            path.MoveTo((float)first.X, (float)first.Y);
            foreach(var pos in strip.Skip(1)) {
                var x = (float)pos.X;
                var y = (float)pos.Y;
                path.LineTo(x, y);
            }

            if (strip.IsLoop)
                path.Close();

            return path;
        }

        public static AG.Matrix ToNative(this Affine2D transform)
        {
            var m = new AG.Matrix();
            m.SetValues(transform.ToRowOrderArrayAsFloat9());
            return m;
        }

        public static Affine2D ToAffine2D(this AG.Matrix transform)
        {
            var values = new float[9];
            transform.GetValues(values);

            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
            {
                if(values[6]!=0.0 || values[7]!=0.0 || values[8]!=1.0) // Affine map(matrix) => {{a,b,c},{d,e,f},{0,0,1}}
                    Log.Error("Non affine matrix is not able to convert to affine matrix"); 
            }
            else
            {
                if (!transform.IsAffine) // IsAffine() API level 21
                    Log.Error("Non affine matrix is not able to convert to affine matrix"); 
            }
            return Affine2D.InitColumns(
                values[0], values[1],
                values[3], values[4],
                values[6], values[7]
            );
        }

    }
}
