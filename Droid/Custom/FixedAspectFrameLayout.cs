using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Custom
{
    public class FixedAspectFrameLayout: FrameLayout
    {
        protected AspectRatio AspectRatioAttrib { get; set; } = AspectRatio.Unspecified;

        public FixedAspectFrameLayout(Context context, IAttributeSet attrs): base(context, attrs)
		{
            var a = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.FixedAspectFrameLayout, 0, 0);
            try
            {
                var id = Resource.Styleable.FixedAspectFrameLayout_aspectRatio;
                if (a.HasValue(id))
                {
                    float v = a.GetFloat(id, 1.0F);
                    AspectRatioAttrib = AspectRatio.Exact(v);
                }
            } finally {
				a.Recycle();
            }

		}

		/// <summary>
		/// Measure layout bounds.
		/// 
		/// レイアウトの範囲を決定します. 条件指定によって以下のように挙動が変化します.
		/// 
		/// a-1. width exactly (or at most), height unspecified
		///     height = width / aspect
		/// 
		/// a-2. width unspecified, height exactly (or at most)
		///     width = height * aspect
		/// 
		/// b. otherwize
		///     if height $gt; width / aspect: 
		///         height = height / aspect
		///     if width &gt; height * aspect:
		///         width = height * aspect
		/// 
		/// 
		/// </summary>
		/// <param name="widthMeasureSpec">Width measure spec.</param>
		/// <param name="heightMeasureSpec">Height measure spec.</param>
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			var aspectAttrib = AspectRatioAttrib;
            if (aspectAttrib.IsUnspecified) {
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }

			var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			var heightMode = MeasureSpec.GetMode(heightMeasureSpec);
			int w = MeasureSpec.GetSize(widthMeasureSpec);
			int h = MeasureSpec.GetSize(heightMeasureSpec);
            var aspect = aspectAttrib.Value;


            //Console.WriteLine(String.Format("width {0}, Height {1}", w, h));

			if (widthMode != MeasureSpecMode.Unspecified && heightMode == MeasureSpecMode.Unspecified)
			{
                //Console.WriteLine("Enter a-1");
				var newH = w / aspect;
				h = (int)newH;
			}
			else if (widthMode == MeasureSpecMode.Unspecified && heightMode != MeasureSpecMode.Unspecified)
			{
				//Console.WriteLine("Enter a-2");
				var newW = h * aspect;
				w = (int)newW;
			}
			else
			{
				//Console.WriteLine("Enter b");
				var newW = h * aspect;
				var newH = w / aspect;
				if (w >= newW)
					w = (int)newW;
				else
					h = (int)newH;
			}

			//Console.WriteLine(String.Format("width {0}, Height {1}", w, h));

            var mw = MeasureSpec.MakeMeasureSpec(w, MeasureSpecMode.Exactly);
            var mh = MeasureSpec.MakeMeasureSpec(h, MeasureSpecMode.Exactly);

            SetMeasuredDimension(mw, mh);
			base.OnMeasure(mw, mh);
		}
    }
}
