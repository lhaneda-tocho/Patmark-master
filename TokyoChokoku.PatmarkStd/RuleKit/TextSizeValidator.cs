/// @file TextSizeValidator.cs
/// @brief クラス定義ファイル
using System;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.EmbossmentKit;
using System.Threading.Tasks;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 文字サイズの検証を行うクラスです。
    /// </summary>
    public class TextSizeValidator : ISendDataValidator
    {
        /// <summary>
        /// バリデータをインスタンス化します。
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static TextSizeValidator Create(IValidationResourceProvider provider)
        {
            return new TextSizeValidator(provider);
        }

        /// <summary>
        /// リソースへアクセスするために必要なオブジェクト
        /// </summary>
        IValidationResourceProvider ResourceProvider { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider"></param>
        public TextSizeValidator(
            IValidationResourceProvider provider
        )
        {
            ResourceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }


        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data)
        {
            var model = CurrentMachineModel();
            var textsize = CurrentTextSize(data);
            var supportTextSizeMax = model.Profile.MaxTextSize();
            if (textsize > supportTextSizeMax)
            {
                // サポートする最大値を超える場合はエラーとする
                var message = $"text size too large: {textsize}";
                OnTextSizeTooLarge(model);
                return MarkingResult.Failure(message).ToReturnable();
            }

            return MarkingResult.Success().ToReturnable();
        }


        /// <inheritdoc/>
        public Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data)
        {
            return Task.FromResult(MarkingResult.Success().ToReturnable());
        }



        // ↓ Resource Query ↓

        PatmarkMachineModel CurrentMachineModel()
            => ResourceProvider.CurrentMachineModel ?? throw new InvalidOperationException(
                $"CurrentMachineModel cannot be retrieved."
            );

        decimal CurrentTextSize(QuickModeData data)
            => data.Data.TextSizeLevel.ToMaterialized().ToDecimal();

        void OnTextSizeTooLarge(PatmarkMachineModel arg)
        {
            ResourceProvider.CallbackOnTextSizeTooLarge(arg);
        }
    }
}
