using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Monad;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// PPG, BPPG の両方の読み込みに対応した FileIO です。
    /// 拡張子に　.bppg を指定するとバイナリ形式で保存されます。
    /// </summary>
    public class HybridLocalFileIO : ILocalFileIO
    {
        /// <summary>
        /// ピボットディレクトリです。
        /// </summary>
        public LocalFilePath PivotDirectory { get; }


        /// <summary>
        /// PPG, BPPG ファイルを検索してリストに入れます。
        /// </summary>
        public IList<LocalFilePath> FileNameList
        {
            get
            {
                var ppgDirectory = PivotDirectory.ToStringNormalized();

                if (File.Exists(ppgDirectory))
                    throw new LocalFileIOCouldNotAccessException("File Exists at PPG Directory Path.");

                if (!Directory.Exists(ppgDirectory))
                    return new List<LocalFilePath>();

                // PPG ファイル
                var textfiles = from file in Directory.GetFiles(ppgDirectory, "*.ppg")
                                select LocalFilePath.Create(file);

                // BPPG ファイル
                var binfiles = from file in Directory.GetFiles(ppgDirectory, "*.bppg")
                               select LocalFilePath.Create(file);

                return new List<IEnumerable<LocalFilePath>>() {
                    binfiles,
                    textfiles,
                }.SelectMany(it => it)
                 .ToList();
            }
        }


        readonly ILocalMBFileIODriver PPGDriver = new PPGFileIODriver();
        readonly ILocalMBFileIODriver BPPGDriver = new BinaryPPGFileIODriver();


        /// <summary>
        /// ファイル読み取りドライバを取得します。
        /// </summary>
        /// <param name="file">ファイル名</param>
        /// <returns>ファイルに対応するドライバ. 対応するドライバが見つからない場合は PPGDriver で読み取ります。</returns>
        ILocalMBFileIODriver GetDriverWith(LocalFilePath file)
        {
            if (file.HasExtension("bppg"))
                return BPPGDriver;
            return PPGDriver;
        }

        /// <summary>
        /// ピボットディレクトリを指定して初期化します。
        /// </summary>
        /// <param name="pivot">ppg ファイルを読み書きするファイルが配置されているディレクトリ</param>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        public HybridLocalFileIO(LocalFilePath pivot)
        {
            PivotDirectory = pivot ?? throw new ArgumentNullException();
        }

       

        public Option<FileOwner> Load(string name)
        {
            var path = PivotDirectory.AppendName(name);
            var driver = GetDriverWith(path);
            return driver.Load(path);
        }

        public Option<string> LoadToString(string name)
        {
            var path = PivotDirectory.AppendName(name);
            var driver = GetDriverWith(path);
            return driver.LoadToString(path);
        }

        public bool SaveAs(FileOwner owner, string name)
        {
            var path = PivotDirectory.AppendName(name);
            var driver = GetDriverWith(path);
            return driver.SaveAs(owner, path);
        }

        public bool SaveAsEmpty(string name)
        {
            var path = PivotDirectory.AppendName(name);
            var driver = GetDriverWith(path);
            return driver.SaveEmpty(path);
        }

        public void SaveOver(FileOwner owner, string name)
        {
            var path = PivotDirectory.AppendName(name);
            var driver = GetDriverWith(path);
            driver.SaveOver(owner, path);
        }

        public bool SaveConvert(FileOwner owner, string name, string ext, bool overwrite)
        {
            var oldPath = PivotDirectory.AppendName(name);
            if (oldPath.HasExtension(ext))
            {
                SaveOver(owner, name);
                return true;
            }
            var newPath = oldPath.RemoveExtensions("ppg", "bppg").SetExtension(ext);
            var driver  = GetDriverWith(newPath);

            // こちらは 上書きを許可する場合
            if(overwrite)
            {
                driver.SaveOver(owner, newPath);
                File.Delete(oldPath.ToStringNormalized());
                return true;
            }
            // 上書きを許可しない場合
            var success = driver.SaveAs(owner, newPath);
            if (!success)
                return false;
            File.Delete(oldPath.ToStringNormalized());
            return true;
        }

        public void LoadToConsole(string name)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(name);
            }
            catch (LocalFileIOCouldNotAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"ファイルへのアクセスが拒否されました: {ex.Message} ({ex})  :: {ex.StackTrace} ");
            }
            catch (ArgumentNullException ex)
            {
                System.Diagnostics.Debug.WriteLine($"(BUG) 引数が null です: {ex.Message} ({ex})  :: {ex.StackTrace} ");
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine($"引数が異常です。ファイル名に使用できない文字が含まれている可能性があります: {ex.Message} ({ex})  :: {ex.StackTrace} ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"想定外のエラー: {ex.Message} ({ex})  :: {ex.StackTrace} ");
            }
        }
    }
}

