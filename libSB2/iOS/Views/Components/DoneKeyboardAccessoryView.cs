using Foundation;
using System.ComponentModel;
using System;
using ObjCRuntime;
using CoreGraphics;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    [DesignTimeVisible (true)]
    public partial class DoneKeyboardAccessoryView : UIView
    {
        public static string NibKey = "DoneKeyboardAccessoryView";

        [Outlet]
        UIView ContentView { get; set; }

        public event Action PressDoneButton;

        public DoneKeyboardAccessoryView ()
        {
        }

        public DoneKeyboardAccessoryView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();
            var bundle = NSBundle.FromClass (Class);
            var nib = UINib.FromName (NibKey, bundle);
            var view = nib.Instantiate (this, null) [0] as UIView;

            Bounds = new CGRect (0, 0, Bounds.Width, 42);

            ContentView = view;
            ContentView.Frame = Bounds;
            AddSubview (view);
        }

        partial void DoneButton_Activated (UIBarButtonItem sender)
        {
            if (PressDoneButton != null)
                PressDoneButton ();
        }

        public void PurgeListener ()
        {
            PressDoneButton = null;
        }
    }
}