using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class RemoteFileMenuSource
    {
        
        // 読み込み場所を記憶
        FieldSource Source { get; set; }

        // FileLoaded 後の時のみ nonnull
        // 読み込み結果を記憶
        IList<iOSOwner> Loaded { get; set; }

        // 保存するリスト
        List<iOSOwner> OwnerList { get; }

        // ファイル操作の結果に応じて呼び出す必要のあるメソッドたち
        FileMenuActionSource Actions { get; }

        public RemoteFileMenuSource (FieldSource source, List<iOSOwner> ownerList, FileMenuActionSource actions)
        {
            if (source == null || ownerList == null || actions == null)
                throw new NullReferenceException ();
            Source = source;
            OwnerList = ownerList;
            Actions = actions;
        }

        public async Task<List<RemoteFileManager.RemoteFileInfo>> GetFileList ()
        {
            if (CommunicationClientManager.Instance.IsOnline ()) {
                if (!RemoteFileManager.Instance.DidLoadFileInfos) {
                    await RemoteFileManager.Instance.LoadFileInfos ();
                }
            }
            return RemoteFileManager.Instance.FileInfos;
        }

        public async Task<List<RemoteFileManager.RemoteFileInfo>> GetFileListForcibly()
        {
        	if (CommunicationClientManager.Instance.IsOnline())
        	{
                await RemoteFileManager.Instance.LoadFileInfos();
        	}
        	return RemoteFileManager.Instance.FileInfos;
        }

        public async Task<bool> Save (int indexOfFile, string fileName)
        {
            var saved = (
                await RemoteFileManager.Instance.SaveFile (
                    indexOfFile,
                    OwnerList
                ) &&
                await RemoteFileManager.Instance.SetFileName(
                    indexOfFile,
                    fileName
                ) &&
                await RemoteFileManager.Instance.SetFileMap(
                    indexOfFile,
                    OwnerList.Count
                ) &&
                await RemoteFileManager.Instance.SaveFileNameAndFileMapToSdCard() &&
                await SerialSettingsManager.Instance.Save(indexOfFile)
            );
            return saved;
        }

        public async Task<bool> SaveFileName (int indexOfFile, string fileName)
        {
            return (
                await RemoteFileManager.Instance.SetFileName(
                    indexOfFile,
                    fileName
                ) &&
                await RemoteFileManager.Instance.SaveFileNameAndFileMapToSdCard()
            );
        }

        public async Task<bool> Load (int indexOfFile, string fileName)
        {
            var source = new FieldSourceFromRemoteFile (indexOfFile, fileName);
            var data = await source.TryLoadAsync ();
            if (data.HasValue) {
                Source = source;
                Loaded = data.Value;
                return true;
            } 
            return false;
        }

        public Maybe<Action> GetAction (FileMenuDismissedEvent ev)
        {
            switch (ev.Cause) {
            case FileMenuDismissedEvent.BecauseOf.FileLoaded: {
                    if (Loaded == null)
                        throw new NullReferenceException ();
                    return Actions.WillExitAfterLoad (Source, Loaded).ToMaybe ();
                }

            default: {
                    return Actions.WillExit (Source).ToMaybe ();
                }
            }
        }
    }
}

