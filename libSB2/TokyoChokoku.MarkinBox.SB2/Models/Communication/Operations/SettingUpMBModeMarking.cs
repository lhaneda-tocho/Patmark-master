using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
        /// <summary>
        /// Making with button of head.
        /// For MB1 controller.
        /// </summary>
        /// <param name="fields">Fields.</param>
		public static async Task<bool> SetupMBModeMarking(List<MBData> fields){

			//
			{
				var res = await SetMarkingMode (MBMemories.MarkingMode.Permanent);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetMarkingMode");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetMarkingMode");
					return false;
				}
			}

			// 
			{
				var res = await SetRemoteMarkingFileNo (MBMemories.RemoteMarkingFileNoPC);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetRemoteMarkingFileNo");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetRemoteMarkingFileNo");
					return false;
				}
			}

			// 
			{
                var res = await SetAlert (MBMemories.Alert.None);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetAlert");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetAlert");
					return false;
				}
			}

			// 
			{
				var res = await SetMarkingPausingStatus (MBMemories.MarkingPausingStatus.None);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetMarkingPausingStatus");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetMarkingPausingStatus");
					return false;
				}
			}

			// 
			{
				var res = await SetHeadButtonMarkingAbility (MBMemories.HeadButtonMarkingAbility.Enabled);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetHeadButtonMarkingAbility");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetHeadButtonMarkingAbility");
					return false;
				}
			}

			// 
			{
				var res = await ClearPermanentMarkingFileNo();
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... ClearPermanentMarkingFileNo");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... ClearPermanentMarkingFileNo");
					return false;
				}
			}
			// 
			{
				var res = await SetPermanentMarkingFileNoToSdCard ();
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetMBModeFileMarkingFileNoToSdCard");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetMBModeFileMarkingFileNoToSdCard");
					return false;
				}
			}

			// 
			for (var i = 0; i < fields.Count; i++) {
				var res = await SetCurrentMarkingField (i, fields[i]);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetCurrentMarkingField");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetCurrentMarkingField");
					return false;
				}
			}

			// 
			{
				var res = await SetNumOfFieldOfCurrentMarkingFile ((short)fields.Count);
				if (res.IsOk) {
					Log.Debug ("[SetupMBModeMarking] 成功しました ... SetNumOfFieldOfCurrentMarkingFile");
				} else {
					Log.Debug ("[SetupMBModeMarking] 失敗しました ... SetNumOfFieldOfCurrentMarkingFile");
					return false;
				}
			}

			// シリアルカウンタ値を書き込む
			// TODO

			return true;
		}
	}
}

