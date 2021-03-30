using System;
using System.Threading;
using System.Threading.Tasks;


using UIKit;
using Foundation;

using ToastIOS;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class MarkingMenuController : UIViewController
	{
        public iOSFieldManager FieldManager { get; set; } = null;


		enum Mode
		{
			Initial,
			Marking,
			Pausing,
			Operating
		}

        CommunicationNotifier CommunicationNotifier;

        private object lockObject = new object();

		public MarkingMenuController (IntPtr handle) : base (handle)
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

			UIViewGonablizer.Init ();

			CloseButton.TouchUpInside += PushCloseButton;

			MoveHeadToOriginButton.TouchUpInside += PushMoveHeadToOriginButton;

			MarkingButton.TouchUpInside += PushMarkingButton;
			TestMarkingButton.TouchUpInside += PushTestMarkingButton;
			PermanentMarkingButton.TouchUpInside += PushPermanentMarkingButton;

			PauseMarkingButton.TouchUpInside += PushPauseMarkingButton;
			RestartMarkingButton.TouchUpInside += PushRestartMarkingButton;
			StopMarkingButton.TouchUpInside += PushStopMarkingButton;

			ChangeMode (Mode.Initial);

            //
            CommunicationNotifier = new CommunicationNotifier(1000);
            CommunicationNotifier.OnConnectionStatusChanged += (ConnectionState state, ConnectionState preState) =>
            {
                InvokeOnMainThread(() =>
                {
                    SetButtonsEnableWithConnection (state.Ready());
                });
            };
            CommunicationNotifier.Start();

            SetButtonsEnableWithConnection(CommunicationClientManager.Instance.GetCurrentState().Ready());
		}

		async void PushMoveHeadToOriginButton (object sender, EventArgs arg) {
			ChangeMode (Mode.Operating);
			await CommandExecuter.MoveMarkingHeadToOrigin ();
			ChangeMode (Mode.Initial);
		}

		async void PushMarkingButton (object sender, EventArgs arg) {
            await ExecuteMarking(false);
		}

		async void PushTestMarkingButton (object sender, EventArgs arg) {
            await ExecuteMarking(true);
		}

        async Task<Nil> ExecuteMarking(bool isTestMode)
        {
            var serializableList = FieldManager.SerializableList;
            if (serializableList.Count == 0){
                Toast.MakeText(NSBundle.MainBundle.LocalizedString("Marking file is empty.", "")).Show();
            }
            else{
                ChangeMode(Mode.Marking);
                bool res = false;
                await ControllerUtils.ActionWithLoadingOverlay(async () =>
                {
                    res = (
                        await CommandExecuter.SetUpMarking(serializableList, isTestMode)
                        && await SerialSettingsManager.Instance.Save(null)
                    );
                });
                if (res)
                {
                    if (await CommandExecuter.StartMarking(PauseCallback))
                    {
                        await ControllerUtils.ActionWithLoadingOverlay(async () =>
                        {
                            // シリアル設定を更新
                            await SerialSettingsManager.Instance.Reload(null);
                        });
                        
                        Toast.MakeText(
                            NSBundle.MainBundle.LocalizedString("Completed marking successfully.", ""),
                            ToastDuration.Medium
                        ).Show();
                    }
                    else {
                        Toast.MakeText(
                            NSBundle.MainBundle.LocalizedString("Terminated marking abnormally.", ""),
                            ToastDuration.Medium
                        ).Show();
                    }
                }
                else {
                    Toast.MakeText(
                        NSBundle.MainBundle.LocalizedString("Failed to send marking file.", ""),
                        ToastDuration.Medium
                    ).Show();
                }
                Finish();
            }
            return null;
        }

        // パーマネント打刻ボタンの挙動を定義します。
		async void PushPermanentMarkingButton (object sender, EventArgs arg) {
            var serializableList = FieldManager.SerializableList;
            if (serializableList.Count == 0){
                Toast.MakeText(
                    NSBundle.MainBundle.LocalizedString("Marking file is empty.", ""),
                    ToastDuration.Medium
                ).Show();
            }
            else {
                ChangeMode(Mode.Operating);
                bool res = false;
                await ControllerUtils.ActionWithLoadingOverlay(async () =>
                {
                    res = await CommandExecuter.SetupPermanentMarking(serializableList);
                });
                if (res)
                {
                    Toast.MakeText(
                        NSBundle.MainBundle.LocalizedString("Push the start button on the marking head.", ""),
                        ToastDuration.Medium
                    ).Show();
                }
                else {
                    Toast.MakeText(
                        NSBundle.MainBundle.LocalizedString("Failed to send marking file.", ""),
                        ToastDuration.Medium
                    ).Show();
                }
            }
			Finish ();
		}

		async void PushPauseMarkingButton (object sender, EventArgs arg) {
            if (Monitor.TryEnter (lockObject)) {
                try {
                    ChangeMode (Mode.Operating);
                    await CommandExecuter.SetMarkingPausingStatus (Communication.MBMemories.MarkingPausingStatus.Pause);
                    ChangeMode (Mode.Pausing);
                } finally {
                    Monitor.Exit (lockObject);
                }
            } else {
                PushPauseMarkingButton (sender, arg);
            }
		}

		private async void PushRestartMarkingButton (object sender, EventArgs arg) {
            if (Monitor.TryEnter (lockObject)) {
                try {
                    ChangeMode (Mode.Operating);
                    await CommandExecuter.SetMarkingPausingStatus (Communication.MBMemories.MarkingPausingStatus.Resume);
                    ChangeMode (Mode.Marking);
                } finally {
                    Monitor.Exit (lockObject);
                }
            } else {
                PushRestartMarkingButton (sender, arg);
            }
		}

		private async void PushStopMarkingButton (object sender, EventArgs arg) {
            if (Monitor.TryEnter (lockObject)) {
                try {
                    ChangeMode (Mode.Operating);
                    await CommandExecuter.SetMarkingPausingStatus (Communication.MBMemories.MarkingPausingStatus.Stop);
                    await CommandExecuter.MoveMarkingHeadToOrigin ();
                    Finish ();
                } finally {
                    Monitor.Exit (lockObject);
                }
            } else {
                PushStopMarkingButton (sender, arg);
            }
		}

		private void PushCloseButton (object sender, EventArgs arg) {
			Finish ();
		}

		private void Finish(){
			ChangeMode (Mode.Initial);
            PerformSegue ("exit", this);
		}

        private void PauseCallback (bool pause)
        {
            if (Monitor.TryEnter (lockObject)) {
                try {
                    if (pause)
                        ChangeMode (Mode.Pausing);
                    else
                        ChangeMode (Mode.Marking);
                } finally {
                    Monitor.Exit (lockObject);
                }
            }
        }
            

		private void ChangeMode(Mode mode){
			switch (mode) {
			case Mode.Initial:
				{
					CloseButton.Hidden = false;

					MoveHeadToOriginButton.Gone(false);
					MarkingButton.Gone(false);
					TestMarkingButton.Gone(false);
					PermanentMarkingButton.Gone(false);
					PauseMarkingButton.Gone(true);
					RestartMarkingButton.Gone(true);
					StopMarkingButton.Gone(true);
				}
				break;
			case Mode.Marking:
				{
					CloseButton.Hidden = true;

					MoveHeadToOriginButton.Gone(true);
					MarkingButton.Gone(true);
					TestMarkingButton.Gone(true);
					PermanentMarkingButton.Gone(true);
					PauseMarkingButton.Gone(false);
					RestartMarkingButton.Gone(true);
					StopMarkingButton.Gone(false);
				}
				break;
			case Mode.Pausing:
				{
					CloseButton.Hidden = true;

					MoveHeadToOriginButton.Gone(true);
					MarkingButton.Gone(true);
					TestMarkingButton.Gone(true);
					PermanentMarkingButton.Gone(true);
					PauseMarkingButton.Gone(true);
					RestartMarkingButton.Gone(false);
					StopMarkingButton.Gone(false);
				}
				break;
			case Mode.Operating:
				{
					CloseButton.Hidden = true;

					MoveHeadToOriginButton.Gone(true);
					MarkingButton.Gone(true);
					TestMarkingButton.Gone(true);
					PermanentMarkingButton.Gone(true);
					PauseMarkingButton.Gone(true);
					RestartMarkingButton.Gone(true);
					StopMarkingButton.Gone(true);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("MarkingMenuController.ChangeMode - ケース設定に漏れがあります。");
			}
		}

        void SetButtonsEnableWithConnection(bool status)
        {
            MoveHeadToOriginButton.Enabled = status;
            MarkingButton.Enabled = status;
            TestMarkingButton.Enabled = status;
            PermanentMarkingButton.Enabled = status;
            PauseMarkingButton.Enabled = status;
            RestartMarkingButton.Enabled = status;
            StopMarkingButton.Enabled = status;
        }
	}
}
