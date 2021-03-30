using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class AutoSaveManager
    {
        /// <summary>
        /// PPG ファイルへの絶対パス
        /// </summary>
        /// <value>The file path.</value>
        public static string FilePath { get; }

        /// <summary>
        /// コントローラ番号が保存されているファイルへの絶対パス
        /// </summary>
        /// <value>The controller number path.</value>
        public static string ControllerNumberPath { get; }

        /// <summary>
        /// ファイル名が保存されているファイルへの絶対パス
        /// </summary>
        /// <value>The controller number path.</value>
        public static string FileNamePath { get; }

        /// <summary>
        /// 各種ファイルが保存されるディレクトリ
        /// </summary>
        /// <value>The directory path.</value>
        static string DirectoryPath {
            get {
                return PpgPath.AutoSaveDirectory;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static AutoSaveManager ()
        {
            FilePath = Path.Combine (DirectoryPath, "autosave.ppg");
            FileNamePath = Path.Combine (DirectoryPath, "autosave_file_name.txt");
            ControllerNumberPath = Path.Combine (DirectoryPath, "controller_no.txt");
        }

        /// <summary>
        /// 必要であればディレクトリやファイルの作成を行います．
        /// </summary>
        public static void Ready ()
        {
            string round = "INVALID";
            try
            {
                round = "ディレクトリの作成";
                if (!Directory.Exists(DirectoryPath))
                    Directory.CreateDirectory(DirectoryPath);

                round = "PPG ファイルの作成";
                if (!File.Exists(FilePath))
                    LocalFileManager.CreateNew(FilePath);

                round = "コントローラ番号ファイルの作成";
                if (!File.Exists(ControllerNumberPath))
                    File.WriteAllText(ControllerNumberPath, "");

                round = "ファイル名を保存するファイルの作成";
                if (!File.Exists(FileNamePath))
                    File.WriteAllText(FileNamePath, "");
            } catch (Exception ex)
            {
                Log.Error($"ファイル保存準備中にエラー発生 (致命的): Round: {round}: {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// 自動セーブが存在するとき true を返します，それ以外は false です．
        /// </summary>
        /// <value><c>true</c> if file exists; otherwise, <c>false</c>.</value>
        public static bool FileExists {
            get {
                try
                {
                    return File.Exists(FilePath);
                } catch (Exception ex)
                {
                    Log.Error($"ファイル存在確認中にエラー発生: {ex}");
                    return false;
                }
            }
        }

        /// <summary>
        /// FieldSourceオブジェクトを返します．
        /// プレビュー画面で ファイル名を表示するために使います．
        /// </summary>
        /// <value>The field source.</value>
        public static FieldSource FieldSource {
            get {
                var number   = LoadControllerNumber ();
                var fileName = LoadFileName ();
                if (number.HasValue) {
                    // コントローラのファイルである場合
                    string fileNameOrEmpty = (fileName.HasValue) ? fileName.Value : "";
                    return new AutoSaveFieldSourceFromRemoteFile (number.Value, fileNameOrEmpty);
                }
                // コントローラのファイル以外
                return new AutoSaveFieldSource ();
            }
        }

        /// <summary>
        /// 自動セーブデータを読み込みます．
        /// </summary>
        /// <returns>The ppg as list.</returns>
        public static Maybe<IList<iOSOwner>> LoadPpgAsList ()
        {
            try
            {
                Ready();
                return LocalFileManager.LoadPpgAsList (FilePath).ToMaybe<IList<iOSOwner>> ();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine (ex.Message);
            }
            return Maybe<IList<iOSOwner>>.Nothing;
        }

        /// <summary>
        /// 指定された MBData で自動保存ファイルを上書き保存します．
        /// </summary>
        /// <param name="list">List.</param>
        static void Overwrite (List<MBData> list)
        {
            using (TextWriter writer = File.CreateText (FilePath)) {
                MBDataTextizer.Save (writer, list.ToArray ());
            }
        }

        public static void Overwrite (iOSFieldContext context)
        {
            Overwrite(context.Serializable.ToList ());
        }

        /// <summary>
        /// コントローラ番号を保存します．
        /// 番号を記録しない場合は Maybe.Nothing を指定します．
        /// </summary>
        /// <param name="maybeNumber">Number.</param>
        public static void SaveControllerNumber (Maybe<int> maybeNumber)
        {
            if (maybeNumber.HasValue)
                File.WriteAllText (ControllerNumberPath, maybeNumber.Value.ToString (), System.Text.Encoding.UTF8);
            else
                File.WriteAllText ( ControllerNumberPath,  "", System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// コントローラの番号を読み取ります．
        /// 番号が無い場合は Maybe.Nothing を返します．
        /// </summary>
        /// <returns>The controller number.</returns>
        public static Maybe<int> LoadControllerNumber ()
        {
            string data = "";
            // 読み込み
            try {
                data = File.ReadAllText (ControllerNumberPath, System.Text.Encoding.UTF8);
            } catch (IOException ex) {
                System.Diagnostics.Debug.WriteLine (ex.Message);
            }

            // int に変換
            int value = 0;
            if (int.TryParse (data, out value))
                // 成功 -> 値を返す
                return value.ToMaybe ();
            else
                // 失敗 -> 空
                return Maybe<int>.Nothing;
        }

        /// <summary>
        /// 最後に読み取ったファイルの名前を保存するためのメソッドです．
        /// Maybe.Nothing or 空の文字列のときは 名無し として扱われます．
        /// </summary>
        /// <param name="">.</param>
        public static void SaveFileName (Maybe<string> maybeFileName)
        {
            maybeFileName.Do(it =>
            {
                File.WriteAllText(FileNamePath, it, System.Text.Encoding.UTF8);
            });
        }

        /// <summary>
        /// 最後に読み取ったファイルの名前を読み取ります．
        /// 名無しの場合は Maybe.Nothing が返ります．
        /// 返り値の文字列は 必ず 1つ以上の要素を持ちます．
        /// </summary>
        /// <returns>The file name.</returns>
        public static Maybe<string> LoadFileName ()
        {
            try {
                var name = File.ReadAllText (FileNamePath, System.Text.Encoding.UTF8);
                if (string.IsNullOrEmpty (name))
                    return Maybe<string>.Nothing;
                return name.ToMaybe ();
            } catch (IOException ex) {
                System.Diagnostics.Debug.WriteLine (ex.Message);
                return Maybe<string>.Nothing;
            }
        }
    }
}
