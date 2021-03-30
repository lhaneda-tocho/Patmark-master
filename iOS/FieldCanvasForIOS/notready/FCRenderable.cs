using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    public abstract class FCRenderable {
        public abstract void Render(FCContext context, Frame2D Pivot);
    }



    public class FCChar: FCRenderable {
        public char    Char { get; set; }

        public override void Render(FCContext context, Frame2D Pivot)
        {
            context.ShowChar(Pivot, Char);
        }
    }


    public class FCLogoFrame: FCRenderable {
        public int ID { get; set; }
        public override void Render(FCContext context, Frame2D Pivot) {
            context.ShowLogo(Pivot, ID);
        }
    }
}
