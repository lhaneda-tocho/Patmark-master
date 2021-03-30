using System;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	partial class CommandExecuter
	{
        public static async Task<bool> LoadFileNameAndMapOnSdCard(){
            if ((await ReadFlagFileLoadedFromSdCard()).Value == 0)
            {
                await Task.Delay(50);
                await SetFlagFileLoadedFromSdCard(1);
                await Task.Delay(100);
                await LoadFileMapFromSdCard();
                await Task.Delay(100);
                await LoadFileNamesFromSdCard();
                await Task.Delay(100);
            }
            return true;
		}
	}
}

