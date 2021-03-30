using System;
using Android.Views;
using Android.Graphics;
namespace TokyoChokoku.Patmark.Droid.Presenter.Ruler
{
    using TokyoChokoku.Patmark.Presenter.Preview;
    using Presenter.Implementation;
    using Presenter.FieldPreview;

    public class DroidRulerContent: PreviewRulerContent
    {
        public RulerView RulerView { get; internal set; }
        FieldPreView PreView { get; }

        public DroidRulerContent(FieldPreView view): base(view.CommonView)
        {
            PreView = view;
        }


        public override RenderKit.Value.Frame2D Frame
        {
            get
            {
                if (RulerView == null)
                    throw new InvalidOperationException("DroidRulerContent must be attached a RulerView.");
                var localRect = RectFromTo(PreView, RulerView);
                var dpp = RulerView.DotPerPt;
                var x = (localRect.Left) / dpp;
                var y = (localRect.Top) / dpp;
                var w = (localRect.Right - localRect.Left) / dpp;
                var h = (localRect.Bottom - localRect.Top) / dpp;
                return RenderKit.Value.Frame2D.Create(x, y, w, h);
            }
        }



        static Rect RectFromTo(View fromView, View toView)
        {
            var fw = fromView.Width;
            var fh = fromView.Height;
            var frect = new Rect(0, 0, fw-2, fh-2);
            var trect = ConvertRect(frect, fromView, toView);
            trect.Left   -= 1;
            trect.Right  -= 1;
            trect.Top    -= 1;
            trect.Bottom -= 1;
            return trect;
        }

        static Rect ConvertRect(Rect fromRect, View fromView, View toView)
        {
            int[] fromCoord = new int[2];
            int[] toCoord = new int[2];
            fromView.GetLocationOnScreen(fromCoord);
            toView.GetLocationOnScreen(toCoord);

            int xShift = fromCoord[0] - toCoord[0];
            int yShift = fromCoord[1] - toCoord[1];

            Rect toRect = new Rect(fromRect.Left + xShift, fromRect.Top + yShift,
                    fromRect.Right + xShift, fromRect.Bottom + yShift);

            return toRect;
        }
    }


}
