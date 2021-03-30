using System;
using System.Threading.Tasks;

namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
	{
		public async Task<bool> SaveMachineModelNo(short number){

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

