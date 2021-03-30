using System;

namespace TokyoChokoku.Patmark
{
    public interface ILocalFilePathGenerator
    {
        string DirectorySave();
        string DirectoryAutoSave();
    }
}
