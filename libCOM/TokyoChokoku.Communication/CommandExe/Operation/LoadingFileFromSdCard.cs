using System;
using System.Threading.Tasks;

namespace TokyoChokoku.Communication
{
	partial class CommandExecutor
	{
        // 指定したファイルを、SDカードからカレントファイル領域R[10000]~ に読み出します。

        public async Task<bool> LoadFileFromSdCard(int fileIndex){

            // 読み込むファイルの番号を指定します。
            {
                await SetRemoteMarkingFileNo((short)(fileIndex + 1));
            }

            // 読み込むファイルのフィールド数を取得します。
            var numOfField = 0;
            {
                var res = await ReadRemoteFileMap(fileIndex);
                if (res.IsOk)
                {
                    numOfField = (int)res.Value;
                }
            }
            // 異常な値が入ってくることがあるため、制限します。
            numOfField = (numOfField > Sizes.MaxNumOfFieldInFile) ? 0 : numOfField;
            Log.Info(String.Format("[LoadingFileFromSdCard] フィールド数：{0}", numOfField.ToString()));

            // フィールド数のぶんだけ読み出す
            for (int fieldIndex = 0; fieldIndex < numOfField; fieldIndex++)
            {
                {
                    await SetFieldIndexOfRemoteSdCardFile((short)fieldIndex);
                    await LoadFieldFromSdCard();
                }
            }

            // フィールド数をCurrent領域にセット
            await SetNumOfFieldOfCurrentMarkingFile((short)numOfField);

            // 本体ボタンでマーキングをスタートしないようにする
            await SetMarkingMode(MBMarkingMode.Pc);

            return true;
		}
	}
}

