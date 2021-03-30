using System;
namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// LocalFileIO インターフェースを提供するサービスプロバイダクラスです。
    /// </summary>
    public static class LocalFileIOSource
    {
        private static LocalFilePath DefaultPath
        {
            get
            {
                return LocalFilePath.Create(LocalFilePathGeneratorPublisher.Instance.DirectorySave());
            }
        }

        /// <summary>
        /// 標準のPPG ディレクトリを指定して LocalFileIO オブジェクトを作成します。
        /// </summary>
        /// <returns>LocalFileIO オブジェクト</returns>
        public static ILocalFileIO CreateDefault()
        {
            return Create(DefaultPath);
        }

        /// <summary>
        /// LocalFileIO オブジェクトを作成します。
        /// </summary>
        /// <param name="pivot">PPGファイルが保存されているフォルダへのパス</param>
        /// <returns>LocalFileIO オブジェクト</returns>
        public static ILocalFileIO Create(LocalFilePath pivot)
        {
            return new HybridLocalFileIO(pivot ?? throw new ArgumentNullException(nameof(pivot)));
        }
    }
}
