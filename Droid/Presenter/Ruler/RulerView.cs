
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.Presenter.Preview;
using TokyoChokoku.Patmark.Presenter.Ruler;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace TokyoChokoku.Patmark.Droid.Presenter.Ruler
{
	using Custom;
	using Implementation;
	using RenderKitForDroid;

    public class RulerView : View
	{
		Paint mPaint = new Paint();
		public CommonRulerView CommonView { get; private set; }

        public float DotPerPt {
			get
			{
				return DroidViewProperties.DPP(this);
            }
        }

        public RulerView(Context context) :
            base(context)
        {
            Initialize();
        }

        public RulerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public RulerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
		{
            Injecter.InjectIfNeeded(Context);
			CommonView = new CommonRulerView(new DroidViewProperties(this));
        }

        public void InjectContentView(DroidRulerContent content)
		{
			CommonView.Content = content;
            content.RulerView = this;
		}

		protected override void OnDraw(Canvas canvas)
		{
            base.OnDraw(canvas);
            var dpp = DotPerPt;
            var commonCanvas = new CanvasForDroid(mPaint, canvas);
            commonCanvas.Translate(1, 1);
            commonCanvas.Scale(dpp, dpp);
			CommonView.Draw(commonCanvas);

			var p = new Paint(mPaint);
			p.SetStyle(Paint.Style.Stroke);
			p.StrokeWidth = 1f;
			canvas.DrawRect(new Rect(1, 1, Width-1, Height-1), p);
		}
	}
}
