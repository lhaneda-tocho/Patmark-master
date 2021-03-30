using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.Communication;
using System.Collections;

using TokyoChokoku.SerialModule.Setting;
using System.Diagnostics;

namespace TokyoChokoku.ControllerIO
{
    public class FileIO
    {
        public CommunicationClient Client       { get; private set; }
        public CommandExecutor     Exec         { get; }
        public EmbossmentIO        EmbossmentIO { get; internal set; } // Required
        public StatusIO            StatusIO     { get; private  set; }

        private RemoteFileInfoList Cache { get; set; } = null;

        internal FileIO(CommandExecutor exec)
        {
            Exec = exec ?? throw new NullReferenceException();
        }


        public static FileIO Create(CommunicationClient client)
        {
            var exec = client?.CommandExecutor ?? throw new NullReferenceException();
            var fio = new FileIO(exec);
            var eio = new EmbossmentIO(exec);
            var sio = new StatusIO(exec);
            // Injection
            fio.EmbossmentIO = eio;
            fio.StatusIO     = sio;
            fio.Client       = client;
            eio.FileIO       = fio;
            eio.Client       = client;
            return fio;
        }


        /// <summary>
        /// Draws the or load.
        /// </summary>
        /// <returns>The or load.</returns>
        public async Task<RemoteFileInfoList> GetCachedOrLoadFileInfo()
        {
            var c = Cache;
            return c ?? await LoadFileInfo();
        }


        /// <summary>
        /// Loads the file information list.
        /// </summary>
        /// <returns>The file info.</returns>
        public Task<RemoteFileInfoList> LoadFileInfo()
        {
            return Unstable(async () =>
            {
                await Exec.LoadFileNameAndMapOnSdCardIfNeeded();

                var names = (await Exec.ReadRemoteFileNames()).Value;
                //Log.Debug ("Names ... " + names.Count);
                foreach (var name in names)
                {
                    Console.WriteLine(name);
                }

                var maps = (await Exec.ReadRemoteFileMaps()).Value;
                //Log.Debug ("Maps ... " + maps.Count);
                //foreach (var map in maps) {
                //    Log.Debug (map.ToString());
                //}

                var newList = names.Zip(maps, (name, map) => {
                    return new RemoteFileInfo(name, map);
                });

                var result = new RemoteFileInfoList(this, newList.ToList());
                Cache = result;
                return result;
            });
        }



        /// <summary>
        /// Saves the permanent marking file no.
        /// </summary>
        /// <returns>The permanent marking file no.</returns>
        /// <param name="fileNo">File no.</param>
        public Task<bool> SavePermanentMarkingFileNo(short fileNo)
        {
            return ProtectOnMarking(async () =>
                (await Exec.SetPermanentMarkingFileNo(fileNo)).IsOk &&
                (await Exec.SetPermanentMarkingFileNoToSdCard()).IsOk
            );
        }


        ///// <summary>
        ///// Loads the file.
        ///// </summary>
        ///// <returns>The file.</returns>
        ///// <param name="fileIndex">File index.</param>
        //public Task<FileLoadResult> LoadFile(int fileIndex)
        //{
        //    return ProtectOnMarking(async () =>
        //    {
        //        if (fileIndex < 0 && Sizes.NumOfRemoteFile <= fileIndex)
        //        {
        //            return new FileLoadResult(new List<MBData>(), new SerialSetting.Mutable());
        //        }
        //        if (await Exec.LoadFileFromSdCard(fileIndex))
        //        {
        //            // ファイル読み込み
        //            var mbData = await Exec.ReadCurrentFile();
        //            // シリアル読み込み
        //            var ss = await SerialSettingIO.RetrieveFromController(fileIndex + 1);
        //            return new FileLoadResult(mbData, ss);
        //        }
        //        var result = new FileLoadResult(new List<MBData>(), new SerialSetting.Mutable());
        //        return result;
        //    });
        //}

        public Task<bool> SavePermanentFile(List<MBData> fields)
        {
            return ProtectOnMarking(async () =>
            {
                var fileIndex = Sizes.IndexOfRemotePermanentFile;
                //            var fileIndex = 23;
                var fileNumber = fileIndex + 1;

                {
                    if (fields.Count != 0) // 空の時は通信しないようにする (失敗する確率を減らす)
                    {
                        try
                        {
                            if (!await Exec.SaveFileToSdCard(fileIndex, fields, sendFieldCount: false))
                                return false;
                        } catch (ArgumentException ex)
                        {
                            Debug.WriteLine($"Logic Error: {ex}");
                            return false;
                        }
                    }
                    if (!await SetFileMapAndSaveToSdCard(fileIndex, fields.Count))
                        return false;
                    if (!(await Exec.SetPermanentMarkingFileNo((short)fileNumber)).IsOk)
                        return false;
                    await Task.Delay(50);
                    if (!(await Exec.SetPermanentMarkingFileNoToSdCard()).IsOk)
                        return false;
                    await Task.Delay(50);
                    if (!await SerialGlobalSetting.SendToController(fileNumber))
                        return false;
                    return true;
                }
            });
        }

        public Task<bool> ClearPermanentFile()
        {
            return SavePermanentFile(new List<MBData>());
        }

        //public Task<bool> SaveFile(int fileIndex, List<MBData> serializable, RemoteFileInfoList list = null)
        //{
        //    return ProtectOnMarking(async () =>
        //    {
        //        if (fileIndex < Sizes.NumOfRemoteFile)
        //        {
        //            if (list != null)
        //                list[fileIndex].NumOfField = serializable.Count;

        //            // ファイル書き込み
        //            return (
        //                await Exec.SaveFileToSdCard(
        //                    fileIndex,
        //                    serializable
        //                )
        //                //&& await SerialGlobalSetting.SendToController(fileIndex + 1)
        //                && await SavePermanentMarkingFileNo((short)(fileIndex + 1))
        //                && await SerialGlobalSetting.SendToController(fileIndex + 1)
        //            );
        //        }
        //        return false;
        //    });
        //}


        /// <summary>
        /// ファイル名を書き換えるメソッドです。この実行でSDカードへ保存されます。
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<bool> RenameAndSaveToSdCard(int fileIndex, string fileName, RemoteFileInfoList list = null)
        {
            async Task<bool> Send(string fixedName)
            {
                return await Exec.RenameFileAndSaveToSdCard(fileIndex, fixedName, (nextFileIndex, nextFileName) => {
                    if (ReferenceEquals(list, null))
                        return;
                    list[nextFileIndex].Name = nextFileName;
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
            return Unstable(() => Block());
        }

        /// <summary>
        /// ファイル名とファイルマップを指定して保存します。この実行でSDカードへ保存されます。
        /// </summary>
        /// <param name="fileIndex">ファイルインデックス</param>
        /// <param name="numOfFields">フィールド数</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task<bool> SetFileMapAndSaveToSdCard(int fileIndex, int numOfFields, RemoteFileInfoList list = null)
        {
            async Task<bool> Send()
            {
                return await Exec.SetFileMapAndSaveToSdCard(fileIndex, (short)numOfFields, (nextFileIndex, nextNumOfFields) => {
                    if (ReferenceEquals(list, null))
                        return;
                    list[nextFileIndex].NumOfField = nextNumOfFields;
                });
            }
            async Task<bool> Block()
            {
                try
                {
                    return await Send();
                } catch (ArgumentException ex)
                {
                    Debug.WriteLine($"Logic Error: {ex}");
                    return false;
                }
            }
            return Unstable(() => Block());
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


        async Task<T> Unstable<T>(Func<Task<T>> task)
        {
            try
            {
                return await task();
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IOException("", ex);
            }
        }


        Task<T> ProtectOnMarking<T>(Func<Task<T>> task)
        {
            return StatusIO.ProtectOnMarking(task);
        }
    }

    


    public class RemoteFileInfo
    {
        public string Name    { get; set; } = "";
        public int NumOfField { get; set; } = 0;

        public RemoteFileInfo(string name, int numOfField)
        {
            Name = name;
            NumOfField = numOfField;
        }
    }



    public class FileLoadResult
    {
        public readonly IList<MBData> MBData;
        public readonly SerialSetting SerialSetting;

        public FileLoadResult(IList<MBData> mbData, SerialSetting serialSetting)
        {
            MBData = mbData;
            SerialSetting = serialSetting;
        }

        /// <summary>
        /// シリアル設定をグローバル設定に反映します
        /// </summary>
        public void CommitSettings() {
            SerialGlobalSetting.SetSetting(SerialSetting);
        }
    }




    public class RemoteFileInfoList : IEnumerable<RemoteFileInfo>
    {
        readonly FileIO FileIO;
        readonly IList<RemoteFileInfo> List;

        public int Count => List.Count;

        public RemoteFileInfoList(FileIO io, IList<RemoteFileInfo> list)
        {
            List = list;
            FileIO = io;
        }

        public RemoteFileInfo this[int fileIndex] {
            get => List[fileIndex];
            set => List[fileIndex] = value;
        }

        ///// <summary>
        ///// Loads the file.
        ///// </summary>
        ///// <returns>The file.</returns>
        ///// <param name="fileIndex">File index.</param>
        //public async Task<FileLoadResult> LoadFile(int fileIndex)
        //{
        //    return await FileIO.LoadFile(this, fileIndex);
        //}

        //public async Task<bool> SaveFile(int fileIndex, List<MBData> serializable)
        //{
        //    return await FileIO.SaveFile(this, fileIndex, serializable);
        //}

        //public async Task<bool> SavePermanentFile(List<MBData> fields)
        //{
        //    return await FileIO.SavePermanentFile(this, fields);
        //}

        //public async Task<bool> ClearPermanentFile()
        //{
        //    return await FileIO.ClearPermanentFile(this);
        //}

        //public async Task<bool> SetFileName(int fileIndex, string fileName)
        //{
        //    return await FileIO.SetFileName(this, fileIndex, fileName);
        //}

        //public async Task<bool> SetFileMap(int fileIndex, int numOfField)
        //{
        //    return await FileIO.SetFileMap(this, fileIndex, numOfField);
        //}

        #region IEnumerable
        public IEnumerator<RemoteFileInfo> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
        #endregion
    }
}
