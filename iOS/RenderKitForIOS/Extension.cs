using System;
using CoreGraphics;

using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;

namespace TokyoChokoku.Patmark.iOS.RenderKitForIOS
{
    public static class Extension
    {
        public static CGAffineTransform ToNative(this Affine2D it) {
            return new CGAffineTransform(
                (nfloat) it.RXX , (nfloat) it.RYX,
                (nfloat) it.RXY , (nfloat) it.RYY,
                (nfloat) it.PosX, (nfloat) it.PosY
            );
        }

        public static CGPoint ToNative(this Pos2D it) {
            return new CGPoint(
                (nfloat) it.X,
                (nfloat) it.Y
            );
        }

        public static Pos2D ToCommon(this CGPoint it) {
            return Pos2D.Init(it.X, it.Y);
		}


        public static Size2D ToCommon(this CGSize it) {
            return Size2D.Init(it.Width, it.Height);
        }

        public static CGSize ToNative(this Size2D it) {
            return new CGSize(it.W, it.H);
        }

		public static CGRect ToNative(this Frame2D it) {
            return new CGRect(it.X, it.Y, it.W, it.H);
        }

        public static Frame2D ToCommon(this CGRect it) {
            return Frame2D.Create(it.X, it.Y, it.Width, it.Height);
        }

        public static CGPath ToNativePath(this IStrip it)
        {
            var array = new CGPoint[it.Count];
            {
                var i = 0;
                foreach (var p in it)
                {
                    array[i] = p.ToNative();
                    i++;
                }
            }

			var np = new CGPath();
			np.AddLines(array);
            if(it.IsLoop)
                np.CloseSubpath();
            return np;
        }

        public static CGPoint[] ToNativePoints(this Line it)
        {
            var s = it.Start.ToNative();
            var e = it.End.ToNative();
            return new CGPoint[] {s, e};
        }

        public static CGColor ToNativeColor(this CommonColor it) 
        {
            return new CGColor(it.R, it.G, it.B, it.A);
        }
    }
}
