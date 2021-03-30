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
    [Register ("MarkingMenuController")]
    partial class MarkingMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton MarkingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton MoveHeadToOriginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton PauseMarkingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton PermanentMarkingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton RestartMarkingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.RootFieldView RootFieldView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton StopMarkingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton TestMarkingButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (MarkingButton != null) {
                MarkingButton.Dispose ();
                MarkingButton = null;
            }

            if (MoveHeadToOriginButton != null) {
                MoveHeadToOriginButton.Dispose ();
                MoveHeadToOriginButton = null;
            }

            if (PauseMarkingButton != null) {
                PauseMarkingButton.Dispose ();
                PauseMarkingButton = null;
            }

            if (PermanentMarkingButton != null) {
                PermanentMarkingButton.Dispose ();
                PermanentMarkingButton = null;
            }

            if (RestartMarkingButton != null) {
                RestartMarkingButton.Dispose ();
                RestartMarkingButton = null;
            }

            if (RootFieldView != null) {
                RootFieldView.Dispose ();
                RootFieldView = null;
            }

            if (StopMarkingButton != null) {
                StopMarkingButton.Dispose ();
                StopMarkingButton = null;
            }

            if (TestMarkingButton != null) {
                TestMarkingButton.Dispose ();
                TestMarkingButton = null;
            }
        }
    }
}