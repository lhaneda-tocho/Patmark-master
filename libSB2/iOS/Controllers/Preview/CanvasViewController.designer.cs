// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [Register ("CanvasViewController")]
    partial class CanvasViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.CanvasView CanvasView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.HorizontalRulerView HorizontalRulerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.VerticalRulerView VerticalRulerView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CanvasView != null) {
                CanvasView.Dispose ();
                CanvasView = null;
            }

            if (HorizontalRulerView != null) {
                HorizontalRulerView.Dispose ();
                HorizontalRulerView = null;
            }

            if (VerticalRulerView != null) {
                VerticalRulerView.Dispose ();
                VerticalRulerView = null;
            }
        }
    }
}