using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
	{
        /// <summary>
        /// SDカードへファイルの内容を送信します。
        /// </summary>
        /// <param name="fileIndex">ファイルインデックス</param>
        /// <param name="fields">フィールドリスト</param>
        /// <param name="sendFieldCount">フィールド数も一緒に送る場合は true, そうでなければ false です。 false を指定した場合は、 SetFileMapAndSaveToSdCard を使用して呼び出します。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">fileIndex が異常な場合 (ファイルリスト の外であるか、パーマネント打刻ファイルでない場合)</exception>
        public Task<bool> SaveFileToSdCard(int fileIndex, List<MBData> fields, bool sendFieldCount)
        {
            void Require(bool condition, Func<string> message)
            {
                if (!condition)
                    throw new ArgumentException(message?.Invoke() ?? "");
            }
            bool FileIndexIsInFileList()
            {
                return 0 <= fileIndex && fileIndex < Sizes.NumOfRemoteFile;
            }
            bool FileIndexIsPermanent()
            {
                return fileIndex == Sizes.IndexOfRemotePermanentFile;
            }

            async Task<bool> Block()
            {
                Require(FileIndexIsInFileList() || FileIndexIsPermanent(), () => $"{nameof(fileIndex)} (={fileIndex}) is out of range");

                {
                    var success = await LoadFileNameAndMapOnSdCardIfNeeded();
                    if (!success)
                        return false;
                }

                if (sendFieldCount)
                {
                    var success = await SetFileMapAndSaveToSdCard(fileIndex, fields.Count, null);
                    if (!success)
                        return false;
                }

                //
                // フィールドの書き込み
                //

                var bytesOfFields = fields.Count * Sizes.BytesOfField;

                // SDカードの内容を、一度 C[20512]~ に読み込む
                for (var i = 0; i < bytesOfFields; i += Sizes.SdCard.BytesOfTransferUnit)
                {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    var success = (await SetSdCardDataWritingInfo(
                        Addresses.SdCard.FileBlock.StartOfFile(fileIndex) + i,
                        Addresses.SdCardDataExchangeArea.File.Address + i,
                        Sizes.SdCard.BytesOfTransferUnit
                    )).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です

                    if (!success)
                        return false;
                    await Task.Delay(50);

#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    success = (await LoadValueFromSdCard()).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です

                    if (!success)
                        return false;
                    await Task.Delay(30);
                }

                // C[20512]~ にファイルを書き込む
                for (int i = 0; i < fields.Count; i++)
                {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    var success = (await SetFileToSdCardDataExchangeArea(i, fields[i])).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                    if (!success)
                        return false;
                    await Task.Delay(50);
                }

                // C[20512]~ の内容を、SDカードへ書き込む
                for (var i = 0; i < bytesOfFields; i += Sizes.SdCard.BytesOfTransferUnit)
                {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    var success = (await SetSdCardDataWritingInfo(
                        Addresses.SdCard.FileBlock.StartOfFile(fileIndex) + i,
                        Addresses.SdCardDataExchangeArea.File.Address + i,
                        Sizes.SdCard.BytesOfTransferUnit
                    )).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                    if (!success)
                        return false;
                    await Task.Delay(50);
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    success = (await SaveValueToSdCard()).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                    if (!success)
                        return false;
                    await Task.Delay(50);
                }

                return true;
            }

            return Block();
		}

	}
}

