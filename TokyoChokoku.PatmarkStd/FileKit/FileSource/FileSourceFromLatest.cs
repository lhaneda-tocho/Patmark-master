using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using Monad;


namespace TokyoChokoku.Patmark
{
    public class FileSourceFromLatest : IFileSource
    {
        public string From {
            get {
                return CommonStrings.Instance.FileSourceLabelLatest;
            }
        }

        public IFileSource Clone => new FileSourceFromLatest();

        public void Autosave (FileOwner owner)
        {
            AutoSaveManager.SaveFileName(Option.Return(()=>From));
            AutoSaveManager.SaveControllerNumber(Option.Nothing<int>());
            AutoSaveManager.Overwrite(owner);
        }
    }
}

