using System;
using System.Collections.Generic;
using Monad;

namespace TokyoChokoku.Patmark
{

    public interface ILocalFileIO
    {
        /// <summary>
        /// ファイルを保存するディレクトリ
        /// </summary>
        LocalFilePath PivotDirectory  { get; }

        /// <summary>
        /// <c>PivotDirectory</c> に配置されているファイルの一覧を取得します。
        /// </summary>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        IList<LocalFilePath> FileNameList    { get; }

        /// <summary>
        /// 空のファイルを保存します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>ファイルが存在しなければ保存を行い, true を返します。 ファイルがあった場合は何もせず false を返します。</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        bool SaveAsEmpty(string name);

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name">ファイル名</param>
        /// <returns>ファイルが存在しなければ保存を行い, true を返します。 ファイルがあった場合は何もせず false を返します。</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        bool SaveAs   (FileOwner owner, string name);

        /// <summary>
        /// ファイルを上書き保存します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name">ファイル名</param>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        void SaveOver (FileOwner owner, string name);

        /// <summary>
        /// ファイル形式を変換し、上書き保存します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name">ファイル名</param>
        /// <param name="ext">新しい拡張子</param>
        /// <param name="overwrite">同名の bppg ファイルがある場合に上書きするかどうか.</param>
        /// <returns>ファイルが存在しなければ保存を行い, true を返します。 ファイルがあった場合は何もせず false を返します。</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        bool SaveConvert (FileOwner owner, string name, string ext, bool overwrite=false);

        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>読み込み結果</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        Option<FileOwner> Load(string name);

        /// <summary>
        /// ファイルの内容を文字列で返します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///      引数が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///      引数のファイル名が、ファイル名として利用できない場合。
        /// </exception>
        Option<string> LoadToString(string name);

        /// <summary>
        /// ファイルの内容をコンソールに出力します、
        /// </summary>
        /// <param name="name">ファイル名</param>
        void LoadToConsole(string name);
    }
}

