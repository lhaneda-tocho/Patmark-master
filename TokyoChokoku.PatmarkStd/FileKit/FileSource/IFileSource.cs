using System;
namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// ファイルの出処を示します。
    /// </summary>
    public interface IFileSource
    {
        string From { get; }

        void Autosave(FileOwner owner);

        IFileSource Clone { get; }
    }
}
