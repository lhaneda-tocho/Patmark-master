using System;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.Droid.RenderKitForDroid;
using TokyoChokoku.Patmark.Presenter;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace TokyoChokoku.Patmark.Droid.Presenter.Implementation
{
    using UnitKit;

    public class DroidViewProperties: IViewProperties
    {
        public DroidViewProperties(View view)
		{
			TheView = view;
        }

        View TheView { get; }

        public static float DPP(View view) {
			var dotPerInch = (float)view.Resources.DisplayMetrics.DensityDpi;
			var dpp = dotPerInch / 160f;
			//var dpp = (float)(dotPerInch * Unit.adobePt.ToInch(1.0));
			//for (int i = 0; i < 100; i++)
			//{
			//Console.WriteLine(dotPerInch);
			//Console.WriteLine(dpp);
			//}
			return dpp;
        }

        public float DotPerPt
        {
            get
            {
                return DPP(TheView);
            }
        }

		public Frame2D Bounds
		{
			get
            {
                var dpp = DotPerPt;
                var w = TheView.Width / dpp;
                var h = TheView.Height/ dpp;
				return Frame2D.Bounds(w-2, h-2);
			}
		}

		public Frame2D Frame
		{
			get
			{
                var dpp = DotPerPt;
                var x = (TheView.GetX()) / dpp;
				var y = (TheView.GetY()) / dpp;
				var w = TheView.Width / dpp;
				var h = TheView.Height/ dpp;
                return Frame2D.Create(x+1, y+1, w-2, h-2);
			}
		}
    }
}
