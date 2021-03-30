using System;
using System.Collections.Generic;
using System.IO;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class FieldSourceFromLocalFile : FieldSource
    {
        readonly PathName path;

        public string From {
            get {
                return "file-category.local".Localize() + " : " + path.Simple;
            }
        }

        public bool IsAvailable {
            get {
                return File.Exists (path.Full);
            }
        }

        public FieldSourceFromLocalFile (PathName path)
        {
            this.path = path;
        }

        public IList<iOSOwner> Load ()
        {
            try {
                return LocalFileManager.LoadPpgAsList (path.Full);
            } catch (IOException ex) {
                throw new FieldSourceFailureToLoadException (ex);
            }
        }

        public Maybe<IList<iOSOwner>> TryLoad ()
        {
            if (File.Exists (path.Full)) {
                try {
                    return LocalFileManager.LoadPpgAsList (path.Full).ToMaybe<IList<iOSOwner>> ();
                } catch (IOException ex) {
                    Console.WriteLine (ex.Message);
                }
            }
            return Maybe<IList<iOSOwner>>.Nothing;
        }

        public void Autosave (iOSFieldContext context)
        {
            AutoSaveManager.Overwrite (context);
        }
    }
}

