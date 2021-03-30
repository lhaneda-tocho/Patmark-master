using System;
using System.Collections.Generic;
using System.IO;

using Monad;

namespace TokyoChokoku.Patmark
{
    public class FileSourceFromRemote : IFileSource
    {
        readonly int    indexOfFile;
        readonly string name;

        public string From {
            get {
                return CommonStrings.Instance.FileSourceLabelRemote + " No." + (indexOfFile + 1) + " : " + name;
            }
        }

        public IFileSource Clone => new FileSourceFromRemote(indexOfFile, name);

        public FileSourceFromRemote (int indexOfFile, string name)
        {
            this.indexOfFile = indexOfFile;
            this.name = name;
        }

        public void Autosave (FileOwner owner)
        {
            AutoSaveManager.SaveFileName(Option.Return(() => name));
            AutoSaveManager.SaveControllerNumber (Option.Return(() => indexOfFile));
            AutoSaveManager.Overwrite (owner);
        }
    }
}
