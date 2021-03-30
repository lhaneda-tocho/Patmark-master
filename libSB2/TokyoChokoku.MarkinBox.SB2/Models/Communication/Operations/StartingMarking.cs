using System;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
		public static async Task<bool> StartMarking(Action<bool> pauseCallback){
			
			// 打刻開始
			{
				var res = await SetMarkingStatus (MBMemories.MarkingStatus.Start);
				if (res.IsOk) {
					Log.Debug ("[StartingMarking - Execute] 成功しました ... SettingMarkingStatus");
				}else{
					Log.Debug ("[StartingMarking - Execute] 失敗しました ... SettingMarkingStatus");
					return false;
				}
			}

            await Task.Delay(200);

			// 打刻終了を待機
			while(true)
			{
				// アラートを取得
				{
                    var res = await ReadAlert();
                    if(res.IsOk){
						if (res.Value != (short)MBMemories.Alert.None)
						{
							Log.Debug("[StartingMarking - Execute] 打刻中にアラート発生 ... " + res.Value.ToString());
							return false;
						}
                    }
				}

                await Task.Delay(50);

				// 打刻状態を取得
				{
					var res = await ReadMarkingStatus();
                    if (res.IsOk)
                    {
                        Log.Debug("[StartingMarking - Execute] 打刻ステータス ... " + res.Value);
                        if (res.Value == (short)MBMemories.MarkingStatus.Stop)
                        {
                            Log.Debug("[StartingMarking - Execute] 打刻終了");
                            break;
                        }
                        else
                        {
                            Log.Debug("[StartingMarking - Execute] 打刻継続");
                        }
                    }
				}

                await Task.Delay(50);

				// ポーズ状態を取得
				{
					var res = await ReadMarkingPausingStatus();
                    if (res.IsOk)
                    {
                        if (res.Value == (short)MBMemories.MarkingPausingStatus.Pause)
                        {
                            // ポーズ中
                            Log.Debug("[StartingMarking - Execute] ポーズ中");
                            pauseCallback(true);
                        }
                        else if (res.Value == (short)MBMemories.MarkingPausingStatus.Stop)
                        {
                            // 停止
                            Log.Debug("[StartingMarking - Execute] 打刻終了（停止）");
                            return false;
                        }
                        else
                        {
                            pauseCallback(false);
                        }
                    }
				}

                await Task.Delay(500);
			}

            await Task.Delay(50);

			// 打刻結果を取得
			{
				var res = await ReadMarkingResult ();
                if (res.IsOk)
                {
                    Log.Debug("[StartingMarking - Execute] 打刻結果 ... " + res.Value.ToString());
                    if (res.Value != (short)MBMemories.MarkingResult.Success)
                    {
                        return false;
                    }
                }
			}

            await Task.Delay(50);

            // ファイル番号をクリア
            if ((await SetRemoteMarkingFileNo(0)).IsOk) {
                Log.Debug ("[StartMarking - Execute] 成功しました ... SetRemoteMarkingFileNo(0)");
            } else {
                Log.Debug ("[StartMarking - Execute] 失敗しました ... SetRemoteMarkingFileNo(0)");
                return false;
            }

			return true;
		}
	}
}

