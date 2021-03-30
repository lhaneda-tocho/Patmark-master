using System;
using TokyoChokoku.Patmark.Presenter.Ruler;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.iOS.Presenter.Ruler;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;

namespace TokyoChokoku.Patmark.iOS.Presenter.FieldPreview
{
    public class ViewRulerContent : RulerViewContentAdapter
    {
        private readonly FieldPreviewController cnt;
        private FieldPreView Preview;
        private RulerView    Ruler;

        public ViewRulerContent(FieldPreviewController cnt, FieldPreView preview, RulerView ruler)
        {
            this.cnt = cnt;
            this.Preview = preview;
            this.Ruler = ruler;
        }


        public override Pos2D StartTick { get; } = Pos2D.Init(0, 0);

        public override Size2D TickRange {
            get {
                var area = Preview.Area;
                return Size2D.Init(area.Widthmm, area.Heightmm);
            }
        }

        public override double Interval { get; } = 5;

        public override double IntervalSub { get; } = 1;

        public override Frame2D Frame
        {
            get
            {
                return Preview.ConvertRectToView(Preview.Bounds, Ruler).ToCommon();
            }
        }
    }
}
