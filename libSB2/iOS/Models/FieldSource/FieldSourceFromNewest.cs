using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Functional.Maybe;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class FieldSourceFromNewest : FieldSource
    {
        FieldSourceFromRemoteFile Parmanent = FieldSourceFromRemoteFile.ForParmanent();

        public string From {
            get {
                return "file-category.latest-marking-file".Localize ();
            }
        }


        public Maybe<IList<iOSOwner>> TryLoad ()
        {
            var task = TryLoadAsync ();
            task.Start ();
            task.Wait ();
            return task.Result;
        }

        public async Task<Maybe<IList<iOSOwner>>> TryLoadAsync ()
        {
            return await Parmanent.TryLoadAsync();
        }

        public void Autosave (iOSFieldContext context)
        {
            AutoSaveManager.Overwrite (context);
        }
    }
}

