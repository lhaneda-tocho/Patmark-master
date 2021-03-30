using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox;

using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.SerialModule.Setting
{
    public static class SerialSettingIO
    {
        /// <summary>
        /// Sends the specified setting to controller.
        /// </summary>
        /// <returns>The to controller.</returns>
        /// <param name="setting">Setting.</param>
        /// <param name="fileNumber">File number.</param>
        public static async Task<bool> SendToController(SerialSetting setting, int? fileNumber)
        {
            try
            {
                var client = CommunicationClient.Instance;
                if (!client.Ready)
                {
                    Console.Error.WriteLine("[SerialSettingIO - SendToController]The client not ready yet.");
                    return false;
                }
                var exec = client.CreateCommandExecutor();

                var sslist = setting.SettingList.ToMBForm();
                var sclist = setting.CountStateList;

                if (!(await exec.SetSerialSettings( sslist )).IsOk)
                {
                    Console.Error.WriteLine("[SerialSettingIO - SendToController]失敗 - SetSerialSettings");
                    return false;
                }
                if (!(await exec.SetSerialCounters( sclist )).IsOk)
                {
                    Console.Error.WriteLine("[SerialSettingIO - SendToController]失敗 - SetSerialCounters");
                    return false;
                }

                if (fileNumber != null)
                {
                    // ファイルに対応した設定値をコントローラからSDカードへ保存

                    if (!(await exec.SetSerialSettingsFileNo((short)fileNumber)).IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingsManager - Save]失敗 - SetSerialSettingsFileNo");
                        return false;
                    }

                    if (!(await exec.SaveSerialSettingsOfFileToSdCard()).IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingsManager - Save]失敗 - SaveSerialSettingsOfFileToSdCard");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
            
        }

        /// <summary>
        /// Retreive SerialSetting from controller.
        /// return null if cannot communicate with the controller.
        /// </summary>
        /// <returns>Mutable SerialSetting instance.</returns>
        public static async Task<SerialSetting> RetrieveFromController(int? fileNumber)
        {
            try
            {
                var client = CommunicationClient.Instance;
                if (!client.Ready)
                {
                    Console.Error.WriteLine("[SerialSettingIO - RetrieveFromController]The client not ready yet.");
                    return null;
                }
                var exec = client.CreateCommandExecutor();

                if (fileNumber != null)
                {
                    // ファイルに対応した設定値をSDカードからコントローラへロード
                    if (!(await exec.SetSerialSettingsFileNo((short)fileNumber)).IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingIO - RetreiveFromController]Failure to SetSerialSettingsFileNo");
                        return null;
                    }

                    if (!(await exec.LoadSerialSettingsOfFileFromSdCard()).IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingIO - RetreiveFromController]Failure to LoadSerialSettingsOfFileFromSdCard");
                        return null;
                    }
                }

                {
                    var resSettings = await exec.ReadSerialSettings();
                    if (!resSettings.IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingIO - RetreiveFromController]Failure to ReadSerialSettings");
                        return null;
                    }
                    var resCounters = await exec.ReadSerialCounters();
                    if (!resCounters.IsOk)
                    {
                        Console.Error.WriteLine("[SerialSettingIO - RetreiveFromController]Failure to ReadSerialCounters");
                        return null;
                    }

                    var sslist = new SCSettingList.Mutable(
                        from it in resSettings.Value
                        select SCSetting.From(it)
                    );
                    var sclist = SCCountStateList.CreateFrom(resCounters.Value);

                    return new SerialSetting.Mutable(sslist, sclist);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return null;
            }
        }
    }
}