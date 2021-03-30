using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    partial class CommandExecuter
	{
        public static async Task<bool> SaveFileMapToSdCard(int fileIndex, int numOfField){

            //// C[6010]~
            //await SetSdCardDataWritingInfo(
            //    Addresses.SdCard.FileMapBlock.Start,
            //    Addresses.FileMapWorkSpace.Address,
            //    Sizes.SdCard.BytesOfTransferUnit
            //);
            //await LoadFileMapBlockFromSdCard();
            //await SetFileMapToWorkSpace(fileIndex, numOfField);
            //await SaveFileMapBlockToSdCard();

            // R[1000]~
            await SetRemoteFileMap(fileIndex, (short)numOfField);

            return true;
		}
	}
}

