
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
	/// <summary>
	/// Dynamic height view.
	/// 
    /// ソースコード元
    /// http://dev.classmethod.jp/smartphone/android-aspect-ratio-view/
	/// </summary>
	public class DynamicHeightView : View
    {
        public AspectRatio AspectRatioAttrib { get; set; } = AspectRatio.Unspecified;


        public DynamicHeightView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context, attrs);
        }

        public DynamicHeightView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context, attrs);
        }

        void Initialize(Context context, IAttributeSet attrs)
		{
            var a = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.DynamicHeightView, 0, 0);
			try
			{
                var id = Resource.Styleable.DynamicHeightView_aspectRatio;
				if (a.HasValue(id))
				{
					float v = a.GetFloat(id, 1.0F);
					AspectRatioAttrib = AspectRatio.Exact(v);
				}
			}
			finally
			{
				a.Recycle();
			}
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			var aspectAttrib = AspectRatioAttrib;
            if (aspectAttrib.IsUnspecified)
			{
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
