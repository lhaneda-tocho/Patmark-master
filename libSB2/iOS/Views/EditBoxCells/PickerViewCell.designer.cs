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
    [Register ("PickerViewCell")]
    partial class PickerViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView Picker { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Picker != null) {
                Picker.Dispose ();
                Picker = null;
            }
        }
    }
}