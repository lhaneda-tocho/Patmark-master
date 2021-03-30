using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
		public static async Task<List<MBData>> ReadCurrentFile(){
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
	}
}

