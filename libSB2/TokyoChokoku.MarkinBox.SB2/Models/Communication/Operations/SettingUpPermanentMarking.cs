using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
        /// <summary>
        /// Making with button of head.
        /// For MB2 controller.
        /// </summary>Communication
        public static async Task<bool> SetupPermanentMarking(List<MBData> fields){

            var fileIndex = Sizes.IndexOfRemotePermanentFile;
//            var fileIndex = 23;
            var fileNumber = fileIndex + 1;

            if (!RemoteFileManager.Instance.DidLoadFileInfos)
            {
                await RemoteFileManager.Instance.LoadFileInfos();
            }

            return (
                await SaveFileToSdCard(fileIndex, fields) &&
                await RemoteFileManager.Instance.SetFileMap(
                    fileIndex,
                    fields.Count
                ) &&
                await RemoteFileManager.Instance.SaveFileNameAndFileMapToSdCard() &&
                (await SerialSettingsManager.Instance.Save(fileNumber)) &&
                (await SetPermanentMarkingFileNo((short)fileNumber)).IsOk &&
                (await SetPermanentMarkingFileNoToSdCard()).IsOk
            );
		}

        /// <summary>
        /// </summary>Communication
        public static async Task<bool> ClearPermanentMarking()
        {
            var state = CommunicationClientManager.Instance.GetCurrentState ();
            var result = (await state.ProcessCommunication (UnsafeClearPermanentMarking)).Invoke();
            ;
            if (result.HasValue)
                return result.Value;
            return false;
        }

        static async Task<bool> UnsafeClearPermanentMarking ()
        {
            if (!RemoteFileManager.Instance.DidLoadFileInfos) {
                await RemoteFileManager.Instance.LoadFileInfos ();
            }
            return (
                (await SetPermanentMarkingFileNo ((short)0)).IsOk &&
                (await SetPermanentMarkingFileNoToSdCard ()).IsOk
            );
        }

        /// <summary>
        /// </summary>Communication
        public static async Task<bool> SavePermanentMarkingFileNo(short fileNo)
        {
            if (!RemoteFileManager.Instance.DidLoadFileInfos)
            {
                await RemoteFileManager.Instance.LoadFileInfos();
            }

            return (
                (await SetPermanentMarkingFileNo(fileNo)).IsOk &&
                (await SetPermanentMarkingFileNoToSdCard()).IsOk
            );
        }
	}
}

