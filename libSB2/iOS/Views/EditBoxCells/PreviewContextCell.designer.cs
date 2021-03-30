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
    [Register ("PreviewContextCell")]
    partial class PreviewContextCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton DeletionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.BorderButton ReplicationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DeletionButton != null) {
                DeletionButton.Dispose ();
                DeletionButton = null;
            }

            if (ReplicationButton != null) {
                ReplicationButton.Dispose ();
                ReplicationButton = null;
            }
        }
    }
}