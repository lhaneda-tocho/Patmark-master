using System;
using System.Collections.Generic;
using System.IO;
using Functional.Maybe;


namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class AutoSaveFieldSourceFromRemoteFile : FieldSource
    {
        readonly int    indexOfFile;
        readonly string name;

        public string From {
            get {
                return "file-category.remote".Localize () + " No." + (indexOfFile + 1) + " : " + name;
            }
        }

        public AutoSaveFieldSourceFromRemoteFile (int indexOfFile, string name)
        {
            this.indexOfFile = indexOfFile;
            this.name = name;
        }


        public Maybe<IList<iOSOwner>> TryLoad ()
        {
            return AutoSaveManager.LoadPpgAsList ();
        }

        public void Autosave (iOSFieldContext context)
        {
            AutoSaveManager.SaveFileName (name.ToMaybe ());
            AutoSaveManager.SaveControllerNumber (indexOfFile.ToMaybe ());
            AutoSaveManager.Overwrite (context);
        }
    }
}
