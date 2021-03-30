namespace TokyoChokoku.Bppg
{
    /// <summary>
    /// BinaryPPG ファイルIOです。
    /// </summary>
    public interface BPPGFileIO
    {
        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BPPGFile ReadFile(string path);


        /// <summary>
        /// ファイルを書き込みます。
        /// </summary>
        /// <param name="file"></param>
        public void WriteFile(BPPGFile file);
    }
}
