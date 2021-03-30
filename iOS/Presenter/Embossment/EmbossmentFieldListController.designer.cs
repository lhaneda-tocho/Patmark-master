// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TokyoChokoku.Patmark.iOS
{
    [Register ("EmbossmentFieldListController")]
    partial class EmbossmentFieldListController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FieldListContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.Patmark.iOS.Presenter.Component.SendButton SendButton { get; set; }

        [Action ("AcceptSend:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AcceptSend (TokyoChokoku.Patmark.iOS.Presenter.Component.SendButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (FieldListContainer != null) {
                FieldListContainer.Dispose ();
                FieldListContainer = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }
        }
    }
}