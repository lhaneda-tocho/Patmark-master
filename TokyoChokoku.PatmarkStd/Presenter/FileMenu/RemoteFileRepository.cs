using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Presenter.Embossment;
            
namespace TokyoChokoku.Patmark.Presenter.FileMenu
{
    public class RemoteFileRepository: EmbossmentFileRepository
    {
        public RemoteFileManager Remote { get; private set; }

        public RemoteFileRepository()
        {
            RestoreFile(null);
            Remote = new RemoteFileManager(CurrentFile, new FileManagementActions((newFile) => {
                CurrentFile = newFile;
            }));
        }

        public async Task<List<RemoteFileInfo>> FileList() {
            return await Remote.GetFileList();
        }

        public async Task<bool> OnRetrieve(int index, RemoteFileInfo name)
        {
            var success = await Remote.Load(index, name.Name);
            if(success){
                AutoSave();
            }
            return success;
        }

        public async Task<bool> OnRetrieveLatest()
        {
            var success = await Remote.LoadLatest();
            AutoSave();
            return success;
        }

        public async Task<bool> OnClear(int index)
        {
            return await Remote.Clear(index);
        }

        /// <summary>
        /// ファイルを上書きします。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onEmpty">書き込むファイルが空の場合に, 呼び出し元のスレッドで呼ばれます。</param>
        /// <returns></returns>
        public Task<bool> OnReplace(int index, Action onEmpty)
        {
            if(IsEmpty)
            {
                try
                {
                    onEmpty();
                    return Task.FromResult(false);
                } catch (Exception ex)
                {
                    return Task.FromException<bool>(ex);
                }
            }
            return Remote.Save(index);
        }

        public async Task<bool> OnRename(int index, string name)
        {
            return await Remote.SaveFileName(index, name);
        }

        public async Task<List<RemoteFileInfo>> OnReload()
        {
            return await Remote.GetFileListForcibly();
        }

        public string ToFileName(int index, string name)
        {
            return RemoteFileIO.ToFileNameWith(name, index);
        }
    }
}
