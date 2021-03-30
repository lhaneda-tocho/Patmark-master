using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
	{
        /// <summary>
        /// Making with button of head.
        /// For MB1 controller.
        /// </summary>
        /// <param name="fields">Fields.</param>
		public async Task<bool> SetupMBModeMarking(List<MBData> fields){
			//
			{
				var res = await SetMarkingMode (MBMarkingMode.Permanent);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetMarkingMode");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetMarkingMode");
					return false;
				}
			}

			// 
			{
				var res = await SetRemoteMarkingFileNo (MBRemoteMarkingFileNo.PC);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetRemoteMarkingFileNo");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetRemoteMarkingFileNo");
					return false;
				}
			}

			// 
			{
                var res = await SetAlert (MBAlert.None);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetAlert");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetAlert");
					return false;
				}
			}

			// 
			{
				var res = await SetMarkingPausingStatus (MBMarkingPausingStatus.None);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetMarkingPausingStatus");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetMarkingPausingStatus");
					return false;
				}
			}

			// 
			{
				var res = await SetHeadButtonMarkingAbility (MBHeadButtonMarkingAbility.Enabled);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetHeadButtonMarkingAbility");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetHeadButtonMarkingAbility");
					return false;
				}
			}

			// 
			{
				var res = await ClearPermanentMarkingFileNo();
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... ClearPermanentMarkingFileNo");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... ClearPermanentMarkingFileNo");
					return false;
				}
			}

			// 
			{
				var res = await SetPermanentMarkingFileNoToSdCard ();
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetMBModeFileMarkingFileNoToSdCard");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetMBModeFileMarkingFileNoToSdCard");
					return false;
				}
			}

			// 
			for (var i = 0; i < fields.Count; i++) {
				var res = await SetCurrentMarkingField (i, fields[i]);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetCurrentMarkingField");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetCurrentMarkingField");
					return false;
				}
			}

			// 
			{
				var res = await SetNumOfFieldOfCurrentMarkingFile ((short)fields.Count);
				if (res.IsOk) {
					Console.WriteLine ("[SetupMBModeMarking] 成功しました ... SetNumOfFieldOfCurrentMarkingFile");
				} else {
					Console.WriteLine ("[SetupMBModeMarking] 失敗しました ... SetNumOfFieldOfCurrentMarkingFile");
					return false;
				}
			}

			// シリアルカウンタ値を書き込む
			// TODO

			return true;
		}
	}
}

