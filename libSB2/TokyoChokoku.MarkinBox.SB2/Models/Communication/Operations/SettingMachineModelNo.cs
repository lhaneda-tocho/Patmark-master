using System;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
		public static async Task<bool> SaveMachineModelNo(short number){

			//
			{
				var res = await SetMachineModelNo (number);
				if (!res.IsOk)
					return false;
			}

			// 
			{
				var res = await SetMachineModelNoToSdCard ();
				if(!res.IsOk)
					return false;
			}

			return true;
		}
	}
}

