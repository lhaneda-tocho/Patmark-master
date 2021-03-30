using System;
using UIKit;
using TokyoChokoku.iOS.Custom;
using TokyoChokoku.Patmark.iOS.Presenter.FieldEditor;
using Monad;
using TokyoChokoku.Patmark.iOS.Presenter.Component;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;


namespace TokyoChokoku.Patmark.iOS.Presenter.FieldPreview
{
    public partial class FieldPreviewController : UIViewController
    {
        public event EventHandler SendRequested;

        //Frame2D DefaultFrame;
        //Size2D RequiredSize;

        /// <summary>
        /// プレビューに表示する内容
        /// </summary>
        /// <value>The preview data.</value>
        internal IPreviewData PreviewData
        {
            get
            {
                return Preview.PreviewData;
            }
            set
            {
                if(Preview != null)
                    Preview.PreviewData = value;
                UpdateViewSize();
                UpdatePageCount();
            }
        }

        /// <summary>
        /// 現在のページ数．ページ番号は 0からスタートする．
        /// </summary>
        internal int CurrentPage {
            get
            {
                if (Preview == null)
                    return 0;
                else
                    return Preview.CurrentPage;
            }
            set
            {
                if (Preview != null)
                    Preview.CurrentPage = value;
                UpdatePageCount();
            }

        }

        #region Constructor
        public FieldPreviewController() : base("FieldPreviewController", null)
        {
        }

        public FieldPreviewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Ruler.InjectContentView(Preview.RulerContent);
            UpdateViewSize();
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            //Console.WriteLine("In Did Move To Parent View Controller");
            if (parent != null)
            {
                IPreviewData prev = PreviewDataProvider.Empty;
                PreviewData = prev;
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Event Handler

        partial void PageBack(FilledButton sender)
        {
            // FIXME: ロガーに変更する．
            Console.WriteLine("Page Back.");
            CurrentPage--;
        }

        partial void PageForward(FilledButton sender)
        {
			// FIXME: ロガーに変更する．
			Console.WriteLine("Page Forward.");
			CurrentPage++;
        }

        partial void AcceptSend(SendButton sender)
        {
            // FIXME: ロガーに変更する．
            Console.WriteLine("Accept Send.");
            SendRequested(this, null);
        }
        #endregion

        public void UpdateViewSize()
        {
            var viewBounds = Ruler.Bounds.ToCommon().Size - Size2D.Init((double)LeadingMargin.Constant, (double)TopMargin.Constant);

            double viewAspectW = viewBounds.W / viewBounds.H;
            double areaAspectW = Preview.Area.Widthmm / Preview.Area.Heightmm;

            // 表示内容の横アスペクト比がViewのものより小さければ 縦幅そのまま 横幅を小さく
            // 逆ならば 縦幅を小さく 横幅そのまま.
            if(viewAspectW >= areaAspectW) {
                double newH = viewBounds.H;
                double newW = newH * areaAspectW;
                UpdateViewSizeWithBounds(newW, newH);
            } else {
                double newW = viewBounds.W;
                double newH = newW / areaAspectW;
                UpdateViewSizeWithBounds(newW, newH);
            }
        }

        void UpdateViewSizeWithBounds(double w, double h)
        {
            var viewBounds = Ruler.Bounds.ToCommon().Size - Size2D.Init((double)LeadingMargin.Constant, (double)TopMargin.Constant);

            double trailingM = viewBounds.W - w;
            double bottomM   = viewBounds.H - h;

            TrailingMargin.Constant = (nfloat)trailingM;
            BottomMargin.Constant   = (nfloat)bottomM;

            Ruler.SetNeedsDisplay();
        }

        void UpdatePageCount() {
            if(Preview == null) {
                CurrentPageLabel.Text = "-";
                PageCountLabel.Text   = "-";
                return;
            }

            var data = PreviewData;
			if (data.IsEmpty)
			{
				CurrentPageLabel.Text = "-";
				PageCountLabel.Text = "-";
				return;
            }

            var c = CurrentPage;
            var t = data.CountPages;
            CurrentPageLabel.Text = (c+1).ToString();
            PageCountLabel.Text = t.ToString();
        }
    }
}

