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

namespace TokyoChokoku.Patmark.iOS.Presenter.FieldPreview
{
    [Register ("FieldPreviewController")]
    partial class FieldPreviewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomMargin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CurrentPageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint LeadingMargin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PageCountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.iOS.Custom.FilledButton PrevButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.FieldPreview.FieldPreView Preview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.Ruler.RulerView Ruler { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.Component.SendButton SendButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TopMargin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TrailingMargin { get; set; }

        [Action ("AcceptSend:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AcceptSend (TokyoChokoku.Patmark.iOS.Presenter.Component.SendButton sender);

        [Action ("PageBack:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PageBack (TokyoChokoku.iOS.Custom.FilledButton sender);

        [Action ("PageForward:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PageForward (TokyoChokoku.iOS.Custom.FilledButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BottomMargin != null) {
                BottomMargin.Dispose ();
                BottomMargin = null;
            }

            if (CurrentPageLabel != null) {
                CurrentPageLabel.Dispose ();
                CurrentPageLabel = null;
            }

            if (LeadingMargin != null) {
                LeadingMargin.Dispose ();
                LeadingMargin = null;
            }

            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (PageCountLabel != null) {
                PageCountLabel.Dispose ();
                PageCountLabel = null;
            }

            if (PrevButton != null) {
                PrevButton.Dispose ();
                PrevButton = null;
            }

            if (Preview != null) {
                Preview.Dispose ();
                Preview = null;
            }

            if (Ruler != null) {
                Ruler.Dispose ();
                Ruler = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (TopMargin != null) {
                TopMargin.Dispose ();
                TopMargin = null;
            }

            if (TrailingMargin != null) {
                TrailingMargin.Dispose ();
                TrailingMargin = null;
            }
        }
    }
}