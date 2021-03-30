using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using Monad;


namespace TokyoChokoku.Patmark
{
    public class FileSourceFromOther : IFileSource
    {
        public string From {
            get {
                return "";
            }
        }

        public IFileSource Clone => new FileSourceFromOther();

        public void Autosave (FileOwner owner)
        {
            AutoSaveManager.SaveFileName(Option.Nothing<string>());
            AutoSaveManager.SaveControllerNumber(Option.Nothing<int>());
            AutoSaveManager.Overwrite (owner);
        }
    }
}

