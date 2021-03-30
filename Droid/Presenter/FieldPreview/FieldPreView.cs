
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Monad;

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

namespace TokyoChokoku.Patmark.Droid.Presenter.FieldPreview
{
    using Custom;
    using Implementation;

    using RenderKitForDroid;
    
    public class FieldPreView : DynamicHeightView
	{
        
		Paint mPaint = new Paint();

        // サイズ変更を要求するデリゲート
        public event Action<EmbossArea> OnSizeUpdateRequired;

        public CommonPreView CommonView { get; private set; }

		public float DotPerPt
		{
			get
			{
                return DroidViewProperties.DPP(this);
			}
		}

		/// <summary>
		/// プレビューに表示する内容
		/// </summary>
		/// <value>The preview data.</value>
		public IPreviewData PreviewData
		{
			get
			{
				var it = CommonView.PreviewData;
				return PreviewDataProvider.CastOrWrap(it);
			}
			set
			{
				CommonView.PreviewData = value;
                OnSizeUpdateRequired?.Invoke(Area);
				//RefreshVariables();
				//SetNeedsDisplay();
			}
		}

		/// <summary>
		/// 表示領域の範囲
		/// </summary>
		public EmbossArea Area { get { return CommonView.Area; } }

		/// <summary>
		/// 表示するページ番号です．
		/// 表示する対象を表すだけであり，このページの内容を取得できる保証はありません．
		/// </summary>
		/// <value>現在の表示ページ．</value>
		public int CurrentPage
		{
			get
			{
				return CommonView.CurrentPage;
			}
			set
			{
				CommonView.CurrentPage = value;
				//SetNeedsDisplay();
			}
		}

		/// <summary>
		/// メインスクリーン上での倍率
		/// </summary>
		public Size2D Scale
		{
			get
			{
				return CommonView.Scale;
			}
		}

        public Ruler.DroidRulerContent RulerContent
		{
			get
			{
                return new Ruler.DroidRulerContent(this);
			}
		}

        //public FieldPreview(Context context) :
        //    base(context)
        //{
        //    Initialize();
        //}

        public FieldPreView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public FieldPreView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            Injecter.InjectIfNeeded(Context);

            Console.WriteLine("Initialize Field Preview");
            CommonView = new CommonPreView(new DroidViewProperties(this));
            PreviewData = PreviewDataProvider.Empty;
        }


        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            // TODO: ここでサイズ計算を行なって プレビューの内容との整合性を保つ
            // NOTE: アニメーション対応で必要かも
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var dpp = DotPerPt;
			var commonCanvas = new CanvasForDroid(mPaint, canvas);
            commonCanvas.Translate(1,1);
            commonCanvas.Scale(dpp, dpp);
			CommonView.Draw(commonCanvas);

			var p = new Paint(mPaint);
			p.SetStyle(Paint.Style.Stroke);
            p.StrokeWidth = 1;
			canvas.DrawRect(new Rect(1, 1, Width-1, Height-1), p);
        } 
    }
}
