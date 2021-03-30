/// @file NotEmptyValidator.cs
/// @brief クラス定義ファイル
using System;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.Common;
using System.Threading.Tasks;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 打刻可能なフィールドが存在することを確認するバリデータです。
    /// </summary>
    public class NotEmptyValidator : ISendDataValidator
    {
        /// <summary>
        /// バリデータをインスタンス化します。
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        /// <returns>このクラスのインスタンス</returns>
        public static NotEmptyValidator Create(IValidationResourceProvider provider)
        {
            return new NotEmptyValidator(provider);
        }

        /// <summary>
        /// リソースへアクセスするために必要なオブジェクト
        /// </summary>
        IValidationResourceProvider ResourceProvider { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">リソースプロバイダ</param>
        public NotEmptyValidator(
            IValidationResourceProvider provider
        )
        {
            ResourceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidateQuickMode(QuickModeData data)
        {
            var s = data.Serialized;
            if (s.Count == 0 || data.Data.IsEmpty)
            {
                var message = "The field list is empty. This request is ignored.";
                OnEmpty();
                return MarkingResult.Failure(message).ToReturnable();
            }

            return MarkingResult.Success().ToReturnable();
        }

        /// <inheritdoc/>
        public async Task<IUnstable<MarkingResult>> ValidatePCFileMode(PCFileModeData data)
        {
            var strict = data.StrictData;
            if(strict.Count == 0)
            {
                var message = "The field list is empty. This request is ignored";
                OnEmpty();
                return MarkingResult.Failure(message).ToReturnable();
            }

            return MarkingResult.Success().ToReturnable();
        }


        // ↓ Resource Query ↓
        void OnEmpty()
        {
            ResourceProvider.CallbackOnEmpty();
        }
    }
}