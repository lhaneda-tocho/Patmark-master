using System;
using System.Collections.Generic;
using System.IO;
using Monad;

namespace TokyoChokoku.Patmark
{
    public class FileSourceFromLocal : IFileSource
    {
        readonly PathName path;

        public string From {
            get {
                return CommonStrings.Instance.FileSourceLabelLocal + " : " + path.Simple;
            }
        }

        public IFileSource Clone => new FileSourceFromLocal(path);

        public bool IsAvailable {
            get {
                return File.Exists (path.Full);
            }
        }

        public FileSourceFromLocal (PathName path)
        {
            this.path = path;
        }

        public void Autosave (FileOwner owner)
        {
            AutoSaveManager.SaveFileName(Option.Return(() => From));
            AutoSaveManager.SaveControllerNumber(Option.Nothing<int>());
            AutoSaveManager.Overwrite (owner);
        }
    }
}

