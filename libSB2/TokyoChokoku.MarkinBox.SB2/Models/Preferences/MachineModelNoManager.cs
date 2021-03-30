using System;
using System.Threading.Tasks;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public static class MachineModelNoManager
	{
		private static readonly string Key;
		private static  MBMemories.MachineModel Value;

		public static MBMemories.MachineModel Get(){
			return Value;
		}

		public static void Set(MBMemories.MachineModel localNumber){
			// アプリの設定を更新します。
            PreferenceManager.Instance.Accessor.SetString(localNumber.ToString(), Key);
			Log.Debug("設定を更新しました ... " +  Key + " : " + localNumber.ToString());

            // FIXME:端末依存の処理です。ユーザに問い合わせて、OKなら上書きします。
//			if (CommunicationManager.IsConnectable ()) {
//				caller.InvokeOnMainThread ( async () => {
//					var remoteNumber = (await new ReadingMachineModelNo ().Execute ()).Value;
//					if (remoteNumber != (short)localNumber) {
//						if (
//							MessageBox.Show (
//								"コントローラの設定内容を上書きしますか？",
//								"",
//								MessageBoxButtons.OKCancel
//							) == MessageBoxResult.OK) {
//							await new SettingMachineModelNo ((short)localNumber).Execute ();
//						}
//					}
//				});
//			}

            //
            Value = localNumber;
		}

		static MachineModelNoManager(){
			//
			Key = DataStoreKey.MachineModelNumber.ToKey ();
			// 初期値を設定します。
			MBMemories.MachineModel tmpValue;
			if (Enum.TryParse (PreferenceManager.Instance.Accessor.GetString (Key), out tmpValue)) {
				Value = tmpValue;
			} else {
				Value = MBMemories.MachineModel.No3315;
			}
		}

        public static async Task TrySendToController ()
        {
            // コントローラに接続している時のみ
            var state = CommunicationClientManager.Instance.GetCurrentState ();
            await state.ProcessCommunication ( UnsafeSendToController );
        }

        static async Task UnsafeSendToController ()
        {
            await CommandExecuter.SetMachineModelNo ((short)Value);
        }
	}
}

