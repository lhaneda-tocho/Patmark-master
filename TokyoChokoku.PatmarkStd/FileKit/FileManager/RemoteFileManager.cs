using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.ControllerIO;


namespace TokyoChokoku.Patmark
{
    using RemoteFileInfoList = List<RemoteFileInfo>;

    public class RemoteFileManager : IFileManager
    {
        public FileContext Loaded { get; private set; }
        public FileManagementActions Actions { get; private set; }


        public RemoteFileManager (FileContext loaded, FileManagementActions actions)
        {
            Loaded = loaded ?? throw new NullReferenceException("loaded をセットしてください。");
            Actions = actions ?? throw new NullReferenceException("actions をセットしてください。");
        }

        public Task<RemoteFileInfoList> GetFileList ()
        {
            return Unstable(async () =>
            {
                if (CommunicationClient.Instance.Ready)
                {
                    if (!RemoteFileIO.DidLoadFileInfos)
                    {
                        await RemoteFileIO.LoadFileInfos();
                    }
                }
                return RemoteFileIO.FileInfos;
            });
        }

        public Task<RemoteFileInfoList> GetFileListForcibly()
        {
            return Unstable(async () => 
            {
                if (CommunicationClient.Instance.Ready)
                {
                    await RemoteFileIO.LoadFileInfos();
                }
                return RemoteFileIO.FileInfos; 
            });
        }

        public Task<bool> Save(int indexOfFile)
        {
            return ProtectOnMarking(async () =>
            {
                var saved = (
                    await RemoteFileIO.SaveFile(
                        indexOfFile,
                        Loaded.Owner.Serializable,
                        sendFieldCount: true
                    )
                );
                return saved;
            });
        }

        public Task<bool> Clear(int indexOfFile)
        {
            return ProtectOnMarking(async () =>
            {
                var saved = await RemoteFileIO.ClearFileAt(indexOfFile);
                return saved;
            });
        }

        public Task<bool> SaveFileName (int indexOfFile, string fileName)
        {
            return Unstable(async () => 
                await RemoteFileIO.SetFileNameAndSaveToSdCard(
                    indexOfFile,
                    fileName
                )
            );
        }

        public Task<bool> Load(int indexOfFile, string fileName)
        {
            return ProtectOnMarking(async () =>
            {
                var fields = await RemoteFileIO.LoadFile(indexOfFile);
                if (fields.Count > 0)
                {
                    Loaded = new FileContext(
                        new FileOwner(fields),
                        new FileSourceFromRemote(indexOfFile, fileName)
                    );
                    Actions.FileReplaced(Loaded);
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public Task<bool> LoadLatest()
        {
            return ProtectOnMarking(async () =>
            {
                var fields = await RemoteFileIO.LoadLatestFile();
                if (fields.Count > 0)
                {
                    Loaded = new FileContext(
                        new FileOwner(fields),
                        new FileSourceFromRemote(Sizes.IndexOfRemotePermanentFile, "latest")
                    );
                    Actions.FileReplaced(Loaded);
                    return true;
                }
                else
                {
                    return false;
                }
            });
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
            return new StatusIO().ProtectOnMarking<T>(task);
        }

    }
}

