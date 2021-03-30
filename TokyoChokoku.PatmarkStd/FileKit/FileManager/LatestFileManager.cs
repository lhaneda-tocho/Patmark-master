using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.Patmark
{
    public class LatestFileManager : IFileManager
    {
        public FileContext Loaded { get; private set; }
        public FileManagementActions Actions { get; private set; }


        public LatestFileManager (FileContext loaded, FileManagementActions actions)
        {
            Loaded = loaded ?? throw new NullReferenceException("loaded をセットしてください。");
            Actions = actions ?? throw new NullReferenceException("actions をセットしてください。");
        }

        public Task<bool> Load ()
        {
            return ProtectOnMarking(async () => { 
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


        Task<T> ProtectOnMarking<T>(Func<Task<T>> task)
        {
            return new ControllerIO.StatusIO().ProtectOnMarking(task);
        }

    }
}

