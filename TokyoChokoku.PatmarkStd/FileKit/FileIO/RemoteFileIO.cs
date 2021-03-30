using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Communication;
using TokyoChokoku.SerialModule.Setting;
using System;
using System.Diagnostics;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// </summary>
    public static class RemoteFileIO
    {
        public static List<RemoteFileInfo> FileInfos { get; private set; } = new List<RemoteFileInfo>();
        public static bool DidLoadFileInfos { get; private set; } = false;

        static CommandExecutor Exe
        {
            get
            {
                return CommunicationClient.Instance.CommandExecutor;
            }
        }

        public static async Task<bool> LoadFileInfos()
        {
            await Exe.LoadFileNameAndMapOnSdCardIfNeeded();

            var names = (await Exe.ReadRemoteFileNames()).Value;
            //Log.Debug ("Names ... " + names.Count);
            foreach (var name in names) {
                Log.Debug (name);
            }

            var maps = (await Exe.ReadRemoteFileMaps()).Value;
            //Log.Debug ("Maps ... " + maps.Count);
            //foreach (var map in maps) {
            //    Log.Debug (map.ToString());
            //}

            FileInfos.Clear ();
            for (var i = 0; i < names.Count && i < maps.Count; i++) {
                FileInfos.Add (new RemoteFileInfo (names[i], maps[i]));
            }

            DidLoadFileInfos = true;

            return true;
        }

        public static async Task<List<MBData>> LoadLatestFile()
        {
            return await LoadFile(Sizes.IndexOfRemotePermanentFile);
        }

        public static async Task<List<MBData>> LoadFile(int fileIndex)
        {
            if (await Exe.LoadFileFromSdCard(fileIndex))
            {
                // ファイル読み込み
                var mbData = await Exe.ReadCurrentFile();
                // シリアル読み込み
                await SerialGlobalSetting.RetrieveFromController(fileIndex + 1);
                // パーマネント打刻ファイル番号を記録
                //await Exe.SetPermanentMarkingFileNo((short)(fileIndex + 1));

                return mbData;
            }
            return new List<MBData>();
        }

        public static async Task<bool> SaveFile(int fileIndex, IEnumerable<MBData> fields, bool sendFieldCount)
        {
            if (0 <= fileIndex && fileIndex < FileInfos.Count)
            {
                var fieldList = fields?.ToList() ?? new List<MBData>();
                // ファイル書き込み
                bool success;
                try
                {
                    success = await Exe.SaveFileToSdCard(
                        fileIndex,
                        fieldList,
                        sendFieldCount: false
                    );
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine($"Logic Error: {ex}");
                    return false;
                }
                if (!success)
                    return false;
                if (sendFieldCount)
                {
                    success = await SetFileMapAndSaveToSdCard(fileIndex, (short)fieldList.Count);
                    if (!success)
                        return false;
                }
                return await SerialGlobalSetting.SendToController(fileIndex + 1);
            }
            return false;
        }

        public static async Task<bool> ClearFileAt(int fileIndex)
        {
            return await SetFileNameAndSaveToSdCard(
                    fileIndex,
                    ""
                )
                && await SetFileMapAndSaveToSdCard(
                    fileIndex,
                    0
                );
        }

        public static async Task<bool> SetFileNameAndSaveToSdCard(int fileIndex, string fileName)
        {
            async Task<bool> Send(string fixedName)
            {
                return await Exe.RenameFileAndSaveToSdCard(fileIndex, fixedName, (nextFileIndex, nextFileName) => {
                    FileInfos[nextFileIndex].Name = nextFileName;
                });
            }
            async Task<bool> Block()
            {
                try
                {
                    return await Send(FixFileName(fileName, fileIndex));
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine($"Logic Error: {ex}");
                    return false;
                }
            }
            return await Block();
        }

        private static async Task<bool> SetFileMapAndSaveToSdCard(int fileIndex, int numOfFields)
        {
            async Task<bool> Send()
            {
                return await Exe.SetFileMapAndSaveToSdCard(fileIndex, (short)numOfFields, (nextFileIndex, nextNumOfFields) => {
                    FileInfos[nextFileIndex].NumOfField = nextNumOfFields;
                });
            }
            async Task<bool> Block()
            {
                try
                {
                    return await Send();
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine($"Logic Error: {ex}");
                    return false;
                }
            }
            return await Block();
        }

        static string FixFileName(string fileName, int fileIndex)
        {
            if (fileName == null)
            {
                fileName = "";
            }
            fileName = Regex.Replace(fileName, "^F[0-9][0-9][0-9]_", "");
            fileName = string.Format("F{0:D3}_{1}", fileIndex + 1, fileName);
            var maxNum = Sizes.BytesOfRemoteFileName;
            return (fileName.Length > maxNum) ? fileName.Substring(0, maxNum) : fileName;
        }

        public static string ToFileNameWith(string fileName, int fileIndex)
        {
            return FixFileName(fileName, fileIndex);
        }

        public static async Task ClearCurrentFile() {
            await Exe.ClearPermanentMarkingFileNo();
        }
    }
}

