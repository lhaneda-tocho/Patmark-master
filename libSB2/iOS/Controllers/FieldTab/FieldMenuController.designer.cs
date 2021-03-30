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
    [Register ("FieldMenuController")]
    partial class FieldMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton AddButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton AddDataMatrix { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton AddQrCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton AddTextItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.UISpringView FieldFolder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton FieldListButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton RemoveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        TokyoChokoku.MarkinBox.Sketchbook.iOS.RootFieldView RootFieldView { get; set; }

        [Action ("AddButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddButton_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        [Action ("AddDataMatrix_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddDataMatrix_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        [Action ("AddQrCode_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddQrCode_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        [Action ("AddTextItem_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddTextItem_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        [Action ("CancelButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("CloseButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("FieldListButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FieldListButton_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        [Action ("RemoveButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RemoveButton_TouchUpInside (TokyoChokoku.MarkinBox.Sketchbook.iOS.ImageButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddButton != null) {
                AddButton.Dispose ();
                AddButton = null;
            }

            if (AddDataMatrix != null) {
                AddDataMatrix.Dispose ();
                AddDataMatrix = null;
            }

            if (AddQrCode != null) {
                AddQrCode.Dispose ();
                AddQrCode = null;
            }

            if (AddTextItem != null) {
                AddTextItem.Dispose ();
                AddTextItem = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (FieldFolder != null) {
                FieldFolder.Dispose ();
                FieldFolder = null;
            }

            if (FieldListButton != null) {
                FieldListButton.Dispose ();
                FieldListButton = null;
            }

            if (RemoveButton != null) {
                RemoveButton.Dispose ();
                RemoveButton = null;
            }

            if (RootFieldView != null) {
                RootFieldView.Dispose ();
                RootFieldView = null;
            }
        }
    }
}