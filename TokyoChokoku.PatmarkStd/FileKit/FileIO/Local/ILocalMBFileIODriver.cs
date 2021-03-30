using Monad;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// ローカルファイルの読み込みドライバです。
    /// </summary>
    public interface ILocalMBFileIODriver
    {
        /// <summary>
        /// 空のファイルを保存します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        bool SaveEmpty(LocalFilePath path);

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        bool SaveAs(FileOwner owner, LocalFilePath path);

        /// <summary>
        /// ファイルを上書き保存します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="path">ファイルパス</param>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        void SaveOver(FileOwner owner, LocalFilePath path);

        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>読み込み結果. ファイルが存在しない場合は 空です。</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        Option<FileOwner> Load(LocalFilePath path);

        /// <summary>
        /// ファイルの内容を文字列で返します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>読み込み結果</returns>
        /// <exception cref="LocalFileIOCouldNotAccessException">
        ///     パスへ指定された場所へ操作・アクセスできない場合。
        /// </exception>
        Option<string> LoadToString(LocalFilePath path);
    }

}

