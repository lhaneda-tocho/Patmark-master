
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content.Res;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Custom
{
    public class ImageRadioButton : RadioButton
    {
        //Rect     Bounds;
        Drawable Icon;

        public ImageRadioButton(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context, attrs);
        }

        public ImageRadioButton(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
		{
			Initialize(context, attrs);
        }

        void Initialize(Context context, IAttributeSet attrs)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ImageRadioButton);
            try
            {
                var drawable = a.GetDrawable(Resource.Styleable.ImageRadioButton_image);
                Icon = drawable;
            }finally{
                a.Recycle();
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (Icon == null)
                return;
			if (Checked)
			{
                Icon.SetState(new int[] { +Android.Resource.Attribute.StateChecked });
			}
			else
			{
				Icon.SetState(new int[] { -Android.Resource.Attribute.StateChecked });
            }

            // アイコンのアスペクト
			var aspect = GetAspect(Icon);
            // ビューのアスペクト
			var viewAspect = GetSelfAspect();

            if (aspect < 0 || viewAspect < 0)
			{
				Icon.SetBounds(0, 0, Width, Height);
            } else {
                if(aspect < viewAspect) {
                    // 横 : センタリング
                    // 縦 : 一致
                    var half   = (Height * aspect) / 2;
                    var center = Width / 2;
                    var left   = (int)(center - half);
                    var right  = (int)(center + half);
                    Icon.SetBounds(left, 0, right, Height);
                } else {
					// 横 : 一致
					// 縦 : センタリング
					var half   = (Width / aspect) / 2;
					var center = Height / 2;
					var top    = (int)(center - half);
                    var bottom = (int)(center + half);
                    Icon.SetBounds(0, top, Width, bottom);
                }
			}
			Icon.Draw(canvas);
        }

        /// <summary>
        /// calculate icon aspect.
        /// if the icon area is zero, this returns -1
        /// </summary>
        /// <returns>The aspect.</returns>
        /// <param name="icon">Icon.</param>
		float GetAspect(Drawable icon)
		{
			var w = icon.IntrinsicWidth;
            var h = icon.IntrinsicHeight;
            if (w == 0 || h == 0)
                return -1f;
            else
                return w / (float)h;
        }

        float GetSelfAspect()
        {
            var w = Width;
            var h = Height;
			if (w == 0 || h == 0)
				return -1f;
			else
				return w / (float)h;
        }

   //     void SetIconBounds() {
   //         var b = Bounds;
   //         var w = b.Width();
   //         var h = b.Height();
   //         if (w == 0 || h == 0)
   //         {
   //             Icon.Bounds = new Rect(0, 0, 0, 0);
   //             return;
   //         }
			//var a = w / (float)h;

			//var mw = Width;
        //    var mh = Height;
        //    var nw = (int)(Width  * a);
        //    var nh = (int)(Height / a);
        //    if(mh >= nh) {
        //        var center = mh / 2;
        //        var half   = nh / 2;
        //        Icon.Bounds = new Rect(0, center - half, mw, center + half);
        //    } else {
        //        var center = mw / 2;
        //        var half   = nw / 2;
        //        Icon.Bounds = new Rect(center - half, 0, center + half, mh);
        //    }
        //}
    }
}
