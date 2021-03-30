using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.Settings;
using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.iOS
{
    public class AppSetting
    {
        CommunicationClient TheClient = CommunicationClient.Instance;
        CombinedMachineModelNoRepository MachineModelRepo = new CombinedMachineModelNoRepository();

        public string ApplicationVersion {
            get {
                var versionName = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString");
                var versionCode = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion");
                return String.Format("{0}({1})", versionName, versionCode);
            }
        }

        public AppSetting()
        {
        }

        public async Task<string> ReadRomVersion() {
            var maybeV = (await TheClient.RetrieveRomVersion());
            if (maybeV == null)
                return "-.-.- Rev.-";
            RomVersion v = (RomVersion)maybeV;
            return String.Format("{0}.{1:00}.{2:00} Rev.{3}", v.Major, v.Middle, v.Minor, v.Revision);
        }

        /// <summary>
        /// プリファレンスに設定を保存します。
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public bool HoldOnPreference(PatmarkMachineModel spec)
        {
            return MachineModelRepo.HoldOnPreference(spec);
        }

        /// <summary>
        /// コントローラ側のモデルが、引数に指定したモデルと一致する場合に <c>true</c> を返します。
        /// </summary>
        /// <param name="model">期待するモデル番号。<c>null</c> を指定した場合は プリファレンスから読み取った値を使用します。 (初期値 <c>null</c>)</param>
        /// <returns>
        /// 通信に成功し、モデルが一致する場合は CommunicationResult.Success(true)
        /// 通信に成功し、モデルが一致しない場合は CommunicationResult.Success(false)
        /// 通信に失敗した場合は CommunicationResult.Failure()
        /// </returns>
        public async Task<IUnstable<CommunicationResult<bool>>> IsControllerModelMatchedWith(PatmarkMachineModel spec = null)
        {
            return await MachineModelRepo.IsControllerModelMatchedWith(spec);
        }


        /// <summary>
        /// プリファレンスから設定を読み込みます.
        /// </summary>
        /// <returns>The machine model.</returns> 
        public PatmarkMachineModel CurrentMachineModelOnPreference()
        {
            PatmarkMachineModel current;
            try
            {
                current = MachineModelRepo.CurrentOnPreference();
            }
            catch (TaskCanceledException)
            {
                // 失敗したらデフォルト値を取得.
                current = PatmarkMachineModel.Default;
            }
            return current;
        }
    }
}
