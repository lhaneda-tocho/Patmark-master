using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Presenter.Embossment
{
	using EmbossmentKit;
	using FieldEditor;
	using FieldPreview;
	using Ruler;

    public class EmbossmentPreviewer
    {
        // manager
        PageManager ThePageManager = new PageManager();

        public EmbossmentData TheEmbossmentData {
            get => ThePageManager.EmbossmentData;
            set => ThePageManager.EmbossmentData = value;
        }

        // ui
        /// <summary>
        /// プレビュー領域
        /// </summary>
        public FieldPreView ThePreviewArea { get; set; }

        /// <summary>
        /// 次のページへボタン
        /// </summary>
        public ImageButton TheForwardButton { get; set; }
        /// <summary>
        /// 前のページへボタン
        /// </summary>
        public ImageButton TheBackButton { get; set; }
        /// <summary>
        /// ベージ番号表示ラベル
        /// </summary>
        public PageLabels ThePageLabels { get; set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:TokyoChokoku.Patmark.Droid.Presenter.Embossment.EmbossmentPreviewer"/> class.
        /// </summary>
        /// <param name="thePreviewArea">The preview area.</param>
        /// <param name="theForwardButton">The forward button.</param>
        /// <param name="theBackButton">The back button.</param>
        /// <param name="theLabelCurrent">The label current.</param>
        /// <param name="theLabelTotal">The label total.</param>
        public EmbossmentPreviewer(
            FieldPreView thePreviewArea,
            ImageButton theForwardButton,
            ImageButton theBackButton,
            TextView theLabelCurrent,
            TextView theLabelTotal)
        {
            ThePreviewArea   = thePreviewArea   ?? throw new NullReferenceException();
            TheForwardButton = theForwardButton ?? throw new NullReferenceException();
            TheBackButton    = theBackButton    ?? throw new NullReferenceException();

            ThePageLabels = new PageLabels(theLabelCurrent, theLabelTotal);

            SetCallbacks();
        }

        void SetCallbacks()
        {
            TheForwardButton.Click += (sender,ev) => ForwardPage();
            TheBackButton   .Click += (sender,ev) => BackPage();
        }

        /// <summary>
        /// Update this instance state.
        /// </summary>
        public void Update()
        {
            var pageMng = ThePageManager;
            ThePreviewArea.PreviewData = pageMng.PreviewData;
            if (pageMng.IsEmpty) 
            {
                ThePageLabels.Clear();
            } else {
				var cp = ThePageManager.CurrentPageNumber;
				var tp = ThePageManager.TotalPageNumber;

                ThePageLabels.SetPages(cp+1, tp);
                ThePreviewArea.CurrentPage = cp;
			}
			ThePreviewArea.Invalidate();
        }

        public void SetPageIfPossible(int page)
        {
            ThePageManager.SetPageIfPossible(page);
            Update();
        }

        public void ForwardPage()
		{
			ThePageManager.ForwardIfPossible();
			Update();
        }

        public void BackPage()
		{
			ThePageManager.BackIfPossible();
			Update();
        }

        //
#region sub
        public class PageManager
		{
            EmbossmentData data = EmbossmentData.Empty;
            /// <summary>
            /// Gets or sets the embossment data.
            /// </summary>
            /// <value>The data.</value>
            public EmbossmentData EmbossmentData {
                get => data;
                set
                {
                    data = value ?? throw new NullReferenceException();
                    Rebuild();
                }
            }

            public IPreviewData PreviewData { get; private set; } = PreviewDataProvider.Empty;

            /// <summary>
            /// Gets a value indicating whether this
            /// <see cref="T:TokyoChokoku.Patmark.Droid.Presenter.Embossment.EmbossmentPreviewer.PageManager"/> is empty.
            /// </summary>
            /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
            public bool IsEmpty => data.IsEmpty;

            /// <summary>
            /// Gets the current page number.
            /// </summary>
            /// <value>The current page number.</value>
            public int CurrentPageNumber { get; private set; }

            /// <summary>
            /// Gets the total page number.
            /// </summary>
            /// <value>The total page number.</value>
            public int TotalPageNumber { get => PreviewData.CountPages; }

            /// <summary>
            /// Rebuild this instance state and cache.
            /// </summary>
            void Rebuild()
			{
                var spec = new Patmark.Settings.MachineModelNoRepositoryOfKeyValueStore().PullDirect();
                PreviewData = PreviewDataProvider.From(spec, data);
                CurrentPageNumber = 0;
            }

            /// <summary>
            /// Forward page if possible.
            /// </summary>
            public void ForwardIfPossible()
			{
				SetPageIfPossible(CurrentPageNumber + 1);
            }

            /// <summary>
            /// Back page if possible.
            /// </summary>
            public void BackIfPossible()
            {
                SetPageIfPossible(CurrentPageNumber - 1);
            }

            /// <summary>
            /// Sets the page if possible.
            /// </summary>
            /// <param name="next">Next.</param>
            public void SetPageIfPossible(int next)
            {
                if (PreviewData.HasPage(next))
                    CurrentPageNumber = next;
            }

        }

        /// <summary>
        /// Page labels.
        /// </summary>
        public class PageLabels
        {
            public readonly TextView Label_Current;
            public readonly TextView Label_Total;

            public PageLabels(TextView label_Current, TextView label_Total)
            {
                Label_Current = label_Current ?? throw new NullReferenceException();
                Label_Total = label_Total ?? throw new NullReferenceException();
            }

            public void SetPages(int current, int total)
            {
                Label_Current.Text = current.ToString();
                Label_Total.Text = total.ToString();
            }

            public void Clear()
            {
                Label_Current.Text = "-";
                Label_Total.Text = "-";
                Label_Current.Invalidate();
                Label_Total.Invalidate();
            }
        }
#endregion
    }
}

