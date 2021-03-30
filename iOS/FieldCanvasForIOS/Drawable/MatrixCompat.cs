using System;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    public static class MatrixCompat
    {
        public static Pos2D ToPos2D(this Cartesian2D cart)  {
            return Pos2D.Init(cart.X, cart.Y);
        }
    }
}
