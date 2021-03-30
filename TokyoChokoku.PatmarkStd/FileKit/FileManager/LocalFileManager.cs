using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Monad;

namespace TokyoChokoku.Patmark
{

    public class LocalFileManager : IFileManager
    {
        // 
        public FileContext Loaded { get; private set; }
        public FileManagementActions Actions { get; private set; }

        private string DirectoryPath = LocalFilePathGeneratorPublisher.Instance.DirectorySave();


        ILocalFileIO MyFileIO { get; }


        /// <summary>
        /// Complex にファイルパスを与えてリストを構成します．
        /// </summary>
        /// <value>The file list.</value>
        public List<PathName> FileList
        {
            get
            {
                IList<LocalFilePath> paths;
                try
                {
                    paths = MyFileIO.FileNameList;
                } catch(Exception ex)
                {
                    // TODO: エラー通知の入れ方を考える
                    // TODO: 例外処理が必要
                    Log.Error($"{ex}: {ex.Message}");
                    Log.Error($"{ex.StackTrace}");
                    paths = new List<LocalFilePath>();
                }
                return (
                    from e in paths
                    select PathName.FromPath(e.ToStringNormalized())
                ).ToList();
            }
        }

        /// <summary>
        /// コンストラクタ．null を入れると例外が発生します．
        /// </summary>
        public LocalFileManager (FileContext loaded, FileManagementActions actions)
        {
            Loaded = loaded ?? throw new NullReferenceException("loaded をセットしてください。");
            Actions = actions ?? throw new NullReferenceException("actions をセットしてください。");

            MyFileIO = LocalFileIOSource.Create(LocalFilePath.Create(DirectoryPath));

            Ready();
        }


        /// <summary>
        /// 必要であればディレクトリやファイルの作成を行います．
        /// </summary>
        void Ready()
        {
            // ディレクトリの作成
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        /// <summary>
        /// ファイルパスを取得します。
        /// </summary>
        /// <param name="baseName">基本となる名前です。</param>
        /// <param name="extension">ファイルタイプです。 "ppg", "bppg" が入ります。 "*" を指定した場合は サポートする任意の拡張子となります。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">引数が null の場合。</exception>
        public Option<PathName> GetFilePath(string baseName)
        {
            baseName  = baseName  ?? throw new ArgumentNullException(nameof(baseName));
            //extension = extension ?? throw new ArgumentNullException(nameof(extension));

            var fileName = PpgFileName.From(baseName, "*");
            if(fileName.HasValue())
            {
                return Option.Return(() =>
                {
                    return PathName.FromPath(
                        Path.Combine(
                            DirectoryPath,
                            fileName.Value()
                        )
                    );
                });
            } else
            {
                return Option.Nothing<PathName>();
            }
        }

        /// <summary>
        /// 指定されたファイルを削除します．
        /// 引数の文字列は FileList から選ばれたものです．
        /// </summary>
        /// <param name="file">File.</param>
        public void Delete (PathName file)
        {
            File.Delete (file.Full);
        }

        /// <summary>
        /// 指定されたファイルが存在するときは true を返すメソッドです．
        /// 存在するときは true, そうでなければ false を返します．
        /// </summary>
        /// <param name="file">File.</param>
        public bool Exists (PathName file)
        {
            return File.Exists (file.Full);
        }

        /// <summary>
        /// 指定されたファイルの読み込みを行います．
        /// ppg ファイル、　 bppg ファイルの両方に対応しております。
        /// </summary>
        public bool Load (PathName filePath)
        {
            Option<FileOwner> file;
            try
            {
                file = MyFileIO.Load(filePath.Simple);
            } catch (Exception ex)
            {
                // TODO: エラー通知の入れ方を考える
                // TODO: 例外処理が必要
                Log.Error($"{ex}: {ex.Message}");
                Log.Error($"{ex.StackTrace}");
                file = Option.Nothing<FileOwner>();
            }
            if (file.HasValue()){
                Loaded = new FileContext(
                    file.Value(),
                    new FileSourceFromLocal(filePath)
                );
                Actions.FileReplaced(Loaded);
                return true;
            }else{
                return false;
            }
        }

        /// <summary>
        /// ファイル名を変更します．
        /// 引数の名前は Exists によって ファイルが存在しないことを確認され，
        /// かつ IsValid によって ファイル名として適切であることを証明されたものです．
        ///
        /// ファイルの拡張子は  <c>targetFile</c> のものに揃えられます。
        /// </summary>
        /// <param name="targetFile">Rename target filie.</param>
        /// <param name="nextName">New file name</param>
        public void Rename(PathName targetFile, PathName nextName)
        {
            var ext = targetFile
                        .ToLocalFilePath()
                        .GetMatchedExtension("bppg", "ppg")
                        .GetValueOrDefault();


            if(!ReferenceEquals(ext, null))
            {
                // 拡張子が見つかった場合
                PathName.FromPath(
                    nextName
                        .ToLocalFilePath()
                        .RemoveExtensions("bppg", "ppg")
                        .SetExtension(ext)
                        .ToString()
                );
            }

            // 移動
            File.Move(
                targetFile.Full,
                nextName.Full
            );
        }

        /// <summary>
        /// 新しいファイルに保存します．
        /// 引数の名前は Exists によって ファイルが存在しないことを確認され，
        /// かつ IsValid によって ファイル名として適切であることを証明されたものです．
        ///
        /// 新しく保存されるファイルは bppg ファイル形式となります。
        /// </summary>
        /// <param name="file">File.</param>
        public bool SaveAs (PathName file)
        {
            bool success;
            file = PathName.FromPath(
                file.ToLocalFilePath()
                    .RemoveExtensions("bppg", "ppg")
                    .SetExtension("bppg")
                    .ToString()
            );

            try
            {
                success = MyFileIO.SaveAs(
                    Loaded.Owner,
                    file.Simple
                );
            }
            catch (Exception ex)
            {
                // TODO: エラー通知の入れ方を考える
                // TODO: 例外処理が必要
                Log.Error($"{ex}: {ex.Message}");
                Log.Error($"{ex.StackTrace}");
                success = false;
            }

            if (success){
                Loaded = new FileContext(
                    Loaded.Owner,
                    new FileSourceFromLocal(file)
                );
                Actions.FileReplaced(Loaded);
                return true;
            }
            else{
                return false;
            }
        }

        /// <summary>
        /// 指定されたファイルに上書き保存します．
        /// 引数の文字列は FileList から選ばれたものです．
        ///
        /// 上書き保存後のファイルは  bppg 形式で保存されます。
        ///
        /// </summary>
        /// <param name="file">File.</param>
        public void SaveOver (PathName file)
        {
            var ext = file.ToLocalFilePath().GetMatchedExtension("bppg", "ppg").GetValueOrDefault() ?? "bppg";
            try
            {
                if(ext == "bppg")
                {
                    MyFileIO.SaveOver(Loaded.Owner, file.Simple);
                }
                else
                {
                    MyFileIO.SaveConvert(Loaded.Owner, file.Simple, "bppg", overwrite: true);
                }
            }
            catch (Exception ex)
            {
                // TODO: エラー通知の入れ方を考える
                // TODO: 例外処理が必要
                Log.Error($"{ex}: {ex.Message}");
                Log.Error($"{ex.StackTrace}");
            }
        }
    }
}

