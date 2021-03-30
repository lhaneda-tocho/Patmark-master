using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
    {
        [Obsolete("これは Communication プロジェクトの内部 API です。アプリケーションロジックでは使用しないでください。", false)]
        private async Task<bool> UnsafeWriteMap(int fileIndex, int numOfFields)
        {
            // SDカードのデータ入出力番地をセット
            var success = (await SetSdCardDataWritingInfo(
                Addresses.SdCard.FileBlock.StartOfFileMap(fileIndex),
                Addresses.SdCardDataExchangeArea.FileMap.Address,
                Sizes.SdCard.FileBlock.BytesOfFileMap
            )).IsOk;

            await Task.Delay(10);
            if (!success)
                return false;

            // SDカードの内容を、一度 C[20000]~ に読み込む
            success = (await LoadValueFromSdCard()).IsOk;

            await Task.Delay(10);
            if (!success)
                return false;

            // C[20000]~ にフィールド数を書き込む
            success = (await SetNumOfFieldToSdCardDataExchangeArea(
                fileIndex,
                (short)numOfFields
            )).IsOk;

            await Task.Delay(10);
            if (!success)
                return false;

            // C[20000]~ の内容を、SDカードへ書き込む
            success = (await SaveValueToSdCard()).IsOk;

            await Task.Delay(10);
            if (!success)
                return false;

            return true;
        }

        /// <summary>
        /// ファイル名とファイルマップを指定して保存します。この実行でSDカードへ保存されます。
        /// 処理に失敗した場合のコントローラの状態は不定となります。
        /// </summary>
        /// <param name="fileIndex">ファイルインデックス</param>
        /// <param name="numOfFields">フィールド数</param>
        /// <param name="onChangeFileList"><c>fileIndex</c>で指定されたファイルが、ファイル一覧に表示されるファイルである場合に呼ばれるアクションです。(引数1: fileIndex, 引数2: numOfFields)</param>
        /// <returns>成功したら true, 失敗したら false</returns>
        /// <exception cref="ArgumentException"><c>fileIndex</c>が定義域外である場合 (ファイルリストの外であり、かつ、パーマネント打刻ファイルでない場合)</exception>
        public Task<bool> SetFileMapAndSaveToSdCard(int fileIndex, int numOfFields, Action<int, int> onChangeFileListOrNull)
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
            async Task<bool> SetFileMap()
            {
                if (FileIndexIsInFileList())
                {
                    onChangeFileListOrNull?.Invoke(fileIndex, numOfFields);
                }
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                var success = (await this.SetRemoteFileMap(fileIndex, (short)numOfFields)).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                await Task.Delay(50);
                return success;
            }
            async Task<bool> MapToSdCard() {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                return await UnsafeWriteMap(fileIndex, numOfFields);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
            }
            async Task<bool> Block()
            {
                Require(FileIndexIsInFileList() || FileIndexIsPermanent(), () => $"{nameof(fileIndex)} (={fileIndex}) is out of range");

                var success = await LoadFileNameAndMapOnSdCardIfNeeded();
                if (!success)
                    return false;

                // ファイル内のフィールド数の送信
                success = await SetFileMap();
                if (!success)
                    return false;
                success = await MapToSdCard();
                if (!success)
                    return false;
                return true;
            }
            return Block();
        }
        //public async Task<bool> SaveFileMapToSdCardAt(int fileIndex, int numOfField)
        //{
        //    //// R[1000]~
        //    //await SetRemoteFileMap(fileIndex, (short)numOfField);

        //    //// C[20000]~
        //    await SetSdCardDataWritingInfo(
        //        Addresses.SdCard.FileMapBlock.AddressWithMapIndex(fileIndex),
        //        Addresses.SdCardDataExchangeArea.FileMap.Address,
        //        Sizes.SdCard.BytesOfTransferUnit
        //    );
        //    await LoadValueFromSdCard();
        //    await SetFileMapToWorkSpace(fileIndex, (byte)numOfField);
        //    await SaveValueToSdCard();

        //    return true;
        //}
    }
}

