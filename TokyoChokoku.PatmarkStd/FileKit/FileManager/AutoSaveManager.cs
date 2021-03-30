using System;
using System.Linq;
using System.IO;
using Monad;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark
{
    public static class AutoSaveManager
    {
        /// <summary>
        /// 旧形式のファイル名
        /// </summary>
        /// <value>The file path.</value>
        public static string OldPPGFileName { get; } = "autosave.ppg";

        public static LocalFilePath OldPPGFilePath { get; }

        /// <summary>
        /// BPPG ファイルの名前
        /// </summary>
        /// <value>The file path.</value>
        public static string FileName { get; } = "autosave.bppg";

        public static LocalFilePath FilePath { get; }

        /// <summary>
        /// コントローラ番号が保存されているファイルへの絶対パス
        /// </summary>
        /// <value>The controller number path.</value>
        public static LocalFilePath ControllerNumberPath { get; }

        /// <summary>
        /// ファイル名が保存されているファイルへの絶対パス
        /// </summary>
        /// <value>The controller number path.</value>
        public static LocalFilePath FileNamePath { get; }

        public static ILocalFileIO MyFileIO { get; }

        /// <summary>
        /// 各種ファイルが保存されるディレクトリ
        /// </summary>
        /// <value>The directory path.</value>
        static LocalFilePath DirectoryPath
        {
            get
            {
                return LocalFilePath.Create(LocalFilePathGeneratorPublisher.Instance.DirectoryAutoSave());
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static AutoSaveManager()
        {
            MyFileIO             = LocalFileIOSource.Create(DirectoryPath);
            FileNamePath         = DirectoryPath.AppendName("autosave_file_name.txt");
            ControllerNumberPath = DirectoryPath.AppendName("controller_no.txt");
            OldPPGFilePath = DirectoryPath.AppendName(OldPPGFileName);
            FilePath       = DirectoryPath.AppendName(FileName);
        }

        /// <summary>
        /// 必要であればディレクトリやファイルの作成を行います．
        /// </summary>
        public static void Ready()
        {
            // ディレクトリの作成
            if (!Directory.Exists(DirectoryPath.ToStringNormalized()))
                Directory.CreateDirectory(DirectoryPath.ToStringNormalized());
            try
            {
                // 旧形式から変換する処理
                if (File.Exists(OldPPGFilePath.ToString()))
                {
                    var old = MyFileIO.Load(OldPPGFileName);
                    if(old.HasValue())
                    {
                        if (MyFileIO.SaveConvert(old.Value(), OldPPGFileName, ".bppg"))
                            Log.Info($"自動保存ファイルを変換しました: {OldPPGFileName}");
                        else
                        {
                            Log.Warn($"bppg ファイルと ppg ファイルが両方存在するため、ppg ファイルを破棄します.");
                            File.Delete(OldPPGFilePath.ToString());
                        }
                    } else
                    {
                        // ファイルがない場合の分岐。気にしなくていい。
                        Log.Error($"ファイルが見つかりませんでした: {OldPPGFileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: ステータス通知を入れるべき
                Log.Error($"{ex}: {ex.Message}");
                // この処理は失敗しても良いものとする。
            }

            try
            {
                // BPPG ファイルの作成
                MyFileIO.SaveAsEmpty(FileName);
            }
            catch (Exception ex)
            {
                // TODO: ステータス通知を入れるべき
                Log.Error($"{ex}: {ex.Message}");
                Log.Error($"{ex.StackTrace}");
                throw ex;
            }

            // コントローラ番号ファイルの作成
            if (!File.Exists(ControllerNumberPath.ToString()))
                File.WriteAllText(ControllerNumberPath.ToString(), "");

            // ファイル名を保存するファイルの作成
            if (!File.Exists(FileNamePath.ToStringNormalized()))
                File.WriteAllText(FileNamePath.ToString(), "");
        }

        /// <summary>
        /// 自動セーブが存在するとき true を返します，それ以外は false です．
        /// </summary>
        /// <value><c>true</c> if file exists; otherwise, <c>false</c>.</value>
        public static bool FileExists
        {
            get
            {
                return File.Exists(DirectoryPath.AppendName(FileName).ToStringNormalized());
            }
        }

        /// <summary>
        /// FieldSourceオブジェクトを返します．
        /// プレビュー画面で ファイル名を表示するために使います．
        /// </summary>
        /// <value>The field source.</value>
        public static IFileSource FileSource
        {
            get
            {
                var number   = LoadControllerNumber();
                var fileName = LoadFileName();
                if (number.HasValue())
                {
                    // コントローラのファイルである場合
                    string fileNameOrEmpty = (fileName.HasValue()) ? fileName.Value() : "";
                    return new FileSourceFromRemote(number.Value(), fileNameOrEmpty);
                }
                // コントローラのファイル以外
                return new FileSourceFromOther();
            }
        }

        /// <summary>
        /// 自動セーブデータを読み込みます．
        /// </summary>
        /// <returns>The ppg as list.</returns>
        public static Option<FileContext> LoadFileAutoSaved()
        {
            Console.WriteLine("In Load File");
            Ready();
            Console.WriteLine("Ready fin");

            try
            {
                //
                var file = MyFileIO.Load(FileName);
                if (file.HasValue())
                {
                    Console.WriteLine("Read auto saved  (bppg)");
                    return Option.Return<FileContext>(() => new FileContext(file.Value(), FileSource));
                }
            }
            catch (Exception ex)
            {
                // TODO: ステータス通知を入れるべき
                Log.Error($"{ex}");
            }
            Console.WriteLine("Read none auto saved");
            return Option.Nothing<FileContext>();
        }

        /// <summary>
        /// 指定された MBData で自動保存ファイルを上書き保存します．
        /// </summary>
        public static void Overwrite(FileOwner owner)
        {
            try { 
                MyFileIO.SaveOver(owner, FileName);
            }
            catch (Exception ex)
            {
                // TODO: ステータス通知を入れるべき
                Log.Error($"{ex}: {ex.Message}");
                Log.Error($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// コントローラ番号を保存します．
        /// 番号を記録しない場合は Maybe.Nothing を指定します．
        /// </summary>
        /// <param name="maybeNumber">Number.</param>
        public static void SaveControllerNumber(Option<int> maybeNumber)
        {
            if (maybeNumber.HasValue())
                File.WriteAllText(ControllerNumberPath.ToString(), maybeNumber.Value().ToString(), System.Text.Encoding.UTF8);
            else
                File.WriteAllText(ControllerNumberPath.ToString(), "", System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// コントローラの番号を読み取ります．
        /// 番号が無い場合は Maybe.Nothing を返します．
        /// </summary>
        /// <returns>The controller number.</returns>
        public static Option<int> LoadControllerNumber()
        {
            string data = "";
            // 読み込み
            try
            {
                data = File.ReadAllText(ControllerNumberPath.ToString(), System.Text.Encoding.UTF8);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // int に変換
            int value = 0;
            if (int.TryParse(data, out value))
                // 成功 -> 値を返す
                return Option.Return(() => value);
            else
                // 失敗 -> 空
                return Option.Nothing<int>();
        }

        /// <summary>
        /// 最後に読み取ったファイルの名前を保存するためのメソッドです．
        /// Maybe.Nothing or 空の文字列のときは 名無し として扱われます．
        /// </summary>
        /// <param name="">.</param>
        public static void SaveFileName(Option<string> maybeFileName)
        {
            File.WriteAllText(
                FileNamePath.ToString(),
                maybeFileName.HasValue() ? maybeFileName.Value() : "",
                System.Text.Encoding.UTF8
            );
        }

        /// <summary>
        /// 最後に読み取ったファイルの名前を読み取ります．
        /// 名無しの場合は Maybe.Nothing が返ります．
        /// 返り値の文字列は 必ず 1つ以上の要素を持ちます．
        /// </summary>
        /// <returns>The file name.</returns>
        public static Option<string> LoadFileName()
        {
            try
            {
                var name = File.ReadAllText(FileNamePath.ToString(), System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(name))
                {
                    return Option.Return(() => name);
                }
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Option.Nothing<string>();
        }
    }
}
