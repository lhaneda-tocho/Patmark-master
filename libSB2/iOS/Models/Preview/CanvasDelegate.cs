using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface CanvasDelegate
    {
        void MoveCanvas   (CanvasInfo next, bool animate);
        void MoveViewport ();
        void SetNeedsDisplay ();
    }
}

