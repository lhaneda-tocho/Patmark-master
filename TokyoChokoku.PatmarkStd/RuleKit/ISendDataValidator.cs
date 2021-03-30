/// @file ISendDataValidator.cs
/// @brief クラス定義ファイル
///
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 送信データバリデータのインタフェースです。
    /// </summary>
    public interface ISendDataValidator
    {
        /// <summary>
        /// QuickMode の送信前妥当性チェックを行います。
        /// </summary>
        /// <param name="data">検証したいエンティティ</param>
        /// <returns>妥当性チェック結果</returns>
        Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data);

        /// <summary>
        /// PCFileMode の送信前妥当性チェックを行います。
        /// </summary>
        /// <param name="data">検証したいエンティティ</param>
        /// <returns>妥当性チェック結果</returns>
        Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data);
    }
}
