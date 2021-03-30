/// @file CharactorValidator.cs
/// @brief クラス定義ファイル
using System;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.ControllerIO;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 使用可能な文字で構成されているか検証を行うクラスです。
    /// </summary>
    public class CharactorValidator : ISendDataValidator
    {
        /// <summary>
        /// バリデータをインスタンス化します。
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        /// <returns>このクラスのインスタンス</returns>
        public static CharactorValidator Create(IValidationResourceProvider provider)
        {
            return new CharactorValidator(provider);
        }

        /// <summary>
        /// リソースへアクセスするために必要なオブジェクト
        /// </summary>
        IValidationResourceProvider ResourceProvider { get; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        public CharactorValidator(
            IValidationResourceProvider provider
        )
        {
            ResourceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data)
        {
            var res = await CheckText(data);
            if (res.HasError)
            {
                var message = "Found unsupported text";
                OnTextError(res);
                return MarkingResult.Failure(message).ToReturnable();
            }
            return MarkingResult.Success().ToReturnable();
        }

        /// <inheritdoc/>
        public Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data)
        {
            return Task.FromResult(MarkingResult.Success().ToReturnable());
        }


        Task<TextError> CheckText(QuickModeData data)
        {
            return StatusIO.ProtectOnMarking(async () =>
            {
                var ins = new TextValidator();
                var res = await ins.Validate(GetText(data));
                if (res.IsSuccess)
                {
                    return TextError.Success();
                }
                else
                {
                    return TextError.Error(res.ErrorChar, res.Index);
                }
            });
        }

        // ↓ Resource Query ↓
        StatusIO StatusIO => ResourceProvider.StatusIO;

        string GetText(QuickModeData data)
        {
            return data.Data.Text.Text;
        }

        void OnTextError(TextError arg)
        {
            ResourceProvider.CallbackOnTextError(arg);
        }

    }
}
