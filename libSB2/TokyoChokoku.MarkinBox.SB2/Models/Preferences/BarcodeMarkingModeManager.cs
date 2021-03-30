using System;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook.iOS;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public static class BarcodeMarkingModeManager
    {
        static
        string             key = DataStoreKey.BarcodeMarkingMode.ToKey ();

        static
        BarcodeMarkingMode mode = BarcodeMarkingMode.Marking2Way;

        public static
        BarcodeMarkingMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
                PreferenceManager.Instance.Accessor.SetString (mode.ToString (), key);
                Log.Debug ("設定を更新しました ... " + key + " : " + mode);
            }
        }


        // uitilities
        public static
        bool Is1Way ()
        {
            return mode == BarcodeMarkingMode.Marking1Way;
        }


        public static
        bool Is2Way ()
        {
            return mode == BarcodeMarkingMode.Marking2Way;
        }

        public static
        async Task TrySendToController ()
        {
            var state = CommunicationClientManager.Instance.GetCurrentState ();
            await state.ProcessCommunication ( UnsafeSendToController );
        }

        static async Task UnsafeSendToController ()
        {
            await CommandExecuter.SetBarcode1WayMarkingMode (Is1Way ());
            await CommandExecuter.SaveBasiceSettingsToSdCard ()        ;
        }
    }
}

