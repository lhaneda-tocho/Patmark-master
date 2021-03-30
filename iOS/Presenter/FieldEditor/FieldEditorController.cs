using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin;
using TokyoChokoku.Patmark.StorageUtil;
using TokyoChokoku.iOS.Custom;
using TokyoChokoku.Patmark.iOS.Presenter.Component;
using TokyoChokoku.Patmark.iOS.Presenter.Embossment;
using Monad;

namespace TokyoChokoku.Patmark.iOS.Presenter.FieldEditor
{
    public partial class FieldEditorController : UIViewController
    {
        /// <summary>
        /// Nextボタンが押され
        /// 次の画面へ移動する際に呼び出されるセグエのID
        /// </summary>
        const String NextSegueID = "Next";


        FileContext CurrentFile;

		/// <summary>
		/// プログラム側で初期化
		/// </summary>
		public FieldEditorController() : base("FieldEditorController", null)
        {
        }

        /// <summary>
        /// StoryBoardから初期化
        /// </summary>
        /// <param name="handle">Handle.</param>
        public FieldEditorController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetNeedsStatusBarAppearanceUpdate();
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                TextField.SmartDashesType = UITextSmartDashesType.No;
                TextField.SmartQuotesType = UITextSmartQuotesType.No;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!RestoreFile())
            {
                CurrentFile = FileContext.Empty();
            }

            if (CurrentFile.Owner.IsEmpty)
            {
                // 何もしません。
            }
            else if (CurrentFile.isLocalFile)
            {
                // クイックモードの場合は、テキストを編集してもらいます。
                TextField.Text = CurrentFile.Owner.Fields[0].Text;
            }
            else
            {
                // アドバンスモードの場合は、即座に打刻画面へ遷移します。
                PerformSegue(NextSegueID, this);
                MyToast.ShowMessage("This file was created in advance mode and can not be edited.", MyToast.Short);
            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Nexts the button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void NextButton_TouchUpInside(FlashButton sender)
        {
            PerformSegue(NextSegueID, this);
        }

        /// <summary>
        /// セグエにより画面遷移する直前に呼び出される
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == NextSegueID)
                PrepareForNextSegue(segue, sender);
        }

        void PrepareForNextSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if(CurrentFile.isLocalFile)
            {
                // クイックモードの場合は、編集したテキストをセットします。
                CurrentFile = CurrentFile.QuickEditor.ApplyText(TextField.Text);
                // ファイルを保存
                SaveCurrentField();
            }

            var rec = segue.DestinationViewController as EmbossmentController;
            if (rec == null) return;
            rec.ReceiveFile(CurrentFile);
        }

        /// <summary>
        /// Saves the current field.
        /// </summary>
        void SaveCurrentField()
        {
            AutoSaveManager.Overwrite(CurrentFile.Owner);
        }

        /// <summary>
        /// Restores the field.
        /// </summary>
        bool RestoreFile()
        {
            var f = AutoSaveManager.LoadFileAutoSaved();
            if(f.HasValue()){
                CurrentFile = f.Value();
                return true;
            }
            return false;
        }
    }
}

