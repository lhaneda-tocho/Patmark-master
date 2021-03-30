using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Text.RegularExpressions;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class RemoteFileManager
    {
        RemoteFileManager ()
        {
        }
        public static RemoteFileManager Instance = new RemoteFileManager();

        public List<RemoteFileInfo> FileInfos = new List<RemoteFileInfo>();

        public bool DidLoadFileInfos { get; private set; } = false;

        public async Task<bool> LoadFileInfos()
        {
            await CommandExecuter.LoadFileNameAndMapOnSdCard();

            var names = (await CommandExecuter.ReadRemoteFileNames()).Value;
            //Log.Debug ("Names ... " + names.Count);
            foreach (var name in names) {
                Log.Debug (name);
            }

            var maps = (await CommandExecuter.ReadRemoteFileMaps()).Value;
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

        public async Task<List<MBData>> LoadFile(int fileIndex)
        {
            // ファイルマップを展開しておく
            await CommandExecuter.LoadFileNameAndMapOnSdCard();
            if (await CommandExecuter.LoadFileFromSdCard(fileIndex))
            {
                // ファイル読み込み
                var mbData = await CommandExecuter.ReadCurrentFile();
                // シリアル読み込み
                await SerialSettingsManager.Instance.Reload(fileIndex + 1);

                return mbData;
            }
            return new List<MBData>();
        }

        public async Task<bool> SaveFile(int fileIndex, IReadOnlyCollection<Owner> owners)
        {
            if (fileIndex < FileInfos.Count)
            {
                FileInfos[fileIndex].NumOfField = owners.Count;

                // ファイル書き込み
                return (
                    await CommandExecuter.SaveFileToSdCard(
                        fileIndex,
                        owners.Select((owner) => { return owner.ToSerializable(); }).ToList()
                    )
                    && await SerialSettingsManager.Instance.Save(fileIndex + 1)
                );
            }
            return false;
        }

        public async Task<bool> SetFileName(int fileIndex, string fileName)
        {
            if (fileIndex < FileInfos.Count)
            {
                fileName = FixFileName(fileName, fileIndex);
                FileInfos[fileIndex].Name = fileName;
                await CommandExecuter.SetRemoteFileName((short)fileIndex, fileName);
            }
            return true;
        }

        public async Task<bool> SetFileMap(int fileIndex, int numOfField)
        {
            if (fileIndex < FileInfos.Count)
            {
                FileInfos[fileIndex].NumOfField = numOfField;
            }
            return (
                await CommandExecuter.SaveFileMapToSdCard(fileIndex, numOfField) &&
                (await CommandExecuter.SetRemoteFileMap((short)fileIndex, (short)numOfField)).IsOk
            );
        }

        public async Task<bool> SaveFileNameAndFileMapToSdCard()
        {
            return (await CommandExecuter.SaveFileNameToSdCard()).IsOk;
        }



        public class RemoteFileInfo
        {
            public string Name { get; set; } = "";
            public int NumOfField { get; set; } = 0;

            public RemoteFileInfo(string name, int numOfField)
            {
                Name = name;
                NumOfField = numOfField;
            }
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
    }
}

