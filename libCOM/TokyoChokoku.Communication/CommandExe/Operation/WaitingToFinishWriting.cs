using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.Communication
{
    public partial class CommandExecutor
	{
        const int TimeOut = 3000;

		async Task<bool> WaitToFinishWriting(WritingCommandBuilder builder){
            
            var first = DateTime.Now.Millisecond;

            while (
                Living(
                    builder.Data.GetAllBytes(),
                    (await ReadCommandSurvival(builder)).Data.GetAllBytes()
                )
            )
            {
                var delta = (long)Math.Abs(DateTime.Now.Millisecond - first);
                if (delta < TimeOut) {
                    Console.WriteLine("[WaitToFinishWriting - Execute] コマンド「" + builder + "」の終了を待機しています。(time=" + delta + ")");
                } else {
                    Console.WriteLine ("[WaitToFinishWriting - Execute] コマンド「"+ builder +"」の終了待機をタイムアウトします。");
                    return false;
                }
            }

			return true;
		}


        bool Living(byte[] ValueSent, byte[] res)
        {
            var sendLen = ValueSent.Length;
            var resLen  = res.Length;

			Console.WriteLine(String.Format("Check Living Length S/R = {0}/{1}", sendLen, resLen));
            Console.WriteLine("ByteDump of S/R:");
            BinalizerUtil.DumpBytes(ValueSent.ToArray());
            BinalizerUtil.DumpBytes(res);

            if (sendLen == 0 || resLen == 0)
            {
                Console.WriteLine("EMPTY");
                return false;
            }

            for (var i = 0; i < sendLen && i < resLen; i++)
            {
                if (res[i] != ValueSent[i])
                {
                    Console.WriteLine("Not matched.");
                    return false;
                }
            }
            return true;
        }
	}
}

