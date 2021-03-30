using System;
using System.Threading.Tasks;

namespace TokyoChokoku.Communication
{
	partial class CommandExecutor
	{
        public async Task<bool> LoadFileNameAndMapOnSdCardIfNeeded(){
            if ((await ReadFlagFileLoadedFromSdCard()).Value == 0)
            {
                await LoadFileMapFromSdCard();
                await Task.Delay(500);
                await LoadFileNamesFromSdCard();
                await Task.Delay(800);
                await SetFlagFileLoadedFromSdCard(1);
                await Task.Delay(50);
            }
            return true;
		}
	}
}

