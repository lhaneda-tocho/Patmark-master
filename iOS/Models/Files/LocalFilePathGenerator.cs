using System;
using System.IO;

namespace TokyoChokoku.Patmark.iOS
{
    public class LocalFilePathGenerator : ILocalFilePathGenerator
    {
        
        public string DirectorySave()
        {
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(documentPath, @"save/text");
        }

        public string DirectoryAutoSave()
        {
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(documentPath, @"auto_save/text");
        }

    }
}

