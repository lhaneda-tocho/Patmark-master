/// @file OnlineValidator.cs
/// @brief クラス定義ファイル
using System;
using TokyoChokoku.Patmark.EmbossmentKit;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Communication;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// オンラインであるか確認するばりデータです。
    /// </summary>
    public class OnlineValidator : ISendDataValidator
    {
        /// <summary>
        /// バリデータをインスタンス化します。
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        /// <returns>このクラスのインスタンス</returns>
        public static OnlineValidator Create(IValidationResourceProvider provider)
        {
            return new OnlineValidator(provider);
        }

        /// <summary>
        /// リソースへアクセスするために必要なオブジェクト
        /// </summary>
        IValidationResourceProvider ResourceProvider { get; }
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        public OnlineValidator(
            IValidationResourceProvider provider
        )
        {
            ResourceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data)
        {
            return await Validate();
        }

        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data)
        {
            return await Validate();
        }

        async Task<IUnstable<MarkingResult>> Validate()
        {
            var client = CommunicationClient;
            if (client == null || !CommunicationClient.Ready)
            {
                var message = "Client not initialized";
                OnOffline();
                return MarkingResult.Failure(message).ToReturnable();
            }

            return MarkingResult.Success().ToReturnable();
        }

        // ↓ Resource Query ↓
        CommunicationClient CommunicationClient
            => ResourceProvider.CommunicationClient;

        void OnOffline()
        {
            ResourceProvider.CallbackOnOffline();
        }
    }
}