using System;
using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.RenderKit.Context;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.Assets;

using TokyoChokoku.Patmark.FieldCanvas;

namespace TokyoChokoku.Patmark.Presenter.Preview
{
    public class CommonPreView: PhantomView
    {
        public CommonPreView(IViewProperties prop): base(prop)
        {
        }


        ICommonPreviewData thePreviewData = new EmptyPreviewData();

        public ICommonPreviewData PreviewData
        {
            get { return thePreviewData; }
            set
            {
                thePreviewData = value ?? throw new NullReferenceException();
            }
        }

        /// <summary>
        /// 表示領域の範囲. ModelNoで決定される
        /// </summary>
        public EmbossArea Area => PreviewData.EmbossArea; //= new EmbossArea(33.0, 15.0);//new EmbossArea(15.0, 15.0);

        int currentPage = 0;
        public int CurrentPage
        {
			get
			{
                return currentPage;
			}
			set
			{
				var nextpage = PreviewData.HasPage(value) ? value : currentPage;
				currentPage = nextpage;
			}
        }

		/// <summary>
		/// メインスクリーン上での倍率
		/// </summary>
        public Size2D Scale
		{
			get
			{
				var baseW = Area.WidthPt;
				var baseH = Area.HeightPt;
				var vb = Bounds;
				var viewW = vb.W;
				var viewH = vb.H;

				var rateW = viewW / baseW;
				var rateH = viewH / baseH;
                return Size2D.Init(rateW, rateH);
			}
		}

        public CanvasInfo FieldCanvasInfo {
            get {
                var s = Scale;
                var min = Math.Max(s.W, s.H);
                var info = new CanvasInfo((float)min);
                return info;
            }
        }

        public void DrawGrid(Canvas canvas) {
            var grid = new GridSource().Also((it) =>
            {
                it.O = Pos2D.Zero().LengthUnit(Unit.mm);
                it.Range = Frame2D.Create(0, 0, Area.Widthmm, Area.Heightmm).LengthUnit(Unit.mm);
                it.Stride = 1.0.LengthUnit(Unit.mm);
            }).Bake(Unit.adobePt);

            canvas.Use(() =>
            {
                canvas.Scale(Scale);
                canvas.SetStrokeColor(ColorAssets.GridColor);
                canvas.StrokeLines(grid);
            });
        }

        public void DrawText(Canvas canvas) {
            var data  = thePreviewData;
            canvas.Use(() =>
            {
                var w = Unit.mm.ToAdobePt(data.TextSize.Widthmm);
                var h = Unit.mm.ToAdobePt(data.TextSize.Heightmm);
                var point = Size2D.Init(w, h);
                var bounds = CenterFrame(point.H * Scale.SY);
                var stride = Unit.mm.ToAdobePt(data.TextSize.Stridemm);

                canvas.Translate(bounds.Pos);
                canvas.Scale(Scale);

                var t = data.GetPageOrDefault(CurrentPage);

                // draw!!
                //canvas.ShowStringMonoSpaceAt(Pos2D.Zero(), t, point, stride);
                canvas.ShowFieldTextMonoSpaceAt(Pos2D.Zero(), t, point, stride);
            });
        }



        public override void Draw(Canvas canvas)
        {

            //
            DrawGrid(canvas);
            //
            DrawText(canvas);


			//var text = data.GetPageOrDefault(CurrentPage);
			//var pW = Unit.mm.ToAdobePt(data.TextSize.Widthmm ) * Scale.SX;
            //var pH = Unit.mm.ToAdobePt(data.TextSize.Heightmm) * Scale.SY;
            //var point = Size2D.Init(pW, pH);
            //var bounds = CenterFrame(point.H);
            //var stride = Unit.mm.ToAdobePt(data.TextSize.Stridemm) * Scale.SX;
            //canvas.ShowStringAt(bounds.Pos, text, point, stride);

            //デバック用の囲い線
			//canvas.StrokeLines(bounds.ToStrip());

			// おうぎ， はりの描画
			//var fan = new Fan((p)=>{
			//    p.InnerRadius = 70;
			//    p.ThicknessFromInner = 10;
			//    return p;
			//});

			//canvas.Use(()=>{
			//    canvas.Translate(bounds.Pos);
			//    canvas.Translate(20, 0);
			//    canvas.Stroke(fan);
			//});
		}

		#region Util

        Frame2D CenterFrame(double point)
		{
			var vb = Bounds;
            var rect = new Frame2D();
			rect.H = point;
			rect.W = vb.W;
			rect.X = 0f;
			rect.Y = vb.H / 2 - rect.H / 2;
			return rect;
		}

        #endregion
    }
}
