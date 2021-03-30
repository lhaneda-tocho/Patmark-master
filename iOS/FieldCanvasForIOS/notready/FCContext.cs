using System;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;

namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    public class FCContext
    {
        public FieldCanvas Canvas   { get; }
        public FCContext(FieldCanvas canvas)
        {
            Canvas = canvas;
        }


        public void ShowChar(Frame2D pivot, char c)
        {
            //Canvas.ShowCharAt(pivot.Pos, c, pivot.Size);
        }

        public void ShowLogo(Frame2D pivot, int id)
        {
            //Canvas.DrawLogoIdHere();
        }
    }

}
