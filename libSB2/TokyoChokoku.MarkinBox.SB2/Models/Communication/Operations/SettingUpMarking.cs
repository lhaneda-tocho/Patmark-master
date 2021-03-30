using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
		public static async Task<bool> SetUpMarking(List<MBData> fields, bool isTestMode){
			
			if (fields == null || fields.Count == 0) {
				Log.Debug ("[StartingMarking - Execute] フィールド数が0のため、打刻をスキップします。");
				return false;
			}

			//
            if ((await SetMarkingMode(MBMemories.MarkingMode.Pc)).IsOk) {
				Log.Debug ("[StartingMarking - Execute] 成功しました ... SettingMarkingMode");
			} else{
				Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingMarkingMode");
				return false;
			}

			// 
            if ((await SetRemoteMarkingFileNo(MBMemories.RemoteMarkingFileNoPC)).IsOk) {
				Log.Debug ("[StartingMarking - Execute] 成功しました ... SettingRemoteMarkingFileNo");
			} else {
				Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingRemoteMarkingFileNo");
				return false;
			}

            // 
            if ((await ClearPermanentMarkingFileNo()).IsOk)
            {
                Log.Debug("[SetupMBModeMarking] 成功しました ... ClearPermanentMarkingFileNo");
            }
            else {
                Log.Debug("[SetupMBModeMarking] 失敗しました ... ClearPermanentMarkingFileNo");
                return false;
            }

            // 
            if ((await SetPermanentMarkingFileNoToSdCard()).IsOk)
            {
                Log.Debug("[SetupMBModeMarking] 成功しました ... SetMBModeFileMarkingFileNoToSdCard");
            }
            else {
                Log.Debug("[SetupMBModeMarking] 失敗しました ... SetMBModeFileMarkingFileNoToSdCard");
                return false;
            }

			// 
            if ((await SetAlert(MBMemories.Alert.None)).IsOk) {
				Log.Debug ("[StartingMarking - Execute] 成功しました ... SettingAlarm");
			} else {
				Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingAlarm");
				return false;
			}

			// 
            if ((await SetMarkingPausingStatus(MBMemories.MarkingPausingStatus.None)).IsOk) {
				Log.Debug ("[StartingMarking - Execute] 成功しました ... SettingPausingMarkingStatus");
			} else {
				Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingPausingMarkingStatus");
				return false;
			}

			// 
			for (var i = 0; i < fields.Count; i++) {
				Log.Debug ("[StartingMarking - Execute] 処理中です ... SettingCurrentMarkingField , " + i);
				try{
                    if ((await SetCurrentMarkingField(i, fields[i])).IsOk) {
						Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingCurrentMarkingField , " + i);
					}
					else{
						Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingCurrentMarkingField , " + i);
						return false;
					}
				}catch(Exception e){
					Log.Debug ("[StartingMarking - Execute] エラー発生 ... SettingCurrentMarkingField , " + i);
					Log.Debug(e.ToString());
					Log.Debug(e.StackTrace);
					return false;
				}
			}

			// 
            if (!(await SetNumOfFieldOfCurrentMarkingFile((short)fields.Count)).IsOk) {
				Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingNumOfFieldOfCurrentMarkingFile");
				return false;
			}

			// テストモードであれば、各フィールドの打刻力を 0 、スピードを 50 に。
			if (isTestMode) {
				for (var i = 0; i < fields.Count; i++) {
                    if ((await SetMarkingPowerOfCurrentMarkingField(i, 100)).IsOk) // FIXME: パワーの分解能が変わった時にメンテナンスしてください
                        
                    {
                        Log.Debug("[StartingMarking - Execute] 成功しました ... SettingMarkingPowerOfCurrentMarkingField , " + i);
                    }
                    else {
                        Log.Debug("[StartingMarking - Execute] 失敗しました ... SettingMarkingPowerOfCurrentMarkingField , " + i);
                        return false;
                    }
                    // 
                    if ((await SetMarkingSpeedOfCurrentMarkingField(i, 50)).IsOk)
                    {
                        Log.Debug("[StartingMarking - Execute] 成功しました ... SettingMarkingSpeedOfCurrentMarkingField , " + i);
                    }
                    else {
                        Log.Debug("[StartingMarking - Execute] 失敗しました ... SettingMarkingSpeedOfCurrentMarkingField , " + i);
                        return false;
                    }


				}
			}

			return true;
		}
	}
}

