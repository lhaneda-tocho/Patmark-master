using System;
namespace TokyoChokoku.Patmark
{
    public class FileManagementActions
    {
        public Action<FileContext> FileReplaced { get; private set; }

        public FileManagementActions(
            Action<FileContext> fileReplaced
        ){
            FileReplaced = fileReplaced;  
        }
    }
}
