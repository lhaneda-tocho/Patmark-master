using System;
using System.Collections.Generic;
using System.IO;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class AutoSaveFieldSource : FieldSource
    {
        public string From {
            get {
                return "file-category.autosave".Localize ();
            }
        }

        public void Autosave (iOSFieldContext context)
        {
            AutoSaveManager.Overwrite (context);
        }

        public Maybe<IList<iOSOwner>> TryLoad ()
        {
            return AutoSaveManager.LoadPpgAsList ();
        }
    }
}

