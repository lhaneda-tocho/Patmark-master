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
    public class ErasticSizeFrameLayout: FixedAspectFrameLayout
	{
		protected AspectRatio MaxHeightRate { get; set; } = AspectRatio.Unspecified;
		protected AspectRatio MaxWidthRate  { get; set; } = AspectRatio.Unspecified;

        /// <summary>
        /// 最大値，最小値いずれかを受け取る
        /// 両方指定されていた時はエラー
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        public ErasticSizeFrameLayout(Context context, IAttributeSet attrs): base(context, attrs)
		{
            var a = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.ErasticSizeFrameLayout, 0, 0);
			try
			{
                var idMinWidthRate = Resource.Styleable.ErasticSizeFrameLayout_maxWidthRate;
                var idMaxHeightRate = Resource.Styleable.ErasticSizeFrameLayout_maxHeightRate;
				if (a.HasValue(idMaxHeightRate))
				{
					float v = a.GetFloat(idMaxHeightRate, 1.0f);
                    MaxHeightRate = AspectRatio.Exact(v);
				}
                if(a.HasValue(idMinWidthRate))
                {
                    float v = a.GetFloat(idMinWidthRate , 1.0f);
                    MaxWidthRate = AspectRatio.Exact(v);
                }
			}
			finally
			{
				a.Recycle();
			}

            if(MaxHeightRate.IsSpecified && MaxWidthRate.IsSpecified) {
                throw new ArgumentException("Conflict Attributes");
            }
		}

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var wSpec = MeasureSpec.GetMode(widthMeasureSpec);
            var hSpec = MeasureSpec.GetMode(heightMeasureSpec);
            var w = MeasureSpec.GetSize(widthMeasureSpec);
            var h = MeasureSpec.GetSize(heightMeasureSpec);

            var maxWR = MaxWidthRate;
            var maxHR = MaxHeightRate;

            if (IsStiff(wSpec, hSpec))
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }

            if(WidthElastic(wSpec) && maxWR.IsSpecified)
            {
                int maxW = (int)(h * maxWR.Value);
                if(wSpec != MeasureSpecMode.AtMost)
                    maxW = Math.Min(maxW, w);
                base.OnMeasure(
					MeasureSpec.MakeMeasureSpec(maxW, MeasureSpecMode.AtMost),
					MeasureSpec.MakeMeasureSpec(h, MeasureSpecMode.Exactly)
                );
                return;
            }

			if (HeightElastic(hSpec) && maxHR.IsSpecified)
			{
                var maxH = (int)(w * maxHR.Value);
                if(hSpec != MeasureSpecMode.AtMost)
                    maxH = Math.Min(maxH, h);
				base.OnMeasure(
					MeasureSpec.MakeMeasureSpec(w, MeasureSpecMode.Exactly),
					MeasureSpec.MakeMeasureSpec(maxH, MeasureSpecMode.AtMost)
				);
				return;
			}

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        //static bool CheckConflict(AspectRatio max, AspectRatio min)
        //{
        //    return max.IsSpecified && min.IsSpecified && max.Value < min.Value;
        //}

        static bool IsStiff(MeasureSpecMode wSpec, MeasureSpecMode hSpec)
		{
            return wSpec == MeasureSpecMode.Exactly && hSpec == MeasureSpecMode.Exactly;
		}

        static bool WidthElastic(MeasureSpecMode wSpec)
        {
            return wSpec != MeasureSpecMode.Exactly;
        }

        static bool HeightElastic(MeasureSpecMode hSpec)
        {
            return hSpec != MeasureSpecMode.Exactly;
        }

   //     void debugOut(int widthMeasureSpec, int heightMeasureSpec) {
   //         Console.Write("Stiffness ");
			//Console.WriteLine(IsStiff(widthMeasureSpec, heightMeasureSpec));
			//Console.Write("Max ");
   //         Console.WriteLine(MeasureMax(MaxAspectRatioAttrib, widthMeasureSpec, heightMeasureSpec).Value);
			//Console.Write("Min ");
        //    Console.WriteLine(MeasureMin(MinAspectRatioAttrib, widthMeasureSpec, heightMeasureSpec).Value);
        //}

    }
}
