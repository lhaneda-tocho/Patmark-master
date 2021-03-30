/// @file IValidationResourceProvider.cs
/// @brief クラス定義ファイル
using System;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.ControllerIO;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// ISendValidator が必要とするリソースを提供します。
    /// </summary>
    public interface IValidationResourceProvider
    {
        /// <summary>
        /// 通信 Client です。
        /// </summary>
        CommunicationClient CommunicationClient { get; }

        /// <summary>
        /// コマンド実行用クラスです。
        /// </summary>
        CommandExecutor     CommandExecutor { get; }

        /// <summary>
        /// FileIO サービスです。
        /// </summary>
        FileIO              FileIO { get; }

        /// <summary>
        /// StatusIO サービスです。
        /// </summary>
        StatusIO            StatusIO { get; }

        /// <summary>
        /// アプリが保持する機種設定を返します。
        /// </summary>
        PatmarkMachineModel CurrentMachineModel { get; }

        /// <summary>
        /// フィールドが空の時に実行されるコールバックです。
        /// </summary>
        Action                      CallbackOnEmpty { get; }

        /// <summary>
        /// フィールドが保持する文字列に問題がある場合に実行されるコールバックです。
        /// </summary>
        Action<TextError>           CallbackOnTextError { get; }

        /// <summary>
        /// オフラインの時に実行されるコールバックです。
        /// </summary>
        Action                      CallbackOnOffline { get; }

        /// <summary>
        /// 接続先の物理デバイスと、アプリが保持する機種設定が一致しない時に実行されるコールバックです。
        /// </summary>
        Action                      CallbackOnMismatchMachineModel { get; }

        /// <summary>
        /// 文字サイズが大きすぎる時に実行されるコールバックです。
        /// </summary>
        Action<PatmarkMachineModel> CallbackOnTextSizeTooLarge { get; }

        /// <summary>
        /// 現在の機種設定が、接続中の機種と同じであれば true を返します。そうでなければ false となります。
        /// </summary>
        /// <returns>通信に成功し、 接続中の機種と、現在の設定が一致する場合は true, そうでなければ false</returns>
        Task<IUnstable<CommunicationResult<bool>>> CurrentMachineModelIsEqualsToRemote();
    }



}
