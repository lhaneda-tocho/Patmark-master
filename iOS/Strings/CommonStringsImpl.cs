using System;
namespace TokyoChokoku.Patmark.iOS
{
    public class CommonStringsImpl : ICommonStrings
    {
       
        public string FileSourceLabelLocal => "file-category.local".Localize();

        public string FileSourceLabelRemote => "file-category.remote".Localize();

        public string FileSourceLabelLatest => "file-category.latest-marking-file".Localize();

    }
}
