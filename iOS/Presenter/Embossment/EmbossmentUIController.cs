using System;
using Foundation;
using UIKit;
using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    public partial class EmbossmentUIController : UIViewController
    {
        /// <summary>
        /// EmbossmentModeが変更された時に呼び出される．
        /// @para sender このインスタンス自身
        /// @para e      使用しません
        /// </summary>
        public EventHandler EmbossmentModeChanged;

        public EmbossmentMode EmbossmentMode
        {
            get
            {
                return Items.Bake();
            }
        }

        EmbossmentUIItems Items { get; } = new EmbossmentUIItems();

        /// <summary>
        /// プログラム側で初期化
        /// </summary>
        public EmbossmentUIController() : base("EmbossmentUIController", null)
        {
        }

        /// <summary>
        /// StoryBoardから初期化
        /// </summary>
        /// <param name="handle">Handle.</param>
        public EmbossmentUIController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void SetSegmentsEnabled(bool enabled)
        {
            TextSizeSegment.Enabled = enabled;
            ForceSegment.Enabled = enabled;
            QualitySegment.Enabled = enabled;
        }

        public void SelectSegments(
            TextSizeLevel textSizeLevel,
            ForceLevel forceLevel,
            QualityLevel qualityLevel
        )
        {
            if (TextSizeSegment != null)
            {
                TextSizeSegment.SelectedSegment = textSizeLevel.GetIndex();
                Items.SetTextSizeWithSegment(TextSizeSegment.SelectedSegment);
            }
            if (ForceSegment != null)
            {
                ForceSegment.SelectedSegment = forceLevel.GetIndex();
                Items.SetForceWithSegment(ForceSegment.SelectedSegment);
            }
            if (QualitySegment != null)
            {
                QualitySegment.SelectedSegment = qualityLevel.GetIndex();
                Items.SetQualityWithSegment(QualitySegment.SelectedSegment);
            }
        }


        #region Event Handler
        partial void TextSizeChanged(UISegmentedControl sender)
        {
            // FIXME: ロガーに変更する
            var segment = sender.SelectedSegment;
			Items.SetTextSizeWithSegment(segment);
			Console.WriteLine("Text Size Changed: " + Items.TextSizeLabel);
			FireEmbossmentModeChanged("TextSize");
        }

        partial void ForceChanged(UISegmentedControl sender)
		{
			// FIXME: ロガーに変更する
			var segment = sender.SelectedSegment;
			Items.SetForceWithSegment(segment);
			Console.WriteLine("Force Changed: " + Items.ForceLabel);
			FireEmbossmentModeChanged("Force");
        }

        partial void QualityChanged(UISegmentedControl sender)
		{
			// FIXME: ロガーに変更する
			var segment = sender.SelectedSegment;
			Items.SetQualityWithSegment(segment);
			Console.WriteLine("Quality Changed: " + Items.QualityLabel);
			FireEmbossmentModeChanged("Quality");
        }
        #endregion

        #region EventSource
        void FireEmbossmentModeChanged(string id) {
            EmbossmentModeChanged(this, new ModeChangeEventArgs(id));
        }
        #endregion

        public class ModeChangeEventArgs: EventArgs{
            public string ID { get; }
            public ModeChangeEventArgs(string id) { ID = id; }
        }
    }
}

