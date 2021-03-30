using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.Patmark
{
    public class FileClearanceManager : IFileManager
    {
        public FileContext Loaded { get; private set; }
        public FileManagementActions Actions { get; private set; }


        public FileClearanceManager (FileContext loaded, FileManagementActions actions)
        {
            Loaded = loaded ?? throw new NullReferenceException("loaded をセットしてください。");
            Actions = actions ?? throw new NullReferenceException("actions をセットしてください。");
        }

        public void Clear ()
        {
            Loaded = new FileContext(
                new FileOwner(new List<MBData>(){}),
                    new FileSourceFromOther()
                );
            Actions.FileReplaced(Loaded);
        }
    }
}

