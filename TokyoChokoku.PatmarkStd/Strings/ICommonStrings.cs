using System;
namespace TokyoChokoku.Patmark
{
    public interface ICommonStrings
    {
        string FileSourceLabelLocal { get; } //  "file-category.local".Localize()
        string FileSourceLabelRemote { get; } // "file-category.remote".Localize () 
        string FileSourceLabelLatest { get; } // "file-category.latest-marking-file".Localize ()
    }
}
