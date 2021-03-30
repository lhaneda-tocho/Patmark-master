using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
        const int TimeOut = 3000;

		public static async Task<bool> WaitToFinishWriting(WritingCommandBuilder builder){
            
            var first = DateTime.Now.Millisecond;

            while (
                Living(
                    builder.ReadOnlyDatas,
                    (await ReadCommandSurvival(builder)).Value
                )
            )
            {
                if ((DateTime.Now.Millisecond - first) < TimeOut) {
                    Log.Debug ("[WaitToFinishWriting - Execute] コマンド「"+ builder.ToString() +"」の終了を待機しています。");
                } else {
                    Log.Debug ("[WaitToFinishWriting - Execute] コマンド「"+ builder.ToString() +"」の終了待機をタイムアウトします。");
                    return false;
                }
            }

			return true;
		}


        private static bool Living(ReadOnlyCollection<byte> ValueSent, List<byte> res)
        {
            if (ValueSent.Count == 0 || res.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < res.Count && i < ValueSent.Count; i++)
            {
                if (res[i] != ValueSent[i])
                {
                    return false;
                }
            }
            return true;
        }
	}
}

