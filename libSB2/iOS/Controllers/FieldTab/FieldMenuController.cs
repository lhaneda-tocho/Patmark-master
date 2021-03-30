using System;
using UIKit;
using Foundation;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class FieldMenuController : UIViewController
	{
		UITapGestureRecognizer   TapRecognizer { get; set; } = null;
        public iOSFieldManager   FieldManager { get; set; }
        public iOSEditBoxManager EditBoxManager { get; set; }
		

		public FieldMenuController (IntPtr handle) : base (handle)
		{
		}


		/// <summary>
		/// Views the did load.
		/// </summary>
		override
		public void ViewDidLoad() {
			base.ViewDidLoad ();

            if (FieldManager == null)
                throw new NullReferenceException ();
            if (EditBoxManager == null)
                throw new NullReferenceException ();            

            SetupButtonsEnabled();
            ToDefaultMode ();
		}

        // ---- 初期化メソッド ----
        void SetupButtonsEnabled()
        {
            AddButton.Enabled = OperationModeManager.IsAdministrator();
            RemoveButton.Enabled = OperationModeManager.IsAdministrator();
        }

        // ---- 閉じる ---
        partial void CloseButton_TouchUpInside (UIButton sender)
        {
            PerformSegue ("exit", this);
        }

        // ---- 基本 3要素 ----
        partial void FieldListButton_TouchUpInside (ImageButton sender)
        {
            ToDefaultMode ();
            PerformSegue ("ShowFieldList", this);
        }

        partial void AddButton_TouchUpInside (ImageButton sender)
        {
            ToggleAddListMode ();
        }

        partial void RemoveButton_TouchUpInside (ImageButton sender)
        {
            ToDefaultMode ();
            FieldManager.CheckThenDeleteAll ();
        }



        // ----  フィールド追加系 ボタン ----
        partial void AddTextItem_TouchUpInside (ImageButton sender)
        {
            ToAddMode (HorizontalText.Constant.Create ());
        }

        partial void AddQrCode_TouchUpInside (ImageButton sender)
        {
            ToAddMode (QrCode.Constant.Create ());
        }

        partial void AddDataMatrix_TouchUpInside (ImageButton sender)
        {
            ToAddMode (DataMatrix.Constant.Create ());
        }

        // ---- フィールド追加キャンセルボタン ----
        partial void CancelButton_TouchUpInside (UIButton sender)
        {
            ToggleAddListMode ();
        }


        // ---- 状態遷移メソッド ----
		void ToDefaultMode () {
            // 表示切替
            ShowBaseMenu ();
            HideAddMenu ();
            HideAddCancelButton ();

            // デリゲート設定
            RemoveTapGesture ();
		}

        void ToggleAddListMode ()
        {
            ShowBaseMenu ();
            ToggleAddMenu ();
            HideAddCancelButton ();

            // デリゲート設定
            RemoveTapGesture ();
        }

        void ToAddMode (IField<IConstantParameter> request)
        {
            // 表示切替
            HideBaseMenu ();
            HideAddMenu ();
            ShowAddCancelButton ();

            RemoveTapGesture ();

            // デリゲートの設定
            var maybeRec = FieldManager.TapToAddField ((position) => {

                var field = request.ToGenericMutable ();
                var param = field.Parameter;
                param.X = (decimal)position.X;
                param.Y = (decimal)position.Y;
                return new iOSOwner (field.ToGenericConstant ());

            }, (ev) => {

                EditBoxManager.ListenAddFieldEvent (ev);
                ToDefaultMode ();
                PerformSegue ("exit", this);

            });

            if (maybeRec.HasValue)
                SetTapGesture (maybeRec.Value);
            else
                throw new NullReferenceException ();
        }


        // ---- 表示切替メソッド ----
        void ShowBaseMenu ()
        {
            CloseButton.Hidden = false;
            AddButton.Hidden = false;
            RemoveButton.Hidden = false;
            FieldListButton.Hidden = false;
        }

        void HideBaseMenu ()
        {
            CloseButton.Hidden = true;
            AddButton.Hidden = true;
            RemoveButton.Hidden = true;
            FieldListButton.Hidden = true;
        }

        //

        void ShowAddMenu ()
        {
            FieldFolder.Hidden = false;
        }

        void HideAddMenu ()
        {
            FieldFolder.Hidden = true;
        }

        void ToggleAddMenu ()
        {
            FieldFolder.Hidden ^= true;
        }

        //

        void ShowAddCancelButton ()
        {
            CancelButton.Hidden = false;
        }

        void HideAddCancelButton ()
        {
            CancelButton.Hidden = true;
        }

        // ジェスチャの追加・削除
        void SetTapGesture (UITapGestureRecognizer rec)
        {
            RemoveTapGesture ();
            RootFieldView.AddGestureRecognizer (rec);
            TapRecognizer = rec;
        }

        void RemoveTapGesture ()
        {
            if (TapRecognizer != null)
                RootFieldView.RemoveGestureRecognizer (TapRecognizer);
        }

        // ---- セグエイベントハンドリング ----
        override
        public void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("ShowFieldList"))
            {
                var ctrl = (UINavigationController)segue.DestinationViewController;
                var top = (FieldListViewController)ctrl.TopViewController;
                top.FieldManager = FieldManager;
            }
        }

	}
}
