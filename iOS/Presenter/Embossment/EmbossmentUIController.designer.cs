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

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    [Register ("EmbossmentUIController")]
    partial class EmbossmentUIController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl ForceSegment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl QualitySegment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl TextSizeSegment { get; set; }

        [Action ("ForceChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ForceChanged (UIKit.UISegmentedControl sender);

        [Action ("QualityChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void QualityChanged (UIKit.UISegmentedControl sender);

        [Action ("TextSizeChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TextSizeChanged (UIKit.UISegmentedControl sender);

        void ReleaseDesignerOutlets ()
        {
            if (ForceSegment != null) {
                ForceSegment.Dispose ();
                ForceSegment = null;
            }

            if (QualitySegment != null) {
                QualitySegment.Dispose ();
                QualitySegment = null;
            }

            if (TextSizeSegment != null) {
                TextSizeSegment.Dispose ();
                TextSizeSegment = null;
            }
        }
    }
}