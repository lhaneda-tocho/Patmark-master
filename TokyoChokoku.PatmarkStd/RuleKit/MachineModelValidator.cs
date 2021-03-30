/// @file MachineModelValidator.cs
/// @brief クラス定義ファイル
using System;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.Settings;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 機種設定の検証を行うバリデータです。
    /// </summary>
    public class MachineModelValidator : ISendDataValidator
    {
        /// <summary>
        /// バリデータをインスタンス化します。
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        /// <returns>このクラスのインスタンス</returns>
        public static MachineModelValidator Create(IValidationResourceProvider provider)
        {
            return new MachineModelValidator(provider);
        }

        /// <summary>
        /// リソースへアクセスするために必要なオブジェクト
        /// </summary>
        IValidationResourceProvider ResourceProvider { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        public MachineModelValidator(
            IValidationResourceProvider provider
        )
        {
            ResourceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }


        /// <inheritdoc/>
        public Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data)
        {
            return Validate();
        }


        /// <inheritdoc/>
        public Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data)
        {
            return Validate();
        }


        async Task<IUnstable<MarkingResult>> Validate()
        {
            // 機種設定
            var result = await ValidControllerModel();
            var markingResult = result.Aggregate<CommunicationResult<bool>, IUnstable<MarkingResult>>(
                onFailure: (result) =>
                {
                    // 通信に失敗した時
                    return MarkingResult.Failure(result.Message).ToReturnable();
                },
                onSuccess: (result) =>
                {
                    // 通信に成功した時
                    if (!result.Value)
                    {
                        OnMismatchMachineModel();
                        return MarkingResult.Failure("Machine model mismatched").ToReturnable();
                    }
                    return MarkingResult.Success().ToReturnable();
                }
            );
            return markingResult;
        }



        async Task<IUnstable<CommunicationResult<bool>>> ValidControllerModel()
        {
            return await ResourceProvider
                .CurrentMachineModelIsEqualsToRemote();
        }


        // ↓ Resource Query ↓
        void OnMismatchMachineModel()
        {
            ResourceProvider.CallbackOnMismatchMachineModel();
        }
    }
}