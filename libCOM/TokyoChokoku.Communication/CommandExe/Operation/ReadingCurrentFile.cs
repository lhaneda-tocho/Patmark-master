using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.Structure;
using TokyoChokoku.MarkinBox.Sketchbook;
namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
	{
        public async Task<List<MBData>> ReadCurrentFile(){
			var result = new List<MBData> ();
			var resNumOfField = await ReadNumOfFieldInCurrentFile();
			if (!resNumOfField.IsOk) {
				Log.Debug ("[Reader - ReadCurrentFile] フィールド数が0のため、ファイルの読み込みを終了します。");
				return result;
			} else {
				Log.Debug("[Reader - ReadCurrentFile] フィールド数 ... " + resNumOfField.Value );
			}
			for (var i = 0; i < resNumOfField.Value; i++) {
				var resField = await ReadFieldOfCurrentFile (i);
				if (resField.IsOk) {
					result.Add (resField.Value);
				}
			}
			return result;
		}

        public async Task ClearCurrentFile() {
            // シリアル設定は無視
            await SetNumOfFieldOfCurrentMarkingFile(0);
            await ClearPermanentMarkingFileNo();
        }
	}
}

