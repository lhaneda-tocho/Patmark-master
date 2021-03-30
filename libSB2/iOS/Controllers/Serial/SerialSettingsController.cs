using System;
using Foundation;
using UIKit;

using ToastIOS;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    partial class SerialSettingsController : UINavigationController
    {
        
        public SerialSettingsController (IntPtr handle) : base (handle)
        {
        }


        override
        public void ViewDidLoad()
        {
            // ナビゲーションのタイトル、戻るボタンをセットアップします。
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-serial-menu.title", "");
            ControllerUtils.SetupNavigationBackButton(NavigationItem);
        }


        public static SerialSettingsEditorController.ViewModel
        GenViewModel (
                iOSFieldManager fieldManager, 
                string         text,
                Action<string> insertAfter,
                Action<string> replaceSerial)
        {
            var model = new SerialSettingsEditorController.ViewModel ();

            // テキストから シリアル番号を読み取る
            var replaceTarget = MatchingGrammer.SearchSerialNo(text);

            if (replaceTarget != null) {
                var no = (int)replaceTarget;
                model.SerialNo  = no;
                model.Stores    = SerialSettingsManager.Instance.CreateSerialStores () [no-1];
                model.OnInsert += replaceSerial;
                return model;
            }

            // 他のフィールドから 使われていない シリアル番号を 探す．
            var maybeNo = fieldManager.UnusedSerialNo.Invoke();

            if (maybeNo.HasValue) {
                var no = maybeNo.Value;
                model.SerialNo  = no;
                model.Stores    = SerialSettingsManager.Instance.CreateSerialStores () [no-1];
                model.OnInsert += insertAfter;
                return model;
            }


            // 利用可能なシリアルNoがなければ null を返す
            return null;

            // 書き込み

        }


        public static void Modal (
            iOSFieldManager fieldManager,
            string         text,
            Action<string> insertAfter,
            Action<string> replaceSerial)
        {
            var topCtrl = ControllerUtils.FindTopViewController();
            var storyboard = UIStoryboard.FromName("SerialStoryboard", null);
            var ctrl = storyboard.InstantiateViewController("SerialSettingsController") as SerialSettingsController;

            // ViewModel 作成
            var model = GenViewModel (
                fieldManager,
                text,
                insertAfter,
                replaceSerial);

            // ViewModel を生成できない場合．
            // つまり，もうシリアルを追加できない場合
            if (model == null) {
                Toast
                    .MakeText ("You can't add serial anymore.")
                    .SetDuration (1500)
                    .Show ();
                return;
            }

            // 表示準備
            ((SerialSettingsEditorController)ctrl.TopViewController).Model = model;
            ctrl.Modal(topCtrl);
        }


        void Modal (UIViewController parent)
        {
            ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
            parent.PresentViewController(this, true, () =>
            {

            });
        }
    }
}