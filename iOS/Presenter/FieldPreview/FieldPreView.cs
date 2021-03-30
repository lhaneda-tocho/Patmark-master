using System;
using System.ComponentModel;
using System.Linq;

using Foundation;
using UIKit;
using CoreGraphics;

using Monad;

using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.Presenter.Preview;
using TokyoChokoku.Patmark.Presenter.Ruler;

using TokyoChokoku.iOS.Custom;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;
using TokyoChokoku.Patmark.iOS.FieldCanvasForIOS;
using TokyoChokoku.Patmark.iOS.Presenter.Ruler;
using TokyoChokoku.Patmark.iOS.Presenter.Implementation;


namespace TokyoChokoku.Patmark.iOS.Presenter.FieldPreview
{
    [Register("FieldPreView"), DesignTimeVisible(true)]
    public class FieldPreView : BorderView, IComponent
    {

        #region IComponent implementation
        public ISite Site { get; set; }
        public event EventHandler Disposed;
        #endregion


        #region Properties
        CommonPreView CommonView { get; set; }


        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
				SetNeedsDisplay();
            }
        }

        public override CGRect Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
                SetNeedsDisplay();
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
                RefreshVariables();
                SetNeedsDisplay();
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
                SetNeedsDisplay();
            }
        }

        /// <summary>
        /// メインスクリーン上での倍率
        /// </summary>
        public Size2D Scale
        {
            get {
                return CommonView.Scale;
            }
        }
        #endregion

        private void RefreshVariables() {
            CurrentPage = 0;
        }

        #region Constructor
        public FieldPreView() { InitFieldPreView(); }
        public FieldPreView(NSCoder coder) : base(coder)  { /*InitFieldPreView();*/ } // awake from nib
        public FieldPreView(NSObjectFlag t): base(t)      { InitFieldPreView(); }
        public FieldPreView(IntPtr handle) : base(handle) {  }
        public FieldPreView(CGRect frame)  : base(frame)  { InitFieldPreView(); }


        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if (Site != null && Site.DesignMode)
            {
                Injection.Injecter.DesignTimeInit(); 
            }

            InitFieldPreView();

            if (Site != null && Site.DesignMode)
            {
                var sv = Superview;
                if (sv is RulerView)
                {
                    var ruler = sv as RulerView;
                    ruler.InjectContentView(RulerContent);
                }
            }
        }

        void InitFieldPreView()
        {
            var p = new iOSViewProperties(this);
            CommonView = new CommonPreView(p);
        }
        #endregion

        public PreviewRulerContent RulerContent
        {
            get {
                return new PreviewRulerContent(CommonView);
            }
        }

        #region Draw
        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);

            var ctx = UIGraphics.GetCurrentContext();
            var canvas = new CanvasForiOS(ctx);
            //CommonView.Draw(canvas);
            CommonView.DrawGrid(canvas);
            CommonView.DrawText(canvas);


			//canvas.ShowStringAt(frame.Location.ToCommon(), text, point, point);
            //Console.WriteLine(frame.Location);



            //Console.WriteLine("Scale = " + Scale.sy);
            //Console.WriteLine("Page");
            //Console.WriteLine(CurrentPage);
            //Console.WriteLine("Contents");
            //foreach (var i in data.Contents)
            //    Console.WriteLine(i);
            //Console.WriteLine("Output Text");
            //Console.WriteLine(text);
        }
        #endregion

        #region Utility

        UIFont GetMyFont(nfloat point)
        {
            var f = UIFont.SystemFontOfSize(point);
            return f;
        }

        NSParagraphStyle Style
        {
            get
            {
                var style = NSParagraphStyle.Default.MutableCopy() as NSMutableParagraphStyle;
                style.Alignment = UITextAlignment.Left;
                return style;
            }
        }

        UIStringAttributes CreateAttrib(NSParagraphStyle style, UIFont font)
        {
            var attr = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = font,
                ParagraphStyle = style
            };
            return attr;
        }

        /// <summary>
        /// テキストを描画するべき範囲．
        /// フォントによっては文字が余白にはみ出すので，描画前にHeightを設定する必要がある．
        /// 
        /// </summary>
        /// <returns>The frame.</returns>
        /// <param name="point">Point.</param>
        CGRect CenterFrame(double point)
        {
            var vb = Bounds;
            var rect = new CGRect();
            rect.Height = (nfloat)point;
            rect.Width = vb.Width;
            rect.X = 0f;
            rect.Y = vb.Height / 2 - rect.Height / 2;
            //rect.Height *= (nfloat)1.2;
            return rect;
        }

		/// <summary>
		/// 指定したフォントの描画領域を計算する．
		/// 
		/// </summary>
		/// <returns>The frame.</returns>
		/// <param name="target">Target.</param>
		/// <param name="font">Font.</param>
		CGRect DrawingFrame(CGRect target, UIFont font) {
            var frame = target;
            frame.Height = font.LineHeight;
            return frame;
        }
        #endregion


    }
}
