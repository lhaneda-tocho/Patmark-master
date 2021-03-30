using System;
using System.Collections.Generic;
using System.IO;

using TokyoChokoku.Patmark.Presenter.Embossment;
using Monad;
namespace TokyoChokoku.Patmark.Presenter.FileMenu
{
    public class LocalFileRepository: EmbossmentFileRepository
    {
        public readonly string DirectoryPath = LocalFilePathGeneratorPublisher.Instance.DirectorySave();
        LocalFileManager Source { get; }

        public LocalFileRepository()
        {
            // AutoSaveファイルの取得
            RestoreFile(FileContext.Empty());
            // フェイルセーフ
            SaveQuickModeAsBlankIfNeeded();
            Source = new LocalFileManager(CurrentFile, new FileManagementActions((newFile) => {
                CurrentFile = newFile;
            }));
        }

        /// <summary>
        /// 必要であればディレクトリやファイルの作成を行います．
        /// </summary>
        public void ReadyDirectoriesIfNeeded() {
            // ディレクトリの作成
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        /// <summary>
        /// Fire Rename event with the specified file name text.
        /// </summary>
        /// <returns><c>true</c>, if success rename, <c>false</c> otherwise.</returns>
        /// <param name="path">Path.</param>
        public bool OnRenameWithText(PathName path, string fileName, Action onInvalidName, Action onSameName, Action<PathName, PathName> onRenameOver)
        {
            var newPath = Source.GetFilePath(fileName);
            if (!newPath.HasValue())
            {
                onInvalidName();
                return false;
            }
            else if(newPath.Value().Full == path.Full)
            {
                onSameName();
                return false;
            }
            else if (Source.Exists(newPath.Value()))
            {
                onRenameOver(path, newPath.Value());
                return false;
            }
            else
            {
                OnRename(path, newPath.Value());
                return true;
            }
        }

        /// <summary>
        /// Fire SaveAs event with the specified file name text.
        /// </summary>
        /// <returns><c>true</c>, if success save as, <c>false</c> otherwise.</returns>
        /// <param name="text">Text.</param>
        /// <param name="onInvalidName">Invalid name event.</param>
        /// <param name="onSaveOver">On save over event.</param>
        public bool OnSaveAsWithText(string text, Action onInvalidName, Action<PathName> onSaveOver, Action onEmpty, Action onImcompatibleSerial)
        {
            var path = Source.GetFilePath(text);

            // var path = FilePathManager.TryConvertToPpgPath (text);

            if (path.HasValue())
            {
                return OnSaveAs(path.Value(), onSaveOver, onEmpty, onImcompatibleSerial);
            }
            else
            {
                onInvalidName();
                return false;
            }
        }

        /// <summary>
        /// Fire SaveAs event.
        /// </summary>
        /// <returns><c>true</c> if success save as, <c>false</c> if fired save over event.</returns>
        /// <param name="path">Path.</param>
        /// <param name="onSaveOver">On save over.</param>
        /// <param name="onEmpty">is invoked if the file is empty.</param>
        public bool OnSaveAs(PathName path, Action<PathName> onSaveOver, Action onEmpty, Action onImcompatibleSerial)
        {
            onSaveOver = onSaveOver ?? throw new NullReferenceException();
            if (IsEmpty)
            {
                onEmpty();
                return false;
            }
            if (Source.Loaded.HasSerial)
            {
                onImcompatibleSerial();
                return false;
            }
            if (Source.Exists(path))
            {
                onSaveOver(path);
                return false;
            }
            Source.SaveAs(path);
            return true;
        }

        /// <summary>
        /// 上書き保存します。
        /// </summary>
        /// <param name="path">保存先のパス</param>
        /// <param name="onEmpty">空の場合に呼び出されます。</param>
        /// <returns>保存に成功したら true, そうでなければ false.</returns>
        public bool OnSaveOver(PathName path, Action onEmpty, Action onImcompatibleSerial)
        {
            if (Source.Loaded.IsEmpty)
            {
                onEmpty();
                return false;
            }
            if (Source.Loaded.HasSerial)
            {
                onImcompatibleSerial();
                return false;
            }
            Source.SaveOver(path);
            return true;
        }


        public void OnLoad(PathName path)
        {
            Source.Load(path);
            CurrentFile = Source.Loaded;
            AutoSave();
        }

        public void OnRename(PathName name, PathName newName)
        {
            Source.Rename(name, newName);
        }

        public void OnRenameOver(PathName name, PathName newName)
        {
            Source.Delete(newName);
            Source.Rename(name, newName);
        }


        /// <summary>
        /// Fire Delete event.
        /// </summary>
        /// <returns><c>true</c> if success deletion, <c>false</c> if not found file.</returns>
        /// <param name="path">Path.</param>
        public bool OnDelete(PathName path)
        {
            if (!Source.Exists(path))
                return false;
            Source.Delete(path);
            return true;
        }

        /// <summary>
        /// ファイル名リストです。
        /// ソートしたリストを返す必要があります。
        /// </summary>
        /// <returns></returns>
        public List<PathName> FileList() {
            var list = Source.FileList;
            list.Sort((x, y) => {
                if (ReferenceEquals(x, y))
                    return 0;
                if (ReferenceEquals(x, null))
                    return -1;
                if (ReferenceEquals(y, null))
                    return +1;
                return x.Full.CompareTo(y.Full);
            });
            return list;
        }
    }
}
