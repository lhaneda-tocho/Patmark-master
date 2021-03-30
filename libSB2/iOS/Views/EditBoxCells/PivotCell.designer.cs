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
    [Register ("PivotCell")]
    partial class PivotCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton CCWButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton CWButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.CellFrameView TitleFrame { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CCWButton != null) {
                CCWButton.Dispose ();
                CCWButton = null;
            }

            if (CWButton != null) {
                CWButton.Dispose ();
                CWButton = null;
            }

            if (TitleFrame != null) {
                TitleFrame.Dispose ();
                TitleFrame = null;
            }
        }
    }
}