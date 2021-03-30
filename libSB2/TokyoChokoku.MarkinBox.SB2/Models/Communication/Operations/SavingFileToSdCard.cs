using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
        public static async Task<bool> SaveFileToSdCard(int fileIndex, List<MBData> fields){

            // SDカードのデータ入出力番地をセット
            await SetSdCardDataWritingInfo (
                Addresses.SdCard.FileBlock.StartOfFileMap(fileIndex),
                Addresses.SdCardDataExchangeArea.FileMap.Address,
                Sizes.SdCard.FileBlock.BytesOfFileMap
            );

            await Task.Delay(50);

            // SDカードの内容を、一度 C[20000]~ に読み込む
            await LoadValueFromSdCard();

			await Task.Delay(50);

			// C[20000]~ にフィールド数を書き込む
			await SetNumOfFieldToSdCardDataExchangeArea(
                fileIndex,
                (short)fields.Count
            );

			await Task.Delay(50);

			// C[20000]~ の内容を、SDカードへ書き込む
			await SaveValueToSdCard();

			await Task.Delay(50);
		
            //
			// フィールドの書き込み
			//

			var bytesOfFields = fields.Count * Sizes.BytesOfField;

            // SDカードの内容を、一度 C[20512]~ に読み込む
            for (var i = 0; i < bytesOfFields; i += Sizes.SdCard.BytesOfTransferUnit) {
                await SetSdCardDataWritingInfo(
                    Addresses.SdCard.FileBlock.StartOfFile(fileIndex) + i,
                    Addresses.SdCardDataExchangeArea.File.Address + i,
                    Sizes.SdCard.BytesOfTransferUnit
                );
				await Task.Delay(50);
				await LoadValueFromSdCard();
				await Task.Delay(50);
			}

            // C[20512]~ にファイルを書き込む
            for (int i = 0; i < fields.Count; i++)
            {
                await SetFileToSdCardDataExchangeArea(i, fields[i]);
				await Task.Delay(50);
			}

            // C[20512]~ の内容を、SDカードへ書き込む
            for (var i = 0; i < bytesOfFields; i += Sizes.SdCard.BytesOfTransferUnit)
            {
                await SetSdCardDataWritingInfo(
                    Addresses.SdCard.FileBlock.StartOfFile(fileIndex) + i,
                    Addresses.SdCardDataExchangeArea.File.Address + i,
                    Sizes.SdCard.BytesOfTransferUnit
                );
				await Task.Delay(50);
				await SaveValueToSdCard();
				await Task.Delay(50);
			}

            return true;
		}
	}
}

