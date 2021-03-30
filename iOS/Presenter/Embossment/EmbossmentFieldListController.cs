using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using TokyoChokoku.Patmark.iOS.Presenter.Component;
using static UIKit.UIViewAutoresizing;


namespace TokyoChokoku.Patmark.iOS
{
    public partial class EmbossmentFieldListController : UIViewController
    {
        public event EventHandler SendRequested;
        FieldListController FiedlsController;

        public EmbossmentFieldListController() : base("EmbossmentFieldListController", null)
        {
            InitThisController();
        }

        public EmbossmentFieldListController(IntPtr handle) : base(handle)
        {
            InitThisController();
        }

        void InitThisController()
        {
            FiedlsController = new FieldListController();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (FiedlsController != null)
            {
                SetupChildController(FiedlsController, FieldListContainer);
            }
        }

        public void SetFields(IEnumerable<FieldReader> fields)
        {
            if (FiedlsController != null)
            {
                FiedlsController.SetFields(fields);
            }
        }

        private void SetupChildController(UIViewController subCnt, UIView parent)
        {
            var subView = subCnt.View;

            AddChildViewController(subCnt);

            var b = parent.Bounds;
            subView.Frame = b;
            subView.AutoresizingMask = All;
            parent.AddSubview(subView);
            subCnt.DidMoveToParentViewController(this);
        }

        partial void AcceptSend(SendButton sender)
        {
            SendRequested(this, null);
        }
    }
}