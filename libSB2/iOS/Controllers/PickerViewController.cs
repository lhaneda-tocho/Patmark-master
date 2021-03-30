using System;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    partial class PickerViewController : UIViewController 
	{
        public  PickerViewControllerDelegate Delegate { get; private set; } = null;


		public PickerViewController (IntPtr handle) : base (handle)
		{
		}



        public static void Popover (PickerViewControllerDelegate model, CGSize size, UIView source, Action<UIPickerView> afterInit) {
            var c = GetTopController ();
            var picker = (PickerViewController) c.Storyboard.InstantiateViewController ("PickerViewController");

            picker.Delegate = model;
            picker.Popover (c, size, source, afterInit);
        }


        private static UIViewController GetTopController () {
            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while ((topController.PresentedViewController) != null) {
                topController = topController.PresentedViewController;
            }

            return topController;
        }


        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            DoneButton.Clicked += PushDoneButton;
            Picker.Model = (UIPickerViewModel) Delegate ?? new EmptyModel ();
            Delegate.Initialize (Picker);
        }




        public void Popover (UIViewController parent, CGSize size, UIView source, Action<UIPickerView> afterInit) {
            ModalPresentationStyle = UIModalPresentationStyle.Popover;
            PreferredContentSize = size;
            ContentSizeForViewInPopover = size;

            var presentationController = PopoverPresentationController;


            if (presentationController != null) {
                presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Down;
                presentationController.SourceView = source;
                presentationController.SourceRect = source.Bounds;
                presentationController.Delegate = new MyDelegate (this);
            }


            parent.PresentViewController(this, true, () => {
                if (afterInit != null)
                    afterInit (Picker);
            });
        }


        private void StopPicker ()
        {
            nint componentCount = Picker.NumberOfComponents;
            for (int i = 0; i < componentCount; i++) {
                nint row = Picker.SelectedRowInComponent (i);
                Picker.Select (row, i, false);
            }
        }


        private void PushDoneButton (object sender, EventArgs e) {
            StopPicker ();
            Delegate.Commit (Picker);

            DismissViewController (true, () => {

            });
        }



        private sealed class MyDelegate : UIPopoverPresentationControllerDelegate {
            private readonly PickerViewController controller;

            public MyDelegate (PickerViewController controller)
            {
                this.controller = controller;
            }

            // iphoneで モーダルビューになる設定を 無効にする．
            override
            public UIModalPresentationStyle GetAdaptivePresentationStyle (UIPresentationController forPresentationController) {
                return UIModalPresentationStyle.None;
            }

            // 吹き出しの外側をタッチされたために 吹き出しを閉じる必要が出た時．
            public override bool ShouldDismissPopover (UIPopoverPresentationController popoverPresentationController)
            {
                controller.StopPicker ();
                controller.Delegate.Commit (controller.Picker);
                return true;
            }
        }


        private sealed class EmptyModel : UIPickerViewModel {
            // カラム数
            public override nint GetComponentCount(UIPickerView pickerView) {
                return 1;
            }


            // 行数
            public override nint GetRowsInComponent(UIPickerView pickerView, nint component) {
                return 1;
            }


            // 文字列取得
            public override string GetTitle(UIPickerView pickerView, nint row, nint component) {
                return "--EMPTY--";
            }


            // 高さの指定
            public override nfloat GetRowHeight(UIPickerView pickerView, nint component) {
                return 20f;
            }


            //幅の指定
            public override nfloat GetComponentWidth(UIPickerView pickerView, nint component){
                return 200f;
            }


            //選択が変化した際のイベント
            public override void Selected(UIPickerView pickerView, nint row, nint component){
                
            }

        }

	}
}
