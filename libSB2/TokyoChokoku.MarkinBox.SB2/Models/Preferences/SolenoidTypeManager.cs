using System;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using Communication;

    public static class SolenoidTypeManager
    {
        static readonly string key = DataStoreKey.SolenoidType.ToKey ();
        static volatile SolenoidType    solenoidType = SolenoidType.Standard;

        /// <summary>
        /// 現在設定されているソレノイドの種類を返します．
        /// </summary>
        /// <value>The type of the solenoid.</value>
        public static SolenoidType SolenoidType {
            get { return solenoidType; }
            set {
                solenoidType = value;
                SubmitToApp ();
            }
        }

        /// <summary>
        /// BSD設定が有効であるか調べます．
        /// </summary>
        /// <value><c>true</c> if BSD Enabled; otherwise, <c>false</c>.</value>
        public static bool BSDEnabled {
            get {
                return solenoidType == SolenoidType.BigSolenoid;
            }
        }

        /// <summary>
        /// アプリの環境設定に書き込みます．
        /// </summary>
        static void SubmitToApp ()
        {
            var accessor = PreferenceManager.Instance.Accessor;
            accessor.SetString (SolenoidType.ToString (), key);
            Log.Debug ("設定を更新しました ... " + key + " : " + SolenoidType);
        }

        /// <summary>
        /// コントローラに書き込みます．
        /// </summary>
        public static async Task TrySendToController ()
        {
            var state = CommunicationClientManager.Instance.GetCurrentState ();
            await state.ProcessCommunication (UnsafeSendToController);
        }

        static async Task UnsafeSendToController ()
        {
            var enabled = BSDEnabled;
            if (!(await CommandExecuter.SetBSDEnabled (BSDEnabled)).IsOk) {
                System.Diagnostics.Debug.WriteLine ("Failure to write BSD mode.");
                return;
            }

            // await CommandExecuter.SaveBasiceSettingsToSdCard ();

            var response = await CommandExecuter.ReadBSDEnabled ();

            if (response.IsOk) {
                var type = response.Value != 0 ? SolenoidType.BigSolenoid : SolenoidType.Standard;
                System.Diagnostics.Debug.WriteLine ("BSD 書き込み後 -> " + type);
            } else {
                System.Diagnostics.Debug.WriteLine ("BSDFlagの読み込みに失敗しました．");
            }
        }
    }
}
